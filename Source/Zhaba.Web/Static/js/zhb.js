﻿/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $ */


var ZHB = (function() {
    "use strict";

    var published = {};

    published.ControlScripts = {}; //ns for dynamic control scripts

    function project_setup(pid) { return "/project/{0}".args(pid); }

    function issue_setup(pid, iid) { return project_setup(pid) + "/issue/{0}".args(iid); }

    function common_setup() { return "/common"; }

    published.URIS = {
        ForCOMPONENT: function(id) {
            return common_setup() + "/component?id={0}".args(id);
        },

        ForAREA: function(id) {
            return common_setup() + "/area?id={0}".args(id);
        },

        ForPROJECT_SELECT: function(pid) {
            return project_setup(pid) + "/select";
        },

        ForPROJECT: function(id) {
            return common_setup() + "/project?id={0}".args(id);
        },

        ForCATEGORY: function(id) {
            return common_setup() + "/category?id={0}".args(id);
        },

        ForPROJECT_MILESTONE: function(pid, id) {
            return project_setup(pid) + "/milestone?id={0}".args(id);
        },

        ForPROJECT_COMPONENT: function(pid, id) {
            return project_setup(pid) + "/component?counter={0}".args(id);
        },

        ForPROJECT_AREA: function(pid, id) {
            return project_setup(pid) + "/area?counter={0}".args(id);
        },

        ForPROJECT_ISSUE: function(pid, id) {
            return project_setup(pid) + "/issue?id={0}".args(id);
        },

        ForPROJECT_DELETE_COMPONENT: function(project, counter) {
            return project_setup(project) + "/component?counter={0}".args(counter);
        },

        ForPROJECT_DELETE_AREA: function(project, counter) {
            return project_setup(project) + "/area?counter={0}".args(counter);
        },

        ForPROJECT_ISSUE_AREA: function(project, issue) {
            return project_setup(project) + "/issuearea?issue={0}".args(issue);
        },

        ForPROJECT_ISSUE_COMPONENT: function(project, issue) {
            return project_setup(project) + "/issuecomponent?issue={0}".args(issue);
        },

        ForPROJECT_LINK_ISSUE_AREA: function(project, issue, area) {
            return project_setup(project) + "/linkissuearea?issue={0}&area={1}".args(issue, area);
        },

        ForPROJECT_LINK_ISSUE_COMPONENT: function(project, issue, component) {
            return project_setup(project) + "/linkissuecomponent?issue={0}&component={1}".args(issue, component);
        },

        ForUSER: function(id) {
            return common_setup() + "/user?id={0}".args(id);
        },

        ForDELETE_CATEGORY: function(id) {
            return common_setup() + "/category?id={0}".args(id);
        },

        ForISSUE_ISSUEASSIGN: function(pid, iid, id) {
            return issue_setup(pid, iid) + "/issueassign?id={0}".args(id);
        },

        ForISSUE_ISSUECANCEL: function (pid, iid, id) {
          return issue_setup(pid, iid) + "/issuecancel?id={0}".args(id);
        },

        ForISSUE_STATUSNOTE: function(pid, iid, status) {
            return issue_setup(pid, iid) + "/statusnote?status={0}".args(status);
        },

        ForDELETE_ISSUE: function(pid, iid) {
            return issue_setup(pid, iid) + "/close";
        },

        ForREOPEN_ISSUE: function(pid, iid) {
            return issue_setup(pid, iid) + "/reopen";
        },

        ForDEFER_ISSUE: function(pid, iid) {
            return issue_setup(pid, iid) + "/defer";
        },

        ForDASHBOARD_TASKS: function() {
            return "/dashboard/tasks";
        },
        
        ForPROJECT_ISSUE_CHATLIST: function(pid, iid) {
            return issue_setup(pid, iid)+"/chatlist";
        },
        ForPROJECT_ISSUE_CHAT: function(pid, iid, cid) {
            return issue_setup(pid, iid)+"/chat?id={0}".args(cid);
        },
        ForDASHBOARD_CHANGESTATUS: function() {
            return "/dashboard/changestatus";
        },
        ForPROJECT_ISSUE_REPORT : function (pid, iid, report) {
            return issue_setup(pid, iid) +"/{0}".args(report);
        },
        ForREPORTS : function(report) {
            return "/reports/{0}".args(report);    
        },
        ForREPORTS_DUEITEMS : function () {
            return ZHB.URIS.ForREPORTS("dueitems");
        },
        ForREPORTS_DUEITEMS_VIEW : function (asOf, cProject) {
            return ZHB.URIS.ForREPORTS("dueitemsview")+"?AsOf={0}&C_Project={1}".args(asOf,  cProject);
        }
        
    };

    if (!String.prototype.args) {
        String.prototype.args = function() {
            var argts = arguments;
            return this.replace(/{(\d+)}/g, function(match, number) {
                return typeof argts[number] != 'undefined' ?
                    argts[number] :
                    match;
            });
        };
    }

    published.errorLog = function(response) {
        WAVE.GUI.toast("Something went wrong, see console for details");
        console.log(response);
    };
    
    published.debugLog = function(response) {
      //todo setup for debug only!
        console.log(response);
    };


    return published;
}());

/**
 * Created by mad on 13.07.2017.
 */
function linkIssueArea(event, cProject, cIssue, cArea) {
  event = event || window.event;

  var chk = event.currentTarget;

  var formData = new FormData();
  formData.append('link', chk.checked);

  var areaId = 'issue-' + cIssue + '-areatag-' + cArea;
  var acId = 'ac'+cIssue;

  $.ajax({
      url: ZHB.URIS.ForPROJECT_LINK_ISSUE_AREA(cProject, cIssue, cArea),
      type: 'POST',
      dataType: "json",
      data: formData,
      processData: false,
      contentType: false,
      success : function (resp) {
          var el = WAVE.id(acId);
          if (el) ZHB.Tasks.refreashTag(cProject, cIssue);
      }
    })
    .fail(function (xhr, txt, err) {
      WAVE.GUI.toast("The link could not be updated: " + xhr.status + "<br>" + err, "error");
      fetchData();
    });
}

function linkIssueComponent(event, cProject, cIssue, cComponent) {
  event = event || window.event;

  var chk = event.currentTarget;

  var formData = new FormData();
  formData.append('link', chk.checked);

  var compId = 'issue-' + cIssue + '-comptag-' + cComponent;
  var acId = 'ac'+cIssue;

  $.ajax({
      url: ZHB.URIS.ForPROJECT_LINK_ISSUE_COMPONENT(cProject, cIssue, cComponent),
      type: 'POST',
      dataType: "json",
      data: formData,
      processData: false,
      contentType: false,
      success : function (resp) {
          var el = WAVE.id(acId);
          if (el) ZHB.Tasks.refreashTag(cProject, cIssue);
      }
    })
    .fail(function (xhr, txt, err) {
      WAVE.GUI.toast("The link could not be updated: " + xhr.status + "<br>" + err, "error");
      fetchData();
    });
}

WAVE.onReady(function() {
  ZHB.ControlScripts.TableGrid = {
    rowSelect: function(tableElm, rowElm, key, data) {
      var tasksPage = WAVE.id('tasksPage');
      if (!tasksPage) {
        tableElm.SELECTED_ROW_KEY = key;
        tableElm.SELECTED_ROW_DATA = data;
        $(tableElm).find("tr").removeClass("selectedGridTableRow");
        $(rowElm).addClass("selectedGridTableRow");
        if (tableElm.onGridRowSelection)
          tableElm.onGridRowSelection(tableElm, key, data);
      }
    }
  };
});


