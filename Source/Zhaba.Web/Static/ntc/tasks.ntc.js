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

    function renderTasks() {
        clearRosterGrid();
        createHeaders(fRosterGrid);
        WAVE.each(fTasks, function(task) {
            createRow(fRosterGrid, task);
            buildAreasAndComponents('ac' + task.Counter, task);
            buildAssigneeList('assignee' + task.Counter, task);
            createRowDetails(fRosterGrid, task.Counter);
            createTabs("tabs-" + task.Counter, task);
            document.getElementById('description' + task.Counter).innerHTML = WAVE.markup(WAVE.strDefault(task.Description));
        });
    }

    function taskDetailsShowHandler(sender, args) {
        if (args.phase == WAVE.RecordModel.EVT_PHASE_AFTER)
            fTasksDetailsState[sender.detailsId] = true;
    }

    function taskDetailsHideHandler(sender, args) {
        if (args.phase == WAVE.RecordModel.EVT_PHASE_AFTER)
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
            if (phase == WAVE.RecordModel.EVT_PHASE_AFTER) scheduleFetch();
        });
    }

    published.init = function(init) {
        initFilter(init.filter);
        getTasks();
    };

    published.scheduleFetch = function() { scheduleFetch(); };

    return published;
})();