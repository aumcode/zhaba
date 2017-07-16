/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks = (function() {
    "use strict";
    var published = {},
        fRosterGrid = WAVE.id('roster'),
        fREC,
        fRVIEW,
        fTasks,
        fCategories,
        fScheduleTimer,
        fIsPM = false,
        fTasksDetailsState = {}, //hranit otkritie/zakritie details //TODO: pereimenovat!!!
        fTasksDetailsList = [],
        fStatuses = { N: 'New', R: 'Reopen', A: 'Assign', D: 'Done', F: 'Defer', C: 'Close', X: 'Cancel' };


    function clearRosterGrid() {
        WAVE.each(fTasksDetailsList, function(element) {
            element.eventClear();
            element = null; //TODO рассмотреть процесс очистки памяти
        });
        fTasksDetailsList = [];
        fRosterGrid.innerHTML = "";
    }
    
    function buildAreasAndComponents(root, task) {
        var i, l;
        for (i = 0, l = task.Areas.length; i < l; i++) {
            ZHB.Tasks.Render.buildAreaTag(root, task.Counter, task.Areas[i].Counter, task.Areas[i].Name);
        }
        for (i = 0, l = task.Components.length; i < l; i++) {
            ZHB.Tasks.Render.buildCompTag(root, task.Counter, task.Components[i].Counter, task.Components[i].Name);
        }
    }
    
    function buildAssigneeList(root, task) {
        for (var i = 0, l = task.AssigneeList.length; i < l; i++) {
            ZHB.Tasks.Render.buildAssignee(root, task.AssigneeList[i]);
        }
    }

    function createTabs(root, task) {
        var statusId = "status-" + task.Counter;
        var statusContainer = "<div id={0}></div>".args(statusId);

        var assignmentId = "assignment-" + task.Counter;
        var assignmentContainer = "<div id={0}></div>".args(assignmentId);

        var chatId = "chatTab-{0}".args(task.Counter);
        var chatContainer = "<div id={0}></div>".args(chatId);

        var areasId = "areasTab-{0}".args(task.Counter);
        var areasContainer = "<div id={0}></div>".args(areasId);

        var componentsId = "componentsTab-{0}".args(task.Counter);
        var componentsContainer = "<div id={0}></div>".args(componentsId);

        var tabs = new WAVE.GUI.Tabs({
            DIV: WAVE.id(root),
            tabs: [{
                    name: "tStatus",
                    title: "Status",
                    content: statusContainer,
                    visible: true,
                    isHtml: true
                },
                {
                    name: "tAssignment",
                    title: "Assignment",
                    content: assignmentContainer,
                    isHtml: true
                },
                {
                    name: "tChat",
                    title: "Chat",
                    content: chatContainer,
                    isHtml: true
                },
                {
                    name: "tAreas",
                    title: "Areas",
                    content: areasContainer,
                    isHtml: true
                },
                {
                    name: "tComponents",
                    title: "Components",
                    content: componentsContainer,
                    isHtml: true
                }
            ]
        });
        tabs.eventBind(WAVE.GUI.EVT_TABS_TAB_CHANGED, function(sender, args) {
            if (args === "tChat") {
                ZHB.Tasks.Chat.refreshChat(task);
            }
        });

        ZHB.Tasks.Status.buildStatusTab(statusId, task);
        buildAssignmentTab(assignmentId, task);
        ZHB.Tasks.Chat.buildChatTab(chatId, task);
        buildAreasTab(areasId, task);
        buildComponentsTab(componentsId, task);
    }
    
    function renderTasks() {
        clearRosterGrid();
        createHeaders(fRosterGrid);
        WAVE.each(fTasks, function(task) {
            ZHB.Tasks.Render.createRow(fRosterGrid, task);
            buildAreasAndComponents('ac' + task.Counter, task);
            buildAssigneeList('assignee' + task.Counter, task);
            ZHB.Tasks.Render.createRowDetails(fRosterGrid, task.Counter);
            createTabs("tabs-" + task.Counter, task);
            document.getElementById('description' + task.Counter).innerHTML = WAVE.markup(WAVE.strDefault(task.Description));
        });
    }

    function taskDetailsShowHandler(sender, args) {
        if (args.phase === WAVE.RecordModel.EVT_PHASE_AFTER)
            fTasksDetailsState[sender.detailsId] = true;
    }

    function taskDetailsHideHandler(sender, args) {
        if (args.phase === WAVE.RecordModel.EVT_PHASE_AFTER)
            fTasksDetailsState[sender.detailsId] = false;
    }

    function initDetails() {
        var expanders = document.getElementsByClassName("rst-expander");
        for (var i = 0, l = expanders.length; i < l; i++) {
            var detailsId = expanders[i].dataset.detailsId,
                content = document.getElementById(detailsId),
                details = new WAVE.GUI.Details({
                    titleCtrl: expanders[i],
                    contentCtrl: content,
                    hideOnClick: false
                });
            fTasksDetailsList.push(details);
            details.detailsId = detailsId;

            if (fTasksDetailsState[detailsId])
                setTimeout(details.show, 1);

            details.eventBind(WAVE.GUI.EVT_DETAILS_SHOW, taskDetailsShowHandler);
            details.eventBind(WAVE.GUI.EVT_DETAILS_HIDE, taskDetailsHideHandler);
        }
    }

    function getTasks() {
        WAVE.ajaxPostJSON(
            ZHB.URIS.ForDASHBOARD_TASKS(),
            fREC.data(),
            function(resp) {
                var data = JSON.parse(resp);
                fTasks = data.Rows;
                renderTasks("roster");
                initDetails();
                ZHB.Tasks.Chat.init({tasks: fTasks});
            },
            ZHB.errorLog,
            ZHB.errorLog
        );
    }

    function scheduleFetch() {
        if (fScheduleTimer) clearTimeout(fScheduleTimer);
        getTasks();
        fScheduleTimer = setTimeout(scheduleFetch, 300000);
    }

    function initFilter(filter) {
        WAVE.GUI.SUPPRESS_UNLOAD_CHECK = false;
        fREC = new WAVE.RecordModel.Record(filter);
        fRVIEW = new WAVE.RecordModel.RecordView("V1", fREC);

        fREC.eventBind(WAVE.RecordModel.EVT_DATA_CHANGE, function(sender, phase, oldv, newv) {
            if (phase === WAVE.RecordModel.EVT_PHASE_AFTER) scheduleFetch();
        });
    }
 
    published.scheduleFetch = function() { scheduleFetch(); };
    
    published.init = function(init) {
        ZHB.Tasks.Render.init({});
        ZHB.Tasks.Status.init({});
        initFilter(init.filter);
        published.isPM = init.pmPerm;
        scheduleFetch();
    };
    
    return published;
})();