/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Reports = (function () {
  var published = {}
  ;

  function viewReport(data) {
    debugger;
    
    var asOf = WAVE.dateTimeToString(data.AsOf, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
    var cProject = data.C_Project;
    
    var link = ZHB.URIS.ForREPORTS_DUEITEMS_VIEW(asOf, cProject);
    var strWindowFeatures = "location=yes,scrollbars=yes,status=yes";

    var win = window.open(link, "_blank", strWindowFeatures);
    
/*    WAVE.ajaxCall(
      'GET',
      link, 
      data,
      function (resp) {
        var win = window.open(link, "_blank", strWindowFeatures);
        win.document.write(resp)
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.TUNDEFINED,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );*/

  }

  published.DueItemsReportForm = function (e) {
    var link = ZHB.URIS.ForREPORTS_DUEITEMS();
    WAVE.ajaxCall(
      'GET',
      link,
      null,
      function (resp) {
        var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
        var dlg = new WAVE.GUI.Dialog({
          header: "Due Items Report",
          body: ZHB.Reports.Render.DueItemsReportForm(),
          footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
          onShow: function () {
            var rv = new WAVE.RecordModel.RecordView("V_DueItemsReport", _rec);
          },
          onClose: function (dlg, result) {
            if (result === WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
            _rec.validate();
            if (!_rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
            viewReport(_rec.data());
            return WAVE.GUI.DLG_CANCEL;
          }
        });
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );

  };

  return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Reports.Render = (function () {
    var published = {}
    ;

    published.DueItemsReportForm = function (root) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('id', 'DueItemsReport');
        Ø1.setAttribute('data-wv-rid', 'V_DueItemsReport');
        var Ø2 = WAVE.ce('div');
        Ø2.setAttribute('data-wv-fname', 'AsOf');
        Ø2.setAttribute('class', 'fView');
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('div');
        Ø3.setAttribute('data-wv-fname', 'C_Project');
        Ø3.setAttribute('data-wv-ctl', 'combo');
        Ø3.setAttribute('class', 'fView');
        Ø1.appendChild(Ø3);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;        
    };
    
    return published;
})();
/*jshint devel: true,browser: true, sub: true */
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

/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Render = (function () {
    "use strict";
    var published = {};

    function getStatusBarStyle(value) {
        var red = 0;
        var green = 0;
        if (value <= 25)
            red = 255;
        else if (value > 25 && value < 65) {
            red = 255;
            green = (value - 25) * 6;
        } else if (value == 65) {
            red = 255;
            green = 255;
        } else if (value > 65) {
            red = 255 - ((value - 65) * 7);
            green = 255;
        } else if (value == 100) {
            red = 0;
            green = 255;
        }
        return "width: {0}%; background: rgb({1},{2}, 0)".args(value, red, green);
    }


    function getPriorityStyle(value) {
        var priority;
        if (value === 0) {
            priority = "highest";
        } else if (value > 0 && value <= 3) {
            priority = "high";
        } else if (value > 3 && value <= 5) {
            priority = "middle";
        } else {
            priority = "lower";
        }
        return "priority-tag {0}".args(priority);
    }

    function buildDate(task) {
        var startDate = WAVE.dateTimeToString(task.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
        var completeDate = "OPEN";
        if (task.Complete_Date) {
            completeDate = WAVE.dateTimeToString(task.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
        }
        return "{0} - {1}".args(startDate, completeDate);
    }

    function getTimeRemainingStyle(remaining) {
      var style = "time-remaining-default";
      if (remaining <= 0) {
        style = "time-remaining-late";
      } else if (remaining > 0 & remaining < 7) {
        style = "time-remaining-week";
      } else if (remaining > 7 & remaining < 14) {
        style = "time-remaining-two-weeks";
      }
      return style;
    }

    function getStatusStyle(value) {
        return "status-tag {0}".args(value.toLowerCase());
    }

    published.getStatusStyle = function (value) {
        return getStatusStyle(value.toLowerCase());
    };

    published.createHeaders = function (root) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        if(1 == 1) {
          var Ø1 = WAVE.ce('div');
          Ø1.innerText = 'ID';
          Ø1.setAttribute('class', 'rst-cell rst-head');
          Ø1.setAttribute('style', 'width: 3%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
          var Ø2 = WAVE.ce('div');
          Ø2.innerText = 'Status';
          Ø2.setAttribute('class', 'rst-cell rst-head');
          Ø2.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
          var Ø3 = WAVE.ce('div');
          Ø3.innerText = 'Date';
          Ø3.setAttribute('class', 'rst-cell rst-head');
          Ø3.setAttribute('style', 'width: 12%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
          var Ø4 = WAVE.ce('div');
          Ø4.innerText = 'Assigned';
          Ø4.setAttribute('class', 'rst-cell rst-head');
          Ø4.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
          var Ø5 = WAVE.ce('div');
          Ø5.innerText = 'Areas\x2FComponents';
          Ø5.setAttribute('class', 'rst-cell rst-head');
          Ø5.setAttribute('style', 'width: 15%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
          var Ø6 = WAVE.ce('div');
          Ø6.innerText = 'Project';
          Ø6.setAttribute('class', 'rst-cell rst-head');
          Ø6.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
          var Ø7 = WAVE.ce('div');
          Ø7.innerText = 'Issue';
          Ø7.setAttribute('class', 'rst-cell rst-head');
          Ø7.setAttribute('style', 'width: 20%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
          var Ø8 = WAVE.ce('div');
          Ø8.innerText = 'Description';
          Ø8.setAttribute('class', 'rst-cell rst-head');
          Ø8.setAttribute('style', 'width: 20%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
        }
        return Ø8;
    };

    published.createRow = function (root, task) {
        var detailsId = "details-" + task.Counter;
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        if(1 == 1) {
          var Ø1 = WAVE.ce('div');
          Ø1.innerText = task.Counter;
          Ø1.setAttribute('class', 'rst-cell issue_id rst-expander');
          Ø1.setAttribute('style', 'width: 3%');
          Ø1.setAttribute('align', 'right');
          Ø1.setAttribute('data-cissue', task.Counter);
          Ø1.setAttribute('data-cproject', task.C_Project);
          Ø1.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
          var Ø2 = WAVE.ce('div');
          Ø2.setAttribute('class', 'rst-cell completeness rst-expander');
          Ø2.setAttribute('style', 'width: 10%');
          Ø2.setAttribute('data-details-id', detailsId);
        var Ø3 = WAVE.ce('div');
        var Ø4 = WAVE.ce('div');
        Ø4.setAttribute('align', 'center');
        var Ø5 = WAVE.ce('div');
        Ø5.innerText = task.Status;
        Ø5.setAttribute('class', 'tag {0} inline-block'.args(getStatusStyle(task.Status)));
        Ø4.appendChild(Ø5);
        var Ø6 = WAVE.ce('div');
        Ø6.innerText = task.Category_Name;
        Ø6.setAttribute('class', 'tag tag-category inline-block');
        Ø4.appendChild(Ø6);
        Ø3.appendChild(Ø4);
        var Ø7 = WAVE.ce('div');
        Ø7.innerText = task.Completeness +'%';
        Ø7.setAttribute('data-cproject', task.C_Project);
        Ø7.setAttribute('data-cissue', task.Counter);
        Ø7.setAttribute('data-progress', task.Completeness);
        Ø7.setAttribute('data-description', task.Description);
        Ø7.setAttribute('data-status', task.statusId);
        Ø7.setAttribute('data-checkuser', ZHB.Tasks.checkUser(task));
        Ø7.addEventListener('click', ZHB.Tasks.changeProgress1, false);
        Ø7.setAttribute('class', 'bar-value');
        Ø7.setAttribute('align', 'center');
        Ø3.appendChild(Ø7);
        var Ø8 = WAVE.ce('div');
        Ø8.setAttribute('class', 'bar');
        Ø8.setAttribute('style', getStatusBarStyle(task.Completeness));
        Ø3.appendChild(Ø8);
        Ø2.appendChild(Ø3);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
          var Ø9 = WAVE.ce('div');
          Ø9.setAttribute('class', 'rst-cell rst-text-align-center rst-expander');
          Ø9.setAttribute('data-details-id', detailsId);
          Ø9.setAttribute('style', 'width: 12%;');
        var Ø10 = WAVE.ce('div');
        Ø10.innerText = task.Priority;
        Ø10.setAttribute('class', 'tag {0} inline-block'.args(getPriorityStyle(task.Priority)));
        Ø9.appendChild(Ø10);
        var Ø11 = WAVE.ce('div');
        Ø11.innerText = buildDate(task);
        Ø11.setAttribute('class', 'inline-block');
        Ø9.appendChild(Ø11);
        var Ø12 = WAVE.ce('div');
        Ø12.innerText = WAVE.dateTimeToString(task.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
        if(!task.Complete_Date) {
          var Ø13 = WAVE.ce('div');
          Ø13.innerText = 'in {0}d'.args(task.Remaining);
          Ø13.setAttribute('class', 'time-r {0} inline-block'.args(getTimeRemainingStyle(task.Remaining)));
          Ø12.appendChild(Ø13);
        }
        Ø9.appendChild(Ø12);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø9);
          var Ø14 = WAVE.ce('div');
          Ø14.setAttribute('id', 'assignee'+task.Counter);
          Ø14.setAttribute('class', 'rst-cell rst-expander');
          Ø14.setAttribute('style', 'width: 10%');
          Ø14.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø14);
          var Ø15 = WAVE.ce('div');
          Ø15.setAttribute('id', 'ac'+task.Counter);
          Ø15.setAttribute('class', 'rst-cell rst-expander');
          Ø15.setAttribute('style', 'width: 15%');
          Ø15.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø15);
          var Ø16 = WAVE.ce('div');
          Ø16.innerText = task.ProjectName;
          Ø16.setAttribute('class', 'rst-cell rst-expander');
          Ø16.setAttribute('style', 'width: 10%');
          Ø16.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø16);
          var Ø17 = WAVE.ce('div');
          Ø17.innerText = task.Name;
          Ø17.setAttribute('class', 'rst-cell rst-expander');
          Ø17.setAttribute('style', 'width: 20%');
          Ø17.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø17);
          var Ø18 = WAVE.ce('div');
          Ø18.innerText = task.Description;
          Ø18.setAttribute('id', 'description'+task.Counter);
          Ø18.setAttribute('class', 'rst-cell rst-expander');
          Ø18.setAttribute('style', 'width: 20%');
          Ø18.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø18);
        }
        return Ø18;
    };

    published.buildAreaTag = function (root, cIssue, cArea, areaName) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.innerText = areaName;
        Ø1.setAttribute('class', 'tag tag-area inline-block');
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.buildCompTag = function (root, cIssue, cComp, compName) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.innerText = compName;
        Ø1.setAttribute('class', 'tag tag-component inline-block');
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.buildAssignee = function (root, assignee) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.innerText = assignee.UserLogin;
        Ø1.setAttribute('class', 'tag tag-assignee inline-block');
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.createRowDetails = function (root, id) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('id', 'details-'+id);
        Ø1.setAttribute('class', 'rst-cell rst-full');
        var Ø2 = WAVE.ce('div');
        Ø2.setAttribute('id', 'tabs-'+id);
        Ø2.setAttribute('class', 'tab-control');
        Ø1.appendChild(Ø2);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };


    published.createGrid = function (root, gridId) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('id', gridId);
        Ø1.setAttribute('class', 'rst-table');
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.buildIssueBody = function () {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('id', 'editIssueForm');
        Ø1.setAttribute('data-wv-rid', 'V6');
        var Ø2 = WAVE.ce('div');
        Ø2.setAttribute('class', 'fView');
        Ø2.setAttribute('data-wv-fname', 'Name');
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('div');
        Ø3.setAttribute('class', 'fView');
        Ø3.setAttribute('data-wv-fname', 'Start_Date');
        Ø1.appendChild(Ø3);
        var Ø4 = WAVE.ce('div');
        Ø4.setAttribute('class', 'fView');
        Ø4.setAttribute('data-wv-fname', 'Due_Date');
        Ø1.appendChild(Ø4);
        var Ø5 = WAVE.ce('div');
        Ø5.setAttribute('class', 'fView');
        Ø5.setAttribute('data-wv-fname', 'C_Milestone');
        Ø5.setAttribute('data-wv-ctl', 'combo');
        Ø1.appendChild(Ø5);
        var Ø6 = WAVE.ce('div');
        Ø6.setAttribute('class', 'fView');
        Ø6.setAttribute('data-wv-fname', 'C_Category');
        Ø6.setAttribute('data-wv-ctl', 'combo');
        Ø1.appendChild(Ø6);
        var Ø7 = WAVE.ce('div');
        Ø7.setAttribute('class', 'fView');
        Ø7.setAttribute('data-wv-fname', 'Priority');
        Ø1.appendChild(Ø7);
        var Ø8 = WAVE.ce('div');
        Ø8.setAttribute('class', 'fView');
        Ø8.setAttribute('data-wv-fname', 'Description');
        Ø1.appendChild(Ø8);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.buildProgressBody = function () {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('id', 'progressForm');
        Ø1.setAttribute('data-wv-rid', 'V3');
        var Ø2 = WAVE.ce('div');
        Ø2.setAttribute('data-wv-fname', 'Value');
        Ø2.setAttribute('class', 'fView');
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('div');
        Ø3.setAttribute('data-wv-fname', 'Description');
        Ø3.setAttribute('class', 'fView');
        Ø1.appendChild(Ø3);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.init = function (init) {

    };

    published.getPriorityStyle = function(priority) {
      return getPriorityStyle(priority);
    };

    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Status = (function () {
    "use strict";
    var published = {},
        CANCELLATION_WARNING = "WARNING! CANCEL is an irrevocable action and can not be un-done in future.",
        fStatuses = {N: 'New', R: 'Reopen', A: 'Assign', D: 'Done', F: 'Defer', C: 'Close', X: 'Cancel'}
    ;

    function buildIsueDialog(rec, pid, iid) {
        var dlg = new WAVE.GUI.Dialog({
            header: 'Issue',
            body: ZHB.Tasks.Render.buildIssueBody(),
            footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
            onShow: function () {
                var rv = new WAVE.RecordModel.RecordView("V6", rec);
            },
            onClose: function (dlg, result) {
                if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                rec.validate();
                if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
                var link = '/project/{0}/issue?id={1}'.args(pid, iid);
                WAVE.ajaxCall(
                    'POST',
                    link,
                    rec.data(),
                    function (resp) {
                        var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                        if (_rec.validationError()) {
                            WAVE.GUI.toast(_rec.validationError(), 'error');
                        } else {

                            ZHB.Tasks.scheduleFetch();
                            WAVE.GUI.currentDialog().close();
                        }
                    },
                    ZHB.errorLog,
                    ZHB.errorLog,
                    WAVE.CONTENT_TYPE_JSON_UTF8,
                    WAVE.CONTENT_TYPE_JSON_UTF8
                );
                return WAVE.GUI.DLG_UNDEFINED;
            }
        });
    }

    function editIssue(pid, iid) {
        var link = ZHB.URIS.ForPROJECT_ISSUE(pid, iid);//  '/project/{0}/issue?id={1}'.args(pid, iid);
        WAVE.ajaxCall(
            'GET',
            link,
            null,
            function (resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                buildIsueDialog(_rec, pid, iid);
            },
            ZHB.errorLog,
            ZHB.errorLog,
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function changeStatus(pid, iid, status, note) {
        var data = {
            C_Project: pid,
            C_Issue: iid,
            status: status,
            note: note
        }
        WAVE.ajaxCall(
            'POST',
            ZHB.URIS.ForDASHBOARD_CHANGESTATUS(),
            data,
            function (resp) {
                ZHB.Tasks.scheduleFetch();

            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function changeAssignStatus(pid, iid, status, data) {
        var link = ZHB.URIS.ForISSUE_ISSUEASSIGN(pid, iid, null);
        WAVE.ajaxCall(
            'POST',
            link,
            data,
            function (resp) {
                ZHB.Tasks.scheduleFetch();

            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function buildIssueAssignPopUpDialog(rec, pid, iid) {
        var dlg = new WAVE.GUI.Dialog({
            header: 'Change state to ' + fStatuses[status],
            body: ZHB.Tasks.Status.Render.buildIssueAssigneeStatusBody(),
            footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
            onShow: function () {
                var rv = new WAVE.RecordModel.RecordView("V2", rec);
            },
            onClose: function (dlg, result) {
                if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                rec.validate();
                if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED
                changeAssignStatus(pid, iid, status, rec.data());
                return WAVE.GUI.DLG_CANCEL;
            }
        });
    }
    
    function buildPopUpDialog(status, rec, pid, iid) {
      var dlg = new WAVE.GUI.Dialog({
        header: 'Change state to ' + fStatuses[status],
        body: ZHB.Tasks.Status.Render.buildStatusBody(),
        footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
        onShow: function () {
          var rv = new WAVE.RecordModel.RecordView("V2", rec);
        },
        onClose: function (dlg, result) {
          if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
          rec.validate();
          if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
          var note = rec.data().Description;
          changeStatus(pid, iid, status, note);
          return WAVE.GUI.DLG_CANCEL;
        }
      });
    }

    function buildPopUpCancelDialog(status, rec, pid, iid) {
        var dlg = new WAVE.GUI.Dialog({
            header: 'Change state to ' + fStatuses[status],
            body: ZHB.Tasks.Status.Render.buildStatusBody(status),
            footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
            onShow: function () {
                var rv = new WAVE.RecordModel.RecordView("V2", rec);
            },
            onClose: function (dlg, result) {
                if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                rec.validate();
                if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
                var dialogResult;
                WAVE.GUI.showConfirmationDialog(
                  'Warning!',
                  CANCELLATION_WARNING,
                  [WAVE.GUI.DLG_YES, WAVE.GUI.DLG_NO],
                  function (sender, result) {
                    if (result == WAVE.GUI.DLG_NO) {
                      dialogResult = WAVE.GUI.DLG_UNDEFINED;
                      return WAVE.GUI.DLG_NO;
                    }
                    var note = rec.data().Description;
                    changeStatus(pid, iid, status, note);
                    dialogResult = WAVE.GUI.DLG_CANCEL;
                    dlg.close();
                    return WAVE.GUI.DLG_YES;
                  });
                return dialogResult;
            }
        });
    }

    function getIssueCancelForm(pid, iid, iaid) {
      var link = ZHB.URIS.ForISSUE_ISSUECANCEL(pid, iid, iaid);
      WAVE.ajaxCall(
          'GET',
          link,
          null,
          function (resp) {
            var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
            buildPopUpCancelDialog("X", _rec, pid, iid);
          },
          function (resp) {
            console.log("error");
          },
          function (resp) {
            console.log("fail");
          },
          WAVE.CONTENT_TYPE_JSON_UTF8,
          WAVE.CONTENT_TYPE_JSON_UTF8
      );
    }
    
    function getIssueAssignForm(pid, iid, iaid) {
        var link = ZHB.URIS.ForISSUE_ISSUEASSIGN(pid, iid, iaid);
        WAVE.ajaxCall(
            'GET',
            link,
            null,
            function (resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                buildIssueAssignPopUpDialog(_rec, pid, iid);
            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function getOtherForm(status, pid, iid) {
        var link = ZHB.URIS.ForISSUE_STATUSNOTE(pid, iid, status);
        WAVE.ajaxCall(
            'GET',
            link,
            null,
            function (resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                buildPopUpDialog(status, _rec, pid, iid);
            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function changeStatusDialog(status, pid, iid) {
      if (status == 'A') {
        getIssueAssignForm(pid, iid, "");
      } else if (status == 'X') {
        getIssueCancelForm(pid, iid, "");
      } else {
        getOtherForm(status, pid, iid);
      }
    }

    published.addIssue = function (pid) {
        editIssue(pid, "");
    };

    published.editIssue1 = function (e) {
        e.stopPropagation();
        editIssue(e.target.dataset.cproject, e.target.dataset.cissue);
    };

    published.buildStatusTab = function (root, task) {
        var gridID = "status-grid-" + task.Counter;
        ZHB.Tasks.Status.Render.buildStatusButtons(root, task);
        ZHB.Tasks.Render.createGrid(root, gridID);
        ZHB.Tasks.Status.Render.createStatusHeader(gridID);
        for (var j = 0, l = task.Details.length; j < l; j++) {
            var d = task.Details[j];
            ZHB.Tasks.Status.Render.createStatusGridRow(gridID, d);
            document.getElementById('details-description' + d.Counter).innerHTML = WAVE.markup(d.Description != null ? d.Description : '');
        }
    };

    published.changeStatusDialog1 = function (e) {
        changeStatusDialog(e.target.dataset.nextstate, e.target.dataset.cproject, e.target.dataset.counter)
    };


    published.getOtherForm1 = function (e) {
        var status = e.target.dataset.nextstate;
        var pid = e.target.dataset.cproject;
        var iid = e.target.dataset.counter;
        getOtherForm(status, pid, iid);
    };

    published.editAssignee = function (e) {
        var pid = e.target.dataset.cproject;
        var iid = e.target.dataset.cissue;
        var id = e.target.dataset.cassignee;
        var link = "/project/{0}/issue/{1}/issueassign?id={2}".args(pid, iid, id);
        WAVE.ajaxCall(
            'GET',
            link,
            null,
            function (resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                _rec.fieldByName('Open_TS').readonly(true);
                _rec.fieldByName('C_User').readonly(true);
                var dlg = WAVE.GUI.Dialog({
                    header: "Unassignee for Issue [{0}]".args(iid),
                    body: ZHB.Tasks.Status.Render.buildIssueAssigneeStatusBody(),
                    footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
                    onShow: function () {
                        var rv = new WAVE.RecordModel.RecordView("V2", _rec);
                    },
                    onClose: function (dlg, result) {
                        if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                        _rec.validate();
                        if (!_rec.valid()) return WAVE.GUI.DLG_UNDEFINED
                        WAVE.ajaxCall(
                            'POST',
                            link,
                            _rec.data(),
                            function (resp) {
                                ZHB.Tasks.scheduleFetch();
                            },
                            function (resp) {
                                console.log("error");
                            },
                            function (resp) {
                                console.log("fail");
                            },
                            WAVE.CONTENT_TYPE_JSON_UTF8,
                            WAVE.CONTENT_TYPE_JSON_UTF8
                        );
                        return WAVE.GUI.DLG_CANCEL;
                    }

                });
            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    };
    
    published.getStatusButtonName = function (status) {
        return fStatuses[status];
    };

    published.init = function (init) {
        ZHB.Tasks.Status.Render.init({});
    };

    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Status.Render = (function () {
  "use strict";
  var published = {};

  published.buildStatusButtons = function (root, task) {
    var detailsId = "details-" + task.Counter;
    if (task.Status == 'Defer') {
      var resumeStatus;
      if (task.HasAssignee)
        resumeStatus = 'A';
      else {
        resumeStatus = 'N';
      }

      var Ør = arguments[0];
      if (WAVE.isString(Ør))
        Ør = WAVE.id(Ør);
      var Ø1 = WAVE.ce('div');
      if(ZHB.Tasks.isPM) {
        var Ø2 = WAVE.ce('a');
        Ø2.innerText = 'Edit Issue';
        Ø2.setAttribute('class', 'button');
        Ø2.setAttribute('style', 'margin: 4px 4px 4px 0px');
        Ø2.setAttribute('data-cissue', task.Counter);
        Ø2.setAttribute('data-cproject', task.C_Project);
        Ø2.setAttribute('data-detailsid', detailsId);
        Ø2.addEventListener('click', ZHB.Tasks.Status.editIssue1, false);
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('a');
        Ø3.innerText = 'Resume';
        Ø3.setAttribute('style', 'margin: 4px 4px 4px 0px');
        Ø3.setAttribute('data-nextstate', resumeStatus);
        Ø3.setAttribute('data-cproject', task.C_Project);
        Ø3.setAttribute('data-counter', task.Counter);
        Ø3.addEventListener('click', ZHB.Tasks.Status.getOtherForm1, false);
        Ø3.setAttribute('class', 'button');
        Ø1.appendChild(Ø3);
        var Ø4 = WAVE.ce('a');
        Ø4.innerText = 'Cancel';
        Ø4.setAttribute('style', 'margin: 4px 4px 4px 0px');
        Ø4.setAttribute('data-nextstate', 'X');
        Ø4.setAttribute('data-cproject', task.C_Project);
        Ø4.setAttribute('data-counter', task.Counter);
        Ø4.addEventListener('click', ZHB.Tasks.Status.getOtherForm1, false);
        Ø4.setAttribute('class', 'button');
        Ø1.appendChild(Ø4);
      }
      var Ø5 = WAVE.ce('a');
      Ø5.innerText = 'Report';
      Ø5.setAttribute('data-cproject', task.C_Project);
      Ø5.setAttribute('data-cissue', task.Counter);
      Ø5.setAttribute('data-report', 'statusreport');
      Ø5.addEventListener('click', ZHB.Tasks.Report.openReport, false);
      Ø5.setAttribute('class', 'button');
      Ø5.setAttribute('style', 'margin: 4px 4px 4px 0px');
      Ø1.appendChild(Ø5);
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
      return Ø1;
    } else {
      var Ør = arguments[0];
      if (WAVE.isString(Ør))
        Ør = WAVE.id(Ør);
      var Ø1 = WAVE.ce('div');
      if(ZHB.Tasks.isPM) {
        var Ø2 = WAVE.ce('a');
        Ø2.innerText = 'Edit Issue';
        Ø2.setAttribute('class', 'button');
        Ø2.setAttribute('style', 'margin: 4px 4px 4px 0px');
        Ø2.setAttribute('data-cissue', task.Counter);
        Ø2.setAttribute('data-cproject', task.C_Project);
        Ø2.setAttribute('data-detailsid', detailsId);
        Ø2.addEventListener('click', ZHB.Tasks.Status.editIssue1, false);
        Ø1.appendChild(Ø2);
         for(var s=0, sl=task.NextState.length; s < sl; s++) {
          var Ø3 = WAVE.ce('a');
          Ø3.innerText = ZHB.Tasks.Status.getStatusButtonName(task.NextState[s]);
          Ø3.setAttribute('style', 'margin: 4px 4px 4px 0px');
          Ø3.setAttribute('data-nextstate', task.NextState[s]);
          Ø3.setAttribute('data-cproject', task.C_Project);
          Ø3.setAttribute('data-counter', task.Counter);
          Ø3.addEventListener('click', ZHB.Tasks.Status.changeStatusDialog1, false);
          Ø3.setAttribute('class', 'button');
          Ø1.appendChild(Ø3);
        }
      }
      var Ø4 = WAVE.ce('a');
      Ø4.innerText = 'Report';
      Ø4.setAttribute('data-cproject', task.C_Project);
      Ø4.setAttribute('data-cissue', task.Counter);
      Ø4.setAttribute('data-report', 'statusreport');
      Ø4.addEventListener('click', ZHB.Tasks.Report.openReport, false);
      Ø4.setAttribute('class', 'button');
      Ø4.setAttribute('style', 'margin: 4px 4px 4px 0px');
      Ø1.appendChild(Ø4);
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
      return Ø1;
    }
  };

  published.createStatusHeader = function (root) {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    if(1==1) {
      var Ø1 = WAVE.ce('div');
      Ø1.innerText = 'Status Date';
      Ø1.setAttribute('class', 'rst-cell rst-details-head');
      Ø1.setAttribute('style', 'width: 9%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
      var Ø2 = WAVE.ce('div');
      Ø2.innerText = 'Operator';
      Ø2.setAttribute('class', 'rst-cell rst-details-head');
      Ø2.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
      var Ø3 = WAVE.ce('div');
      Ø3.innerText = '%';
      Ø3.setAttribute('class', 'rst-cell rst-details-head');
      Ø3.setAttribute('style', 'width: 3%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
      var Ø4 = WAVE.ce('div');
      Ø4.innerText = 'Status';
      Ø4.setAttribute('class', 'rst-cell rst-details-head');
      Ø4.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
      var Ø5 = WAVE.ce('div');
      Ø5.innerText = 'Start';
      Ø5.setAttribute('class', 'rst-cell rst-details-head');
      Ø5.setAttribute('style', 'width: 6%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
      var Ø6 = WAVE.ce('div');
      Ø6.innerText = 'Plan\x2FDue';
      Ø6.setAttribute('class', 'rst-cell rst-details-head');
      Ø6.setAttribute('style', 'width: 6%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
      var Ø7 = WAVE.ce('div');
      Ø7.innerText = 'Complete';
      Ø7.setAttribute('class', 'rst-cell rst-details-head');
      Ø7.setAttribute('style', 'width: 6%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
      var Ø8 = WAVE.ce('div');
      Ø8.innerText = 'Assigned';
      Ø8.setAttribute('class', 'rst-cell rst-details-head');
      Ø8.setAttribute('style', 'width: 20%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
      var Ø9 = WAVE.ce('div');
      Ø9.innerText = 'Description';
      Ø9.setAttribute('class', 'rst-cell rst-details-head');
      Ø9.setAttribute('style', 'width: 30%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø9);
    }
    return Ø9;
  };


  published.createStatusGridRow = function (root, details) {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    if(1==1) {
      var Ø1 = WAVE.ce('div');
      Ø1.innerText = WAVE.dateTimeToString(details.Status_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE_TIME);
      Ø1.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø1.setAttribute('style', 'width: 9%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
      var Ø2 = WAVE.ce('div');
      Ø2.innerText = details.Operator;
      Ø2.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø2.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
      var Ø3 = WAVE.ce('div');
      Ø3.innerText = details.Completeness +'%';
      Ø3.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø3.setAttribute('style', 'width: 3%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
      var Ø4 = WAVE.ce('div');
      Ø4.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø4.setAttribute('style', 'width: 10%');
    var Ø5 = WAVE.ce('div');
    Ø5.setAttribute('align', 'center');
    var Ø6 = WAVE.ce('div');
    Ø6.innerText = details.Priority;
    Ø6.setAttribute('class', 'tag {0} inline-block'.args(ZHB.Tasks.Render.getPriorityStyle(details.Priority)));
    Ø5.appendChild(Ø6);
    var Ø7 = WAVE.ce('div');
    Ø7.innerText = details.Status;
    Ø7.setAttribute('class', 'tag {0} inline-block'.args(ZHB.Tasks.Render.getStatusStyle(details.Status)));
    Ø5.appendChild(Ø7);
    var Ø8 = WAVE.ce('div');
    Ø8.innerText = details.Category_Name;
    Ø8.setAttribute('class', 'tag tag-category inline-block');
    Ø5.appendChild(Ø8);
    Ø4.appendChild(Ø5);
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
      var Ø9 = WAVE.ce('div');
      Ø9.innerText = WAVE.dateTimeToString(details.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
      Ø9.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø9.setAttribute('style', 'width: 6%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø9);
      var Ø10 = WAVE.ce('div');
      Ø10.innerText = WAVE.dateTimeToString(details.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
      Ø10.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø10.setAttribute('style', 'width: 6%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø10);
      var Ø11 = WAVE.ce('div');
      Ø11.innerText = WAVE.dateTimeToString(details.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
      Ø11.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø11.setAttribute('style', 'width: 6%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø11);
      var Ø12 = WAVE.ce('div');
      Ø12.innerText = details.Assignee;
      Ø12.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø12.setAttribute('style', 'width: 20%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø12);
      var Ø13 = WAVE.ce('div');
      Ø13.innerText = details.Description;
      Ø13.setAttribute('id', 'details-description'+details.Counter);
      Ø13.setAttribute('class', 'rst-cell rst-details-cell');
      Ø13.setAttribute('style', 'width: 30%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø13);
    }
    return Ø13;
  };

  published.buildStatusBody = function (status) {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    var Ø1 = WAVE.ce('div');
    Ø1.setAttribute('id', 'statusForm');
    Ø1.setAttribute('data-wv-rid', 'V2');
    var Ø2 = WAVE.ce('div');
    Ø2.setAttribute('data-wv-fname', 'Description');
    Ø2.setAttribute('class', 'fView');
    Ø1.appendChild(Ø2);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;
  };

  published.buildStatusFooter = function () {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    var Ø1 = WAVE.ce('div');
    Ø1.setAttribute('align', 'right');
    Ø1.setAttribute('style', 'margin: 5px');
    var Ø2 = WAVE.ce('a');
    Ø2.innerText = 'ok';
    Ø2.setAttribute('href', 'javascript:WAVE.GUI.currentDialog().ok()');
    Ø2.setAttribute('style', 'margin: 5px; padding: 2px; width: 75px; height: 23px');
    Ø2.setAttribute('class', 'button');
    Ø1.appendChild(Ø2);
    var Ø3 = WAVE.ce('a');
    Ø3.innerText = 'cancel';
    Ø3.setAttribute('href', 'javascript:WAVE.GUI.currentDialog().cancel()');
    Ø3.setAttribute('style', 'margin: 5px; padding: 2px; width: 75px; height: 23px');
    Ø3.setAttribute('class', 'button');
    Ø1.appendChild(Ø3);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;
  };

  published.buildIssueAssigneeStatusBody = function () {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    var Ø1 = WAVE.ce('div');
    Ø1.setAttribute('id', 'statusForm');
    Ø1.setAttribute('data-wv-rid', 'V2');
    var Ø2 = WAVE.ce('div');
    Ø2.setAttribute('data-wv-fname', 'C_User');
    Ø2.setAttribute('class', 'fView');
    Ø2.setAttribute('data-wv-ctl', 'combo');
    Ø1.appendChild(Ø2);
    var Ø3 = WAVE.ce('div');
    Ø3.setAttribute('data-wv-fname', 'Open_TS');
    Ø3.setAttribute('class', 'fView');
    Ø1.appendChild(Ø3);
    var Ø4 = WAVE.ce('div');
    Ø4.setAttribute('data-wv-fname', 'Close_TS');
    Ø4.setAttribute('class', 'fView');
    Ø1.appendChild(Ø4);
    var Ø5 = WAVE.ce('div');
    Ø5.setAttribute('data-wv-fname', 'Description');
    Ø5.setAttribute('class', 'fView');
    Ø5.setAttribute('type', 'text');
    Ø1.appendChild(Ø5);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;

  };

  published.init = function (init) {

  };

  return published;
})();



/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Assignment = (function () {
    var published = {}
    ;
    
    published.buildAssignmentTab = function(root, task) {
        var gridID = "assignment-grid-" + task.Counter;
        ZHB.Tasks.Assignment.Render.buildAssignmentButtons(root, task);
        ZHB.Tasks.Render.createGrid(root, gridID);
        ZHB.Tasks.Assignment.Render.createAssignmentHeader(gridID);
        for (var j = 0, l = task.Assignments.length; j < l; j++)
            ZHB.Tasks.Assignment.Render.createAssignmentGridRow(gridID, task.Assignments[j], task);
    };
    
    published.init = function (init) {
        
    };
    
    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Assignment.Render = (function () {
    var published = {}
    ;

    published.buildAssignmentButtons = function (root, task) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
         if(task.statusId !='X' && task.statusId !='C' && task.statusId !='D' && ZHB.Tasks.isPM) {
          var Ø2 = WAVE.ce('a');
          Ø2.innerText = 'Add user';
          Ø2.setAttribute('data-counter', task.Counter);
          Ø2.setAttribute('data-cproject', task.C_Project);
          Ø2.setAttribute('data-nextstate', 'A');
          Ø2.addEventListener('click', ZHB.Tasks.Status.changeStatusDialog1, false);
          Ø2.setAttribute('class', 'button');
          Ø2.setAttribute('style', 'margin:4px 4px 4px 0px');
          Ø1.appendChild(Ø2);
        }
        var Ø3 = WAVE.ce('a');
        Ø3.innerText = 'Report';
        Ø3.setAttribute('class', 'button');
        Ø3.setAttribute('style', 'margin:4px 4px 4px 0px');
        Ø3.setAttribute('data-cproject', task.C_Project);
        Ø3.setAttribute('data-cissue', task.Counter);
        Ø3.setAttribute('data-report', 'assignmentreport');
        Ø3.addEventListener('click', ZHB.Tasks.Report.openReport, false);
        Ø1.appendChild(Ø3);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    }

    published.createAssignmentHeader = function (root) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        if(1==1) {
          var Ø1 = WAVE.ce('div');
          Ø1.innerText = '*';
          Ø1.setAttribute('class', 'rst-cell rst-details-head');
          Ø1.setAttribute('style', 'width: 1%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
          var Ø2 = WAVE.ce('div');
          Ø2.innerText = 'ID';
          Ø2.setAttribute('class', 'rst-cell rst-details-head');
          Ø2.setAttribute('style', 'width: 5%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
          var Ø3 = WAVE.ce('div');
          Ø3.innerText = 'User';
          Ø3.setAttribute('class', 'rst-cell rst-details-head');
          Ø3.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
          var Ø4 = WAVE.ce('div');
          Ø4.innerText = 'Assigned';
          Ø4.setAttribute('class', 'rst-cell rst-details-head');
          Ø4.setAttribute('style', 'width: 15%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
          var Ø5 = WAVE.ce('div');
          Ø5.innerText = 'Operator';
          Ø5.setAttribute('class', 'rst-cell rst-details-head');
          Ø5.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
          var Ø6 = WAVE.ce('div');
          Ø6.innerText = 'Unassigned';
          Ø6.setAttribute('class', 'rst-cell rst-details-head');
          Ø6.setAttribute('style', 'width: 15%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
          var Ø7 = WAVE.ce('div');
          Ø7.innerText = 'Operator';
          Ø7.setAttribute('class', 'rst-cell rst-details-head');
          Ø7.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
          var Ø8 = WAVE.ce('div');
          Ø8.innerText = 'Note';
          Ø8.setAttribute('class', 'rst-cell rst-details-head');
          Ø8.setAttribute('style', 'width: 34%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
        }
        return Ø8;
    };

    published.createAssignmentGridRow = function (root, assignment, task) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        if(1==1) {
          var Ø1 = WAVE.ce('div');
          Ø1.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
          Ø1.setAttribute('align', 'right');
          Ø1.setAttribute('style', 'width: 1%');
        if(!assignment.Close_TS && ZHB.Tasks.isPM) {
          var Ø2 = WAVE.ce('a');
          Ø2.innerText = 'x';
          Ø2.setAttribute('class', 'button-delete');
          Ø2.setAttribute('href', '#');
          Ø2.setAttribute('data-cproject', task.C_Project);
          Ø2.setAttribute('data-cissue', task.Counter);
          Ø2.setAttribute('data-cassignee', assignment.Counter);
          Ø2.addEventListener('click', ZHB.Tasks.Status.editAssignee, false);
          Ø1.appendChild(Ø2);
        }
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
          var Ø3 = WAVE.ce('div');
          Ø3.innerText = assignment.Counter;
          Ø3.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
          Ø3.setAttribute('align', 'right');
          Ø3.setAttribute('style', 'width: 5%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
          var Ø4 = WAVE.ce('div');
          Ø4.innerText = assignment.UserFirstName + ' ' + assignment.UserLastName + '(' +assignment.UserLogin+')';
          Ø4.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
          Ø4.setAttribute('align', 'right');
          Ø4.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
          var Ø5 = WAVE.ce('div');
          Ø5.innerText = WAVE.dateTimeToString(assignment.Open_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
          Ø5.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
          Ø5.setAttribute('style', 'width: 15%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
          var Ø6 = WAVE.ce('div');
          Ø6.innerText = assignment.OperatorOpenLogin;
          Ø6.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
          Ø6.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
          var Ø7 = WAVE.ce('div');
          Ø7.innerText = WAVE.dateTimeToString(assignment.Close_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
          Ø7.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
          Ø7.setAttribute('style', 'width: 15%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
          var Ø8 = WAVE.ce('div');
          Ø8.innerText = assignment.OperatorCloseLogin;
          Ø8.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
          Ø8.setAttribute('align', 'center');
          Ø8.setAttribute('style', 'width: 10%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
          var Ø9 = WAVE.ce('div');
          Ø9.innerText = assignment.Note;
          Ø9.setAttribute('class', 'rst-cell rst-details-cell');
          Ø9.setAttribute('style', 'width: 34%');
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø9);
        }
        return Ø9;
    }

    published.init = function (init) {

    };

    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Chat = (function () {
  "use strict";
  var published = {},
    fScheduleTimer,
    fTasks,
    fChatRec = {},
    fChatFilterRec = {},
    fTick = 5000,
    fTickDefault = 5000,
    fTickDelta = 10000;

  function schedulerChat() {
    if (fScheduleTimer) clearTimeout(fScheduleTimer);
    WAVE.each(fTasks, function (task) {
      ZHB.Tasks.Chat.refreshChat(task);
    });
    fScheduleTimer = setTimeout(schedulerChat, fTick);
  }

  function createChatItems(task, rec) {
    var id = 'chatMessage-' + task.Counter;
    if ((document.getElementById(id) !== undefined) && (document.getElementById(id) !== null)) {
      document.getElementById(id).innerHTML = "";
      for (var i = 0, l = rec.Rows.length; i < l; i++) {
        var item = rec.Rows[i];
        ZHB.Tasks.Chat.Render.createChatItem(id, item);
        document.getElementById('chat-note' + item.Counter).innerHTML = WAVE.markup(item.Note);
        ZHB.Tasks.Chat.Render.createEditChatButton('chathedaeritem' + item.Counter, item, task);
      }
    }
  }

  function chatForm(task) {
    var link = ZHB.URIS.ForPROJECT_ISSUE_CHAT(task.C_Project, task.Counter, "");
    WAVE.ajaxCall(
      'GET',
      link,
      null,
      function (resp) {
        fChatRec[task.Counter] = new WAVE.RecordModel.Record(JSON.parse(resp));
        new WAVE.RecordModel.RecordView('chatForm' + task.Counter, fChatRec[task.Counter]);
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  function sendChatMessage(pid, iid, cid, _rec) {
    var task = {Counter: iid, C_Project: pid, createImageData: cid};
    var link = ZHB.URIS.ForPROJECT_ISSUE_CHAT(pid, iid, cid);
    WAVE.ajaxCall(
      'POST',
      link,
      _rec.data(),
      function (resp) {
        chatForm(task);
        ZHB.Tasks.Chat.refreshChat(task);
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  function chatFilterForm(task) {
    var link = ZHB.URIS.ForPROJECT_ISSUE_CHATLIST(task.C_Project, task.Counter);
    WAVE.ajaxCall(
      'GET',
      link,
      null,
      function (resp) {
        // debugger;
        fChatFilterRec[task.Counter] = new WAVE.RecordModel.Record(JSON.parse(resp));
        new WAVE.RecordModel.RecordView('ChatFilterForm' + task.Counter, fChatFilterRec[task.Counter]);

      },
      function (resp) {
        console.log("error");
      },
      function (resp) {
        console.log("fail");
      },
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  published.setChatFilter = function (e) {
    e.stopPropagation();
    var iid = e.target.dataset.cissue;
    var pid = e.target.dataset.cproject;
    var task = {Counter: iid, C_Project: pid};
    ZHB.Tasks.Chat.refreshChat(task);
  }

  published.editChatItem = function (e) {
    e.stopPropagation();
    var chatId = e.target.dataset.chatid;
    var note = e.target.dataset.node;
    var iid = e.target.dataset.cissue;
    var pid = e.target.dataset.cproject;

    var link = ZHB.URIS.ForPROJECT_ISSUE_CHAT(pid, iid, chatId);

    WAVE.ajaxCall(
      'GET',
      link,
      null,
      function (resp) {
        var rec = new WAVE.RecordModel.Record(JSON.parse(resp));
        var dlg = WAVE.GUI.Dialog({
          header: " Edit note",
          body: ZHB.Tasks.Chat.Render.buildEditChatDialog(null, chatId),
          footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
          onShow: function () {
            var rv = new WAVE.RecordModel.RecordView("V22", rec);
          },
          onClose: function (dlg, result) {
            if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
            rec.validate();
            if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
            sendChatMessage(pid, iid, chatId, rec);
            return WAVE.GUI.DLG_CANCEL;
          }
        });
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  };

  published.sendChatMessage1 = function (e) {
    var iid = e.target.dataset.cissue;
    var pid = e.target.dataset.cproject;
    sendChatMessage(pid, iid, "", fChatRec[iid])
  };

  published.refreshChat = function (task) {
    if (fChatFilterRec[task.Counter]) {
      var link = ZHB.URIS.ForPROJECT_ISSUE_CHATLIST(task.C_Project, task.Counter);
      var data = fChatFilterRec[task.Counter].data();
      WAVE.ajaxCall(
        'POST',
        link,
        data,
        function (resp) {
          var rec = JSON.parse(resp);
          fTick = fTickDefault;
          createChatItems(task, rec);
        },
        function (resp) {
          ZHB.errorLog(resp);
          fTick += fTickDelta
        },
        function (resp) {
          ZHB.errorLog(resp);
          fTick += fTickDelta
        },
        WAVE.CONTENT_TYPE_JSON_UTF8,
        WAVE.CONTENT_TYPE_JSON_UTF8
      );
    }
  };

  published.buildChatTab = function (root, task) {
    ZHB.Tasks.Chat.Render.buildChatFilterForm(root, task);
    ZHB.Tasks.Chat.Render.buildChatForm(root, task);
    ZHB.Tasks.Chat.Render.buildChatMessage(root, task);
    chatForm(task);
    chatFilterForm(task);
  };

  published.init = function (init) {
    ZHB.Tasks.Chat.Render.init({});
    fTasks = init.tasks;
    schedulerChat();
  };

  return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Chat.Render = (function () {
    "use strict";
    var published = {}
    ;

    published.buildChatFilterForm = function (root, task) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('id', 'ChatFilterForm'+task.Counter);
        Ø1.setAttribute('data-wv-rid', 'chatFilterForm'+task.Counter);
        var Ø2 = WAVE.ce('div');
        Ø2.setAttribute('data-wv-fname', 'C_User');
        Ø2.setAttribute('class', 'fView');
        Ø2.setAttribute('data-wv-ctl', 'combo');
        Ø2.setAttribute('style', 'display: inline-block; padding: 8px;');
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('div');
        Ø3.setAttribute('data-wv-fname', 'Limit');
        Ø3.setAttribute('class', 'fView');
        Ø3.setAttribute('style', 'display: inline-block; padding: 8px;');
        Ø1.appendChild(Ø3);
        var Ø4 = WAVE.ce('div');
        Ø4.setAttribute('style', 'display: inline-block; padding: 8px;');
        var Ø5 = WAVE.ce('a');
        Ø5.innerText = 'filter';
        Ø5.setAttribute('data-cissue', task.Counter);
        Ø5.setAttribute('data-cproject', task.C_Project);
        Ø5.addEventListener('click', ZHB.Tasks.Chat.setChatFilter, false);
        Ø5.setAttribute('class', 'button');
        Ø4.appendChild(Ø5);
        Ø1.appendChild(Ø4);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.buildChatForm = function (root, task) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('id', 'chatForm'+task.Counter);
        Ø1.setAttribute('class', 'fwDialogBody');
        Ø1.setAttribute('data-wv-rid', 'chatForm'+task.Counter);
        var Ø2 = WAVE.ce('div');
        Ø2.setAttribute('data-wv-fname', 'Note');
        Ø2.setAttribute('class', 'fView');
        Ø2.setAttribute('data-wv-ctl', 'textarea');
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('div');
        var Ø4 = WAVE.ce('a');
        Ø4.innerText = 'send';
        Ø4.setAttribute('class', 'button');
        Ø4.setAttribute('style', 'margin:4px 4px 4px 0px');
        Ø4.setAttribute('data-cissue', task.Counter);
        Ø4.setAttribute('data-cproject', task.C_Project);
        Ø4.addEventListener('click', ZHB.Tasks.Chat.sendChatMessage1, false);
        Ø3.appendChild(Ø4);
        var Ø5 = WAVE.ce('a');
        Ø5.innerText = 'Report';
        Ø5.setAttribute('class', 'button');
        Ø5.setAttribute('style', 'margin:4px 4px 4px 0px');
        Ø5.setAttribute('data-cproject', task.C_Project);
        Ø5.setAttribute('data-cissue', task.Counter);
        Ø5.setAttribute('data-report', 'chatreport');
        Ø5.addEventListener('click', ZHB.Tasks.Report.openReport, false);
        Ø3.appendChild(Ø5);
        Ø1.appendChild(Ø3);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.buildChatMessage = function (root, task) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('class', 'ChatDiv');
        Ø1.setAttribute('id', 'chatMessage-'+task.Counter);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.createChatItem = function (root, item) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('class', 'ChatItem');
        var Ø2 = WAVE.ce('div');
        Ø2.innerText = item.Name +'('+item.Login+') :' + WAVE.dateTimeToString(item.Note_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE_TIME);
        Ø2.setAttribute('id', 'chathedaeritem'+item.Counter);
        Ø2.setAttribute('class', 'fView ChatItemUser');
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('div');
        Ø3.innerText = item.Note;
        Ø3.setAttribute('id', 'chat-note'+item.Counter);
        Ø3.setAttribute('class', 'fView ChatItemNote');
        Ø1.appendChild(Ø3);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    published.createEditChatButton = function (root, item, task) {
        if (item.HasEdit) {
            var Ør = arguments[0];
            if (WAVE.isString(Ør))
              Ør = WAVE.id(Ør);
            var Ø1 = WAVE.ce('a');
            Ø1.innerText = 'edit';
            Ø1.setAttribute('class', 'button');
            Ø1.setAttribute('data-chatid', item.Counter);
            Ø1.setAttribute('data-note', item.Note);
            Ø1.setAttribute('data-cproject', task.C_Project);
            Ø1.setAttribute('data-cissue', task.Counter);
            Ø1.addEventListener('click', ZHB.Tasks.Chat.editChatItem, false);
            if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
            return Ø1;
        }
    };

    published.buildEditChatDialog = function (root, item) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('data-wv-rid', 'V22');
        var Ø2 = WAVE.ce('div');
        Ø2.setAttribute('data-wv-fname', 'Note');
        Ø2.setAttribute('class', 'fView');
        Ø2.setAttribute('data-wv-ctl', 'textarea');
        Ø1.appendChild(Ø2);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };


    published.init = function (init) {

    };

    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Areas = (function () {
  "use strict";
  var published = {};

  published.buildAreasTab = function (root, task) {

    if (!ZHB.Tasks.isPM) return published;

    var link = ZHB.URIS.ForPROJECT_ISSUE_AREA(task.C_Project, task.Counter);
        
    WAVE.ajaxCall(
      'POST',
      link,
      null,
      function (resp) {
        var _rec = JSON.parse(resp);
        ZHB.Tasks.Areas.Render.buildGrid(root, _rec.Rows);
      },
      function (resp) {
        console.log("error");
      },
      function (resp) {
        console.log("fail");
      },
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );

  };

  return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Areas.Render = (function () {
    "use strict";
    var published = {};

    published.buildGrid = function (root, areas) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('class', 'rst-table');
        var Ø2 = WAVE.ce('div');
        Ø2.innerText = 'Counter';
        Ø2.setAttribute('class', 'rst-cell rst-details-head');
        Ø2.setAttribute('style', 'width:10%');
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('div');
        Ø3.innerText = 'Name';
        Ø3.setAttribute('class', 'rst-cell rst-details-head');
        Ø3.setAttribute('style', 'width:80%');
        Ø1.appendChild(Ø3);
        var Ø4 = WAVE.ce('div');
        Ø4.innerText = 'Linked';
        Ø4.setAttribute('class', 'rst-cell rst-details-head');
        Ø4.setAttribute('style', 'width:10%');
        Ø1.appendChild(Ø4);
        for(var i=0, l=areas.length; i<l; i++) {
          var Ø5 = WAVE.ce('div');
          Ø5.innerText = areas[i][2];
          Ø5.setAttribute('class', 'rst-cell rst-details-cell');
          Ø5.setAttribute('style', 'width:10%');
          Ø1.appendChild(Ø5);
          var Ø6 = WAVE.ce('div');
          Ø6.innerText = areas[i][3];
          Ø6.setAttribute('class', 'rst-cell rst-details-cell');
          Ø6.setAttribute('style', 'width:80%');
          Ø1.appendChild(Ø6);
          var Ø7 = WAVE.ce('div');
          Ø7.setAttribute('class', 'rst-cell rst-details-cell rst-text-align-center');
          Ø7.setAttribute('style', 'width:10%');
        var Ø8 = WAVE.ce('input');
        Ø8.innerText = areas[i][4];
        Ø8.setAttribute('style', 'display:inline-block');
        Ø8.setAttribute('type', 'checkbox');
        Ø8.checked = areas[i][4];
        Ø8.setAttribute('onchange', 'linkIssueArea(event,"{0}", "{1}", "{2}")'.args(areas[i][0],areas[i][1],areas[i][2]));
        Ø7.appendChild(Ø8);
          Ø1.appendChild(Ø7);
        }
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Components = (function () {
  "use strict";
  var published = {};

  published.buildComponentsTab = function (root, task) {
    if (!ZHB.Tasks.isPM) return published;

    var link = ZHB.URIS.ForPROJECT_ISSUE_COMPONENT(task.C_Project, task.Counter);

    WAVE.ajaxCall(
      'POST',
      link,
      null,
      function (resp) {
        var _rec = JSON.parse(resp);
        ZHB.Tasks.Components.Render.buildGrid(root, _rec.Rows);
      },
      function (resp) {
        console.log("error");
      },
      function (resp) {
        console.log("fail");
      },
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  };

  return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Components.Render = (function () {
    "use strict";
    var published = {};

    published.buildGrid = function (root, components) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.setAttribute('class', 'rst-table');
        var Ø2 = WAVE.ce('div');
        Ø2.innerText = 'Counter';
        Ø2.setAttribute('class', 'rst-cell rst-details-head');
        Ø2.setAttribute('style', 'width:10%');
        Ø1.appendChild(Ø2);
        var Ø3 = WAVE.ce('div');
        Ø3.innerText = 'Name';
        Ø3.setAttribute('class', 'rst-cell rst-details-head');
        Ø3.setAttribute('style', 'width:80%');
        Ø1.appendChild(Ø3);
        var Ø4 = WAVE.ce('div');
        Ø4.innerText = 'Linked';
        Ø4.setAttribute('class', 'rst-cell rst-details-head');
        Ø4.setAttribute('style', 'width:10%');
        Ø1.appendChild(Ø4);
        for(var i=0, l=components.length; i<l; i++) {
          var Ø5 = WAVE.ce('div');
          Ø5.innerText = components[i][2];
          Ø5.setAttribute('class', 'rst-cell rst-details-cell');
          Ø5.setAttribute('style', 'width:10%');
          Ø1.appendChild(Ø5);
          var Ø6 = WAVE.ce('div');
          Ø6.innerText = components[i][3];
          Ø6.setAttribute('class', 'rst-cell rst-details-cell');
          Ø6.setAttribute('style', 'width:80%');
          Ø1.appendChild(Ø6);
          var Ø7 = WAVE.ce('div');
          Ø7.setAttribute('class', 'rst-cell rst-details-cell rst-text-align-center');
          Ø7.setAttribute('style', 'width:10%');
        var Ø8 = WAVE.ce('input');
        Ø8.innerText = components[i][4];
        Ø8.setAttribute('style', 'display:inline-block');
        Ø8.setAttribute('type', 'checkbox');
        Ø8.checked = components[i][4];
        Ø8.setAttribute('onchange', 'linkIssueComponent(event,"{0}", "{1}", "{2}")'.args(components[i][0],components[i][1],components[i][2]));
        Ø7.appendChild(Ø8);
          Ø1.appendChild(Ø7);
        }
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };

    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Report = (function () {
    var published = {}
    ;
    
    published.openReport = function(e) {
        var pid = e.target.dataset.cproject;
        var iid = e.target.dataset.cissue;
        var report = e.target.dataset.report;
        var link = ZHB.URIS.ForPROJECT_ISSUE_REPORT(pid, iid, report);
        window.open(link);
    };
    
    return published;
})();