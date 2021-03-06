﻿/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks = (function () {
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
    fTasksTabsState = {},
    fTasksTabsList = [],
    fTick = 30000,
    fCurrentUser = 0
  ;


  function clearRosterGrid() {
    WAVE.each(fTasksTabsList, function (element) {
      element.eventClear();
      element = null; //TODO рассмотреть процесс очистки памяти
    });

    WAVE.each(fTasksDetailsList, function (element) {
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

    var tabsArray = [{
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
      }
    ];
    if (ZHB.Tasks.isPM) tabsArray.push(
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
    );

    var tabs = new WAVE.GUI.Tabs({
      DIV: WAVE.id(root),
      tabs: tabsArray
    });
    fTasksTabsList.push(tabs);

    if (fTasksTabsState[task.Counter]) {
      tabs.tabActive(fTasksTabsState[task.Counter]);
    }

    tabs.eventBind(WAVE.GUI.EVT_TABS_TAB_CHANGED, function (sender, args) {
      fTasksTabsState[task.Counter] = args;
      if (args === "tChat") {
        ZHB.Tasks.Chat.refreshChat(task);
      }
    });

    ZHB.Tasks.Status.buildStatusTab(statusId, task);
    ZHB.Tasks.Assignment.buildAssignmentTab(assignmentId, task);
    ZHB.Tasks.Chat.buildChatTab(chatId, task);
    ZHB.Tasks.Areas.buildAreasTab(areasId, task);
    ZHB.Tasks.Components.buildComponentsTab(componentsId, task);
  }

  function renderTasks() {
    clearRosterGrid();
    ZHB.Tasks.Render.createHeaders(fRosterGrid);
    WAVE.each(fTasks, function (task) {
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
        setTimeout(details.show, 1);//will appear after rendering

      details.eventBind(WAVE.GUI.EVT_DETAILS_SHOW, taskDetailsShowHandler);
      details.eventBind(WAVE.GUI.EVT_DETAILS_HIDE, taskDetailsHideHandler);
    }
  }

  function getTasks() {
    WAVE.ajaxPostJSON(
      ZHB.URIS.ForDASHBOARD_TASKS(),
      fREC.data(),
      function (resp) {
        var data = JSON.parse(resp);
        fTick = 300000;
        fTasks = data.Rows;
        renderTasks("roster");
        initDetails();
        ZHB.Tasks.Chat.init({tasks: fTasks});
      },
      function (resp) {
        ZHB.errorLog(resp);
        fTick += 300000
      },
      function (resp) {
        ZHB.errorLog(resp);
        fTick += 300000
      }
    );
  }

  function scheduleFetch() {
    if (fScheduleTimer) clearTimeout(fScheduleTimer);
    getTasks();
    fScheduleTimer = setTimeout(scheduleFetch, fTick);
  }

  function initFilter(filter) {
    WAVE.GUI.SUPPRESS_UNLOAD_CHECK = false;
    fREC = new WAVE.RecordModel.Record(filter);
    fRVIEW = new WAVE.RecordModel.RecordView("V1", fREC);

    fREC.eventBind(WAVE.RecordModel.EVT_DATA_CHANGE, function (sender, phase, oldv, newv) {
      if (phase === WAVE.RecordModel.EVT_PHASE_AFTER) scheduleFetch();
    });
  }

  function changeProgress(pid, iid, progress, description) {
    var data = {
      issueCounter: iid,
      value: progress,
      description: description
    };
    // console.log(data);
    WAVE.ajaxCall(
      'POST',
      "/dashboard/changeprogress",
      data,
      function (resp) {
        ZHB.Tasks.scheduleFetch();
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  function refreshAreaTag(pid, iid) {
    var acId = 'ac' + iid;
    var link = ZHB.URIS.ForPROJECT_ISSUE_AREA(pid, iid);
    WAVE.ajaxCall(
      'POST',
      link,
      null,
      function (resp) {
        var _rec = JSON.parse(resp);
        var _rows = _rec.Rows;
        for (var i = 0, l = _rows.length; i < l; i++) {
          if (_rows[i][4]) ZHB.Tasks.Render.buildAreaTag(acId, iid, _rows[i][0], _rows[i][3]);
        }
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  function refreshComponentTag(pid, iid) {
    var acId = 'ac' + iid;
    var link = ZHB.URIS.ForPROJECT_ISSUE_COMPONENT(pid, iid);
    WAVE.ajaxCall(
      'POST',
      link,
      null,
      function (resp) {
        var _rec = JSON.parse(resp);
        var _rows = _rec.Rows;
        for (var i = 0, l = _rows.length; i < l; i++) {
          if (_rows[i][4]) ZHB.Tasks.Render.buildCompTag(acId, iid, _rows[i][0], _rows[i][3]);
        }
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  published.changeProgress1 = function (e) {
    var checkUser = e.target.dataset.checkuser;
    if (!JSON.parse(checkUser)) return;
    
    var status = e.target.dataset.status;
    if (status == 'C' || status == 'X') return;
    var pid = e.target.dataset.cproject;
    var iid = e.target.dataset.cissue;
    var rec = new WAVE.RecordModel.Record(
      {
        ID: "REC_PROGRESS",
        fields: [
          {def: {Name: 'Value', Type: 'int', MinValue: 0, MaxValue: 100}, val: e.target.dataset.progress},
          {def: {Name: 'Description', Type: 'string', Size: 512, Required: false}, val: e.target.dataset.description},
        ]
      }
    );
    e.stopPropagation();
    var dlg = new WAVE.GUI.Dialog({
      header: 'Change progress',
      body: ZHB.Tasks.Render.buildProgressBody(),
      footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
      onShow: function () {
        var rv = new WAVE.RecordModel.RecordView("V3", rec);
      },
      onClose: function (dlg, result) {
        if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
        rec.validate();
        if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
        changeProgress(pid, iid, rec.data().Value, rec.data().Description)
        return WAVE.GUI.DLG_CANCEL;
      }
    });
  };

  published.refreashTag = function (pid, iid) {
    var acId = 'ac' + iid;

    WAVE.id(acId).innerHTML = '';

    refreshAreaTag(pid, iid);
    refreshComponentTag(pid, iid);
  };

  published.scheduleFetch = function () {
    scheduleFetch();
  };

  published.checkUser = function (task) {
    var result = false;
    if (!ZHB.Tasks.isPM) {
      var assigneeList = task.AssigneeList;
      for(var i=0, l= assigneeList.length ; i < l; i++) {
        if(assigneeList[i].C_User == fCurrentUser) {
          result = true;
          break;
        }  
      }        
    } else {
      result = true;
    }
    return result;
  };

  published.init = function (init) {
    ZHB.Tasks.Render.init({});
    ZHB.Tasks.Status.init({});
    initFilter(init.filter);
    published.isPM = init.pmPerm;
    fCurrentUser = init.currentUser;
    scheduleFetch();
  };

  return published;
})();
