"use strict";

var ZHB = (function () {

  var published = {};

  published.ControlScripts = {};//ns for dynamic control scripts

  function project_setup(pid) { return "/project/{0}".args(pid); }

  function issue_setup(pid, iid) { return project_setup(pid) + "/issue/{0}".args(iid); }

  function common_setup() { return "/common"; }

  published.URIS = {
    ForCOMPONENT: function (id) {
      return common_setup() + "/component?id={0}".args(id);
    },

    ForAREA: function (id) {
      return common_setup() + "/area?id={0}".args(id);
    },

    ForPROJECT_SELECT: function (pid) {
      return project_setup(pid) + "/select";
    },

    ForPROJECT: function (id) {
      return common_setup() + "/project?id={0}".args(id);
    },

    ForCATEGORY: function (id) {
      return common_setup() + "/category?id={0}".args(id);
    },
    
    ForPROJECT_MILESTONE: function (pid, id) {
      return project_setup(pid) + "/milestone?id={0}".args(id);
    },

    ForPROJECT_COMPONENT: function (pid, id) {
      return project_setup(pid) + "/component?counter={0}".args(id);
    },

    ForPROJECT_AREA: function (pid, id) {
      return project_setup(pid) + "/area?counter={0}".args(id);
    },

    ForPROJECT_ISSUE: function (pid, id) {
      return project_setup(pid) + "/issue?id={0}".args(id);
    },

    ForPROJECT_DELETE_COMPONENT: function (project, counter) {
      return project_setup(project) + "/component?counter={0}".args(counter);
    },

    ForPROJECT_DELETE_AREA: function (project, counter) {
      return project_setup(project) + "/area?counter={0}".args(counter);
    },

    ForPROJECT_ISSUE_AREA: function (project, issue) {
      return project_setup(project) + "/issuearea?issue={0}".args(issue);
    },

    ForPROJECT_ISSUE_COMPONENT: function (project, issue) {
      return project_setup(project) + "/issuecomponent?issue={0}".args(issue);
    },

    ForPROJECT_LINK_ISSUE_AREA: function (project, issue, area) {
      return project_setup(project) + "/linkissuearea?issue={0}&area={1}".args(issue, area);
    },

    ForPROJECT_LINK_ISSUE_COMPONENT: function (project, issue, component) {
      return project_setup(project) + "/linkissuecomponent?issue={0}&component={1}".args(issue, component);
    },

    ForUSER: function (id) {
      return common_setup() + "/user?id={0}".args(id);
    },

    ForDELETE_CATEGORY: function (id) {
      return common_setup() + "/category?id={0}".args(id);
    },

    ForISSUE_ISSUEASSIGN: function (pid, iid, id) {
      return issue_setup(pid, iid) + "/issueassign?id={0}".args(id);
    },

    ForISSUE_STATUSNOTE: function (pid, iid, status) {
      return issue_setup(pid, iid) + "/statusnote?status={0}".args(status);
    },

    ForDELETE_ISSUE: function (pid, iid) {
      return issue_setup(pid, iid) + "/close";
    },

    ForREOPEN_ISSUE: function (pid, iid) {
      return issue_setup(pid, iid) + "/reopen";
    },

    ForDEFER_ISSUE: function (pid, iid) {
      return issue_setup(pid, iid) + "/defer";
    }

  };

  if (!String.prototype.args) {
    String.prototype.args = function () {
      var argts = arguments;
      return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof argts[number] != 'undefined'
          ? argts[number]
          : match
        ;
      });
    };
  }

  return published;
}());

﻿var chatRec = {};
var chatFilterRec = {};

function computePriorityStyle(priority) {
  var color = "black";
  if (priority === 0) {
    color = "red";
  } else if (priority > 0 && priority <= 3) {
    color = "orange";
  } else if (priority > 3 && priority <= 5) {
    color = "#9c6f10";
  } else {
    color = "green";
  }
  return "background-color: {0}; width: 20px".args(color);
}

function buildDate(task) {
  var startDate = WAVE.dateTimeToString(task.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  var completeDate = "OPEN";
  if (task.Complete_Date) {
    completeDate = WAVE.dateTimeToString(task.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  }
  return "{0} - {1}".args(startDate, completeDate);
}

function buildDueDate(task) {
  var dueDate = WAVE.dateTimeToString(task.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  return "{0} in {1}d".args(dueDate, task.Remaining);
}

function createHeaders(root) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  if(1 == 1) {
    var Ø1 = WAVE.ce('div');
    Ø1.innerText = 'ID';
    Ø1.setAttribute('class', 'cell head');
    Ø1.setAttribute('style', 'width: 5%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    var Ø2 = WAVE.ce('div');
    Ø2.innerText = 'Status';
    Ø2.setAttribute('class', 'cell head');
    Ø2.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
    var Ø3 = WAVE.ce('div');
    Ø3.innerText = 'Date';
    Ø3.setAttribute('class', 'cell head');
    Ø3.setAttribute('style', 'width: 15%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
    var Ø4 = WAVE.ce('div');
    Ø4.innerText = 'Assigned';
    Ø4.setAttribute('class', 'cell head');
    Ø4.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
    var Ø5 = WAVE.ce('div');
    Ø5.innerText = 'Areas\x2FComponents';
    Ø5.setAttribute('class', 'cell head');
    Ø5.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
    var Ø6 = WAVE.ce('div');
    Ø6.innerText = 'Project';
    Ø6.setAttribute('class', 'cell head');
    Ø6.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
    var Ø7 = WAVE.ce('div');
    Ø7.innerText = 'Issue';
    Ø7.setAttribute('class', 'cell head');
    Ø7.setAttribute('style', 'width: 20%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
    var Ø8 = WAVE.ce('div');
    Ø8.innerText = 'Description';
    Ø8.setAttribute('class', 'cell head');
    Ø8.setAttribute('style', 'width: 20%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
  }
  return Ø8;
}

function createRow(root, task) {
  var detailsId = "details-" + task.Counter;
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  if(1 == 1) {
    var Ø1 = WAVE.ce('div');
    Ø1.innerText = task.Counter;
    Ø1.setAttribute('class', 'cell issue_id expander');
    Ø1.setAttribute('style', 'width: 5%');
    Ø1.setAttribute('align', 'right');
    Ø1.setAttribute('data-cissue', task.Counter);
    Ø1.setAttribute('data-cproject', task.C_Project);
    Ø1.setAttribute('data-detailsid', detailsId);
    Ø1.addEventListener('click', editIssue1, false);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    var Ø2 = WAVE.ce('div');
    Ø2.setAttribute('class', 'cell completeness expander');
    Ø2.setAttribute('style', 'width: 10%');
    Ø2.setAttribute('data-detailsid', detailsId);
  var Ø3 = WAVE.ce('div');
  var Ø4 = WAVE.ce('div');
  Ø4.setAttribute('align', 'center');
  var Ø5 = WAVE.ce('div');
  Ø5.innerText = task.Status;
  Ø5.setAttribute('class', 'tag inline');
  Ø5.setAttribute('style', getStatusStyle(task.Status));
  Ø4.appendChild(Ø5);
  var Ø6 = WAVE.ce('div');
  Ø6.innerText = task.Category_Name;
  Ø6.setAttribute('class', 'tag inline');
  Ø6.setAttribute('style', 'background-color: gray;');
  Ø4.appendChild(Ø6);
  Ø3.appendChild(Ø4);
  var Ø7 = WAVE.ce('div');
  Ø7.innerText = task.Completeness +'%';
  Ø7.setAttribute('data-cproject', task.C_Project);
  Ø7.setAttribute('data-cissue', task.Counter);
  Ø7.setAttribute('data-progress', task.Completeness);
  Ø7.setAttribute('data-description', task.Description);
  Ø7.setAttribute('data-status', task.statusId);
  Ø7.addEventListener('click', changeProgress1, false);
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
    Ø9.setAttribute('class', 'cell expander');
    Ø9.setAttribute('data-detailsid', detailsId);
    Ø9.setAttribute('style', 'width: 15%');
  var Ø10 = WAVE.ce('div');
  Ø10.innerText = buildDate(task);
  Ø9.appendChild(Ø10);
  var Ø11 = WAVE.ce('div');
  var Ø12 = WAVE.ce('div');
  Ø12.innerText = buildDueDate(task);
  Ø12.setAttribute('class', 'inline');
  Ø11.appendChild(Ø12);
  var Ø13 = WAVE.ce('div');
  Ø13.innerText = task.Priority;
  Ø13.setAttribute('class', 'tag inline-block');
  Ø13.setAttribute('style', computePriorityStyle(task.Priority));
  Ø11.appendChild(Ø13);
  Ø9.appendChild(Ø11);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø9);
    var Ø14 = WAVE.ce('div');
    Ø14.setAttribute('id', 'assignee'+task.Counter);
    Ø14.setAttribute('class', 'cell expander');
    Ø14.setAttribute('style', 'width: 10%');
    Ø14.setAttribute('data-detailsid', detailsId);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø14);
    var Ø15 = WAVE.ce('div');
    Ø15.setAttribute('id', 'ac'+task.Counter);
    Ø15.setAttribute('class', 'cell expander');
    Ø15.setAttribute('style', 'width: 10%');
    Ø15.setAttribute('data-detailsid', detailsId);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø15);
    var Ø16 = WAVE.ce('div');
    Ø16.innerText = task.ProjectName;
    Ø16.setAttribute('class', 'cell expander');
    Ø16.setAttribute('style', 'width: 10%');
    Ø16.setAttribute('data-detailsid', detailsId);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø16);
    var Ø17 = WAVE.ce('div');
    Ø17.innerText = task.Name;
    Ø17.setAttribute('class', 'cell expander');
    Ø17.setAttribute('style', 'width: 20%');
    Ø17.setAttribute('data-detailsid', detailsId);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø17);
    var Ø18 = WAVE.ce('div');
    Ø18.innerText = task.Description;
    Ø18.setAttribute('class', 'cell expander');
    Ø18.setAttribute('style', 'width: 20%');
    Ø18.setAttribute('data-detailsid', detailsId);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø18);
  }
  return Ø18;
}

function createRowDetails(root, id) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('id', 'details-'+id);
  Ø1.setAttribute('class', 'cell full');
  var Ø2 = WAVE.ce('div');
  Ø2.setAttribute('id', 'tabs-'+id);
  Ø2.setAttribute('class', 'tab-control');
  Ø1.appendChild(Ø2);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function buildStatusButtons(root, task) {
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
    if(pmperm) {
      var Ø2 = WAVE.ce('a');
      Ø2.innerText = 'Resume';
      Ø2.setAttribute('style', 'margin: 4px 4px 4px 0px');
      Ø2.setAttribute('data-nextstate', resumeStatus);
      Ø2.setAttribute('data-cproject', task.C_Project);
      Ø2.setAttribute('data-counter', task.Counter);
      Ø2.addEventListener('click', getOtherForm1, false);
      Ø2.setAttribute('class', 'button');
      Ø1.appendChild(Ø2);
      var Ø3 = WAVE.ce('a');
      Ø3.innerText = 'Cancel';
      Ø3.setAttribute('style', 'margin: 4px 4px 4px 0px');
      Ø3.setAttribute('data-nextstate', 'X');
      Ø3.setAttribute('data-cproject', task.C_Project);
      Ø3.setAttribute('data-counter', task.Counter);
      Ø3.addEventListener('click', getOtherForm1, false);
      Ø3.setAttribute('class', 'button');
      Ø1.appendChild(Ø3);
    }
    var Ø4 = WAVE.ce('a');
    Ø4.innerText = 'report';
    Ø4.setAttribute('data-cproject', task.C_Project);
    Ø4.setAttribute('data-cissue', task.Counter);
    Ø4.setAttribute('data-report', 'statusreport');
    Ø4.addEventListener('click', openReport, false);
    Ø4.setAttribute('class', 'button');
    Ø4.setAttribute('style', 'margin: 4px 4px 4px 0px');
    Ø1.appendChild(Ø4);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;
  } else {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    var Ø1 = WAVE.ce('div');
    if(pmperm) {
       for(var s=0, sl=task.NextState.length; s < sl; s++) {
        var Ø2 = WAVE.ce('a');
        Ø2.innerText = statuses[task.NextState[s]];
        Ø2.setAttribute('style', 'margin: 4px 4px 4px 0px');
        Ø2.setAttribute('data-nextstate', task.NextState[s]);
        Ø2.setAttribute('data-cproject', task.C_Project);
        Ø2.setAttribute('data-counter', task.Counter);
        Ø2.addEventListener('click', changeStatusDialog1, false);
        Ø2.setAttribute('class', 'button');
        Ø1.appendChild(Ø2);
      }
    }
    var Ø3 = WAVE.ce('a');
    Ø3.innerText = 'report';
    Ø3.setAttribute('data-cproject', task.C_Project);
    Ø3.setAttribute('data-cissue', task.Counter);
    Ø3.setAttribute('data-report', 'statusreport');
    Ø3.addEventListener('click', openReport, false);
    Ø3.setAttribute('class', 'button');
    Ø3.setAttribute('style', 'margin: 4px 4px 4px 0px');
    Ø1.appendChild(Ø3);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;
  }
}

function buildAssignmentButtons(root, task) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
   if(task.statusId !='X' && task.statusId !='C' && task.statusId !='D' && pmperm) {
    var Ø2 = WAVE.ce('a');
    Ø2.innerText = 'Add user';
    Ø2.setAttribute('data-counter', task.Counter);
    Ø2.setAttribute('data-cproject', task.C_Project);
    Ø2.setAttribute('data-nextstate', 'A');
    Ø2.addEventListener('click', changeStatusDialog1, false);
    Ø2.setAttribute('class', 'button');
    Ø2.setAttribute('style', 'margin:4px 4px 4px 0px');
    Ø1.appendChild(Ø2);
  }
  var Ø3 = WAVE.ce('a');
  Ø3.innerText = 'report';
  Ø3.setAttribute('class', 'button');
  Ø3.setAttribute('style', 'margin:4px 4px 4px 0px');
  Ø3.setAttribute('data-cproject', task.C_Project);
  Ø3.setAttribute('data-cissue', task.Counter);
  Ø3.setAttribute('data-report', 'assignmentreport');
  Ø3.addEventListener('click', openReport, false);
  Ø1.appendChild(Ø3);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createStatusHeader(root) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  if(1==1) {
    var Ø1 = WAVE.ce('div');
    Ø1.innerText = 'ID';
    Ø1.setAttribute('class', 'cell head rDetailsTableHead');
    Ø1.setAttribute('style', 'width: 5%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    var Ø2 = WAVE.ce('div');
    Ø2.innerText = 'Progress';
    Ø2.setAttribute('class', 'cell head rDetailsTableHead');
    Ø2.setAttribute('style', 'width: 5%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
    var Ø3 = WAVE.ce('div');
    Ø3.innerText = 'Status';
    Ø3.setAttribute('class', 'cell head rDetailsTableHead');
    Ø3.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
    var Ø4 = WAVE.ce('div');
    Ø4.innerText = 'Start';
    Ø4.setAttribute('class', 'cell head rDetailsTableHead');
    Ø4.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
    var Ø5 = WAVE.ce('div');
    Ø5.innerText = 'Plan\x2FDue';
    Ø5.setAttribute('class', 'cell head rDetailsTableHead');
    Ø5.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
    var Ø6 = WAVE.ce('div');
    Ø6.innerText = 'Complete';
    Ø6.setAttribute('class', 'cell head rDetailsTableHead');
    Ø6.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
    var Ø7 = WAVE.ce('div');
    Ø7.innerText = 'Assigned';
    Ø7.setAttribute('class', 'cell head rDetailsTableHead');
    Ø7.setAttribute('style', 'width: 30%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
    var Ø8 = WAVE.ce('div');
    Ø8.innerText = 'Description';
    Ø8.setAttribute('class', 'cell head rDetailsTableHead');
    Ø8.setAttribute('style', 'width: 20%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
  }
  return Ø8;
}

﻿function createAssignmentHeader(root) {
﻿  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  if(1==1) {
    var Ø1 = WAVE.ce('div');
    Ø1.innerText = 'ID';
    Ø1.setAttribute('class', 'cell head rDetailsTableHead');
    Ø1.setAttribute('style', 'width: 5%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    var Ø2 = WAVE.ce('div');
    Ø2.innerText = 'User';
    Ø2.setAttribute('class', 'cell head rDetailsTableHead');
    Ø2.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
    var Ø3 = WAVE.ce('div');
    Ø3.innerText = 'Assigned';
    Ø3.setAttribute('class', 'cell head rDetailsTableHead');
    Ø3.setAttribute('style', 'width: 15%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
    var Ø4 = WAVE.ce('div');
    Ø4.innerText = 'Operator';
    Ø4.setAttribute('class', 'cell head rDetailsTableHead');
    Ø4.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
    var Ø5 = WAVE.ce('div');
    Ø5.innerText = 'Unassigned';
    Ø5.setAttribute('class', 'cell head rDetailsTableHead');
    Ø5.setAttribute('style', 'width: 15%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
    var Ø6 = WAVE.ce('div');
    Ø6.innerText = 'Operator';
    Ø6.setAttribute('class', 'cell head rDetailsTableHead');
    Ø6.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
    var Ø7 = WAVE.ce('div');
    Ø7.innerText = 'Note';
    Ø7.setAttribute('class', 'cell head rDetailsTableHead');
    Ø7.setAttribute('style', 'width: 35%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
  }
  return Ø7;
﻿}
function createStatusGridRow(root, details) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  if(1==1) {
    var Ø1 = WAVE.ce('div');
    Ø1.innerText = details.Counter;
    Ø1.setAttribute('class', 'cell');
    Ø1.setAttribute('align', 'right');
    Ø1.setAttribute('style', 'width: 5%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    var Ø2 = WAVE.ce('div');
    Ø2.innerText = details.Completeness;
    Ø2.setAttribute('class', 'cell');
    Ø2.setAttribute('style', 'width: 5%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
    var Ø3 = WAVE.ce('div');
    Ø3.innerText = details.Status;
    Ø3.setAttribute('class', 'cell');
    Ø3.setAttribute('align', 'center');
    Ø3.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
    var Ø4 = WAVE.ce('div');
    Ø4.innerText = WAVE.dateTimeToString(details.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
    Ø4.setAttribute('class', 'cell');
    Ø4.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
    var Ø5 = WAVE.ce('div');
    Ø5.innerText = WAVE.dateTimeToString(details.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
    Ø5.setAttribute('class', 'cell');
    Ø5.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
    var Ø6 = WAVE.ce('div');
    Ø6.innerText = WAVE.dateTimeToString(details.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
    Ø6.setAttribute('class', 'cell');
    Ø6.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
    var Ø7 = WAVE.ce('div');
    Ø7.innerText = details.Assignee;
    Ø7.setAttribute('class', 'cell');
    Ø7.setAttribute('style', 'width: 30%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
    var Ø8 = WAVE.ce('div');
    Ø8.innerText = details.Description;
    Ø8.setAttribute('class', 'cell');
    Ø8.setAttribute('style', 'width: 20%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
  }
  return Ø8;
}

function createAssignmentGridRow(root, assignment) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  if(1==1) {
    var Ø1 = WAVE.ce('div');
    Ø1.innerText = assignment.Counter;
    Ø1.setAttribute('class', 'cell');
    Ø1.setAttribute('align', 'right');
    Ø1.setAttribute('style', 'width: 5%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    var Ø2 = WAVE.ce('div');
    Ø2.innerText = assignment.UserFirstName + ' ' + assignment.UserLastName + '(' +assignment.UserLogin+')';
    Ø2.setAttribute('class', 'cell');
    Ø2.setAttribute('align', 'right');
    Ø2.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
    var Ø3 = WAVE.ce('div');
    Ø3.innerText = WAVE.dateTimeToString(assignment.Open_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
    Ø3.setAttribute('class', 'cell');
    Ø3.setAttribute('style', 'width: 15%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
    var Ø4 = WAVE.ce('div');
    Ø4.innerText = assignment.OperatorOpenLogin;
    Ø4.setAttribute('class', 'cell');
    Ø4.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
    var Ø5 = WAVE.ce('div');
    Ø5.innerText = WAVE.dateTimeToString(assignment.Close_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
    Ø5.setAttribute('class', 'cell');
    Ø5.setAttribute('style', 'width: 15%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
    var Ø6 = WAVE.ce('div');
    Ø6.innerText = assignment.OperatorCloseLogin;
    Ø6.setAttribute('class', 'cell');
    Ø6.setAttribute('align', 'center');
    Ø6.setAttribute('style', 'width: 10%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
    var Ø7 = WAVE.ce('div');
    Ø7.innerText = assignment.Note;
    Ø7.setAttribute('class', 'cell');
    Ø7.setAttribute('style', 'width: 35%');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
  }
  return Ø7;
}

function buildChatForm(root, task) {
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
  Ø4.addEventListener('click', sendChatMessage1, false);
  Ø3.appendChild(Ø4);
  var Ø5 = WAVE.ce('a');
  Ø5.innerText = 'report';
  Ø5.setAttribute('class', 'button');
  Ø5.setAttribute('style', 'margin:4px 4px 4px 0px');
  Ø5.setAttribute('data-cproject', task.C_Project);
  Ø5.setAttribute('data-cissue', task.Counter);
  Ø5.setAttribute('data-report', 'chatreport');
  Ø5.addEventListener('click', openReport, false);
  Ø3.appendChild(Ø5);
  Ø1.appendChild(Ø3);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function buildChatMessage(root, task) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('class', 'ChatDiv');
  Ø1.setAttribute('id', 'chatMessage-'+task.Counter);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createChatItem(root, item) {
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
  Ø3.setAttribute('class', 'fView ChatItemNote');
  Ø1.appendChild(Ø3);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function buildChatReport(root, task) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  var Ø2 = WAVE.ce('a');
  Ø2.innerText = 'report';
  Ø2.setAttribute('data-cproject', task.C_Project);
  Ø2.setAttribute('data-cissue', task.Counter);
  Ø2.setAttribute('data-report', 'chatreport');
  Ø2.addEventListener('click', openReport, false);
  Ø2.setAttribute('class', 'button');
  Ø1.appendChild(Ø2);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function buildChatFilterForm(root, task) {
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
  Ø5.addEventListener('click', setChatFilter, false);
  Ø5.setAttribute('class', 'button');
  Ø4.appendChild(Ø5);
  Ø1.appendChild(Ø4);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function buildArea(root, taskCounter, area, canRemove) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.innerText = area.Name;
  Ø1.setAttribute('class', 'tag inline-block');
  Ø1.setAttribute('style', 'background-color: darkgreen');
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function removeComp(e) {
  e.stopPropagation();
  var iid = e.target.dataset.cissue;
  var pid = e.target.dataset.cproject;
  var cid = e.target.dataset.ccomp;
  var data = { issue: iid, component: cid, link: false };
  var link = ZHB.URIS.ForPROJECT_LINK_ISSUE_COMPONENT(pid, iid, cid);
  WAVE.ajaxCall(
       'POST',
       link,
       data,
       function (resp) {
         WAVE.removeElem(e.target.id);
         console.log("success");
       },
       function (resp) { console.log("error"); console.log(resp); },
       function (resp) { console.log("fail"); console.log(resp); },
       WAVE.CONTENT_TYPE_JSON_UTF8,
       WAVE.CONTENT_TYPE_JSON_UTF8
     );
}

function buildComponent(root, task, component, canRemove) {
  if (canRemove)
  {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    var Ø1 = WAVE.ce('div');
    Ø1.innerText = component.Name + ' X';
    Ø1.setAttribute('id', 'comp-' + component.Counter);
    Ø1.setAttribute('class', 'tag inline-block');
    Ø1.setAttribute('style', 'background-color: darkblue; cursor: pointer');
    Ø1.setAttribute('data-ccomp', component.Counter);
    Ø1.setAttribute('data-cissue', task.Counter);
    Ø1.setAttribute('data-cproject', task.C_Project);
    Ø1.addEventListener('click', removeComp, false);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;
  }
  else
  {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    var Ø1 = WAVE.ce('div');
    Ø1.innerText = component.Name;
    Ø1.setAttribute('class', 'tag inline-block');
    Ø1.setAttribute('style', 'background-color: darkblue');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;
  }
}

function buildAssignee(root, taskCounter, assignee, canRemove) {
﻿   var Ør = arguments[0];
   if (WAVE.isString(Ør))
     Ør = WAVE.id(Ør);
   var Ø1 = WAVE.ce('div');
   Ø1.innerText = assignee.UserLogin;
   Ø1.setAttribute('class', 'tag inline-block');
   Ø1.setAttribute('style', 'background-color: brown');
   if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
   return Ø1;
﻿}

﻿function createEditChatButton(root, item, task) {
﻿  if(item.HasEdit) {
﻿    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    var Ø1 = WAVE.ce('a');
    Ø1.innerText = 'edit';
    Ø1.setAttribute('class', 'button');
    Ø1.setAttribute('data-chatid', item.Counter);
    Ø1.setAttribute('data-note', item.Note);
    Ø1.setAttribute('data-cproject', task.C_Project);
    Ø1.setAttribute('data-cissue', task.Counter);
    Ø1.addEventListener('click', editChatItem, false);
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;
﻿  }    
﻿}

function buildEditChatDialog(root, item) {
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
}

﻿function buildAreasAndComponents(root, task, isPM) {
﻿  for (var i=0, l=task.Areas.length; i < l; i++) {
﻿    buildArea(root, task, task.Areas[i], isPM);
﻿  }
﻿  for (var i=0, l=task.Components.length; i < l; i++) {
﻿    buildComponent(root, task, task.Components[i], isPM);
﻿  }
﻿}

function buildAssigneeList(root, task, isPM) {
  for (var i=0, l=task.AssigneeList.length; i<l; i++) {
    buildAssignee(root, task, task.AssigneeList[i], isPM);
  }
}

function chatForm(task) {
  var link='project/{0}/issue/{1}/chat?id='.args(task.C_Project, task.Counter);
  WAVE.ajaxCall(
    'GET',
    link,
    null,
    function (resp) {
      chatRec[task.Counter] = new WAVE.RecordModel.Record(JSON.parse(resp));
      new WAVE.RecordModel.RecordView('chatForm' + task.Counter, chatRec[task.Counter]);
      console.log("success"); 
    },
    function (resp) { console.log("error"); },
    function (resp) { console.log("fail"); },
    WAVE.CONTENT_TYPE_JSON_UTF8, 
    WAVE.CONTENT_TYPE_JSON_UTF8    
  );
}

function chatFilterForm(task) {
  var link = 'project/{0}/issue/{1}/chatlist'.args(task.C_Project, task.Counter);
  WAVE.ajaxCall(
    'GET',
    link,
    null,
    function (resp) {
      // debugger;
      chatFilterRec[task.Counter] = new WAVE.RecordModel.Record(JSON.parse(resp));
      new WAVE.RecordModel.RecordView('ChatFilterForm' + task.Counter, chatFilterRec[task.Counter]);
      console.log("success");
    },
    function (resp) { console.log("error"); },
    function (resp) { console.log("fail"); },
    WAVE.CONTENT_TYPE_JSON_UTF8,
    WAVE.CONTENT_TYPE_JSON_UTF8
  );
}

function sendChatMessage1(e) {
  var iid = e.target.dataset.cissue;
  var pid = e.target.dataset.cproject; 
  sendChatMessage(pid, iid, "", chatRec[iid])
}

function sendChatMessage(pid, iid, cid, _rec) {
  var task = { Counter: iid, C_Project: pid, createImageData: cid };
  console.log(chatRec[iid]);
  var link = "project/{0}/issue/{1}/chat?id={2}".args(pid,iid, cid);
  WAVE.ajaxCall(
    'POST',
    link,
    _rec.data(),
    function (resp) {
      chatForm(task);  
      refreshChat(task);  
      console.log("success"); 
    },
    function (resp) { console.log("error"); console.log(resp); },
    function (resp) { console.log("fail"); console.log(resp); },
    WAVE.CONTENT_TYPE_JSON_UTF8, 
    WAVE.CONTENT_TYPE_JSON_UTF8
  ); 
}
  
function refreshChat(task) {
  var link = "/project/{0}/issue/{1}/chatlist".args(task.C_Project, task.Counter);
  var data = chatFilterRec[task.Counter].data();
  WAVE.ajaxCall(
    'POST',
    link,
    data,
    function (resp) {
      var rec = JSON.parse(resp);
      createChatItems(task, rec);
      console.log(rec);
    },
    function (resp) { console.log("error"); console.log(resp); },
    function (resp) { console.log("fail"); console.log(resp); },
    WAVE.CONTENT_TYPE_JSON_UTF8,
    WAVE.CONTENT_TYPE_JSON_UTF8
  );    
}

function createChatItems(task, rec) {
  var id = 'chatMessage-' + task.Counter;  
  document.getElementById(id).innerHTML = ""; 
  for (var i = 0, l = rec.Rows.length; i < l; i++) {
    createChatItem(id, rec.Rows[i]);
  }
  for (var i = 0, l = rec.Rows.length; i < l; i++) {
    var item = rec.Rows[i];
    createEditChatButton('chathedaeritem'+item.Counter, item, task);
  }

}

﻿function createGrid(root, gridId) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('id', gridId);
  Ø1.setAttribute('class', 'table');
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function buildStatusTab(root, task) {
  var gridID = "status-grid-" + task.Counter;
  buildStatusButtons(root, task);
  createGrid(root, gridID);
  createStatusHeader(gridID);
  for (var j = 0, l = task.Details.length; j < l; j++)
    createStatusGridRow(gridID, task.Details[j]);
}

function buildAssignmentTab(root, task) {
  var gridID = "assignment-grid-" + task.Counter;
  buildAssignmentButtons(root, task);
  createGrid(root, gridID);
  createAssignmentHeader(gridID);
  for (var j = 0, l = task.Assignments.length; j < l; j++)
    createAssignmentGridRow(gridID, task.Assignments[j]);
}

function buildChatTab(root, task) {
  buildChatFilterForm(root, task);
  buildChatForm(root, task);
  buildChatMessage(root, task);
  // buildChatReport(root, task);
  chatForm(task);
  chatFilterForm(task);
  // refreshChat(task);
}

﻿function openReport(e) {
﻿  var pid = e.target.dataset.cproject;
﻿  var iid = e.target.dataset.cissue;
  var report = e.target.dataset.report;
﻿  var link = "/project/{0}/issue/{1}/{2}".args(pid, iid, report);
﻿  window.open(link);
﻿}

function setChatFilter(e) {
  var iid = e.target.dataset.cissue;
  var pid = e.target.dataset.cproject;
  var task = { Counter: iid, C_Project: pid };
  refreshChat(task);
}

function editIssue1(e) {
  e.stopPropagation();
  editIssue(e.target.dataset.cproject, e.target.dataset.cissue);
}

function editChatItem(e) {
  e.stopPropagation();
  var chatId = e.target.dataset.chatid;
  var note = e.target.dataset.node;
  var iid = e.target.dataset.cissue;
  var pid = e.target.dataset.cproject;
  
  var link =  "project/{0}/issue/{1}/chat?id={2}".args(pid,iid, chatId);
  
  WAVE.ajaxCall(
    'GET',
    link,
    null,
    function (resp) {
      var rec = new WAVE.RecordModel.Record(JSON.parse(resp));
      var dlg = WAVE.GUI.Dialog({
        header:" Edit note",
        body:buildEditChatDialog(null, chatId),
        footer: buildStatusFooter(),
        onShow: function() {
          var rv = new WAVE.RecordModel.RecordView("V22", rec);
        },
        onClose: function(dlg, result) {
          if(result==WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
          rec.validate();
          if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED    
          sendChatMessage(pid, iid, chatId, rec);
          return WAVE.GUI.DLG_CANCEL;
        }  
      });    
    },
    function (resp) { console.log("error"); },
    function (resp) { console.log("fail"); },
    WAVE.CONTENT_TYPE_JSON_UTF8, 
    WAVE.CONTENT_TYPE_JSON_UTF8    
  );

  
}

function createTabs(root, task) {
  var statusId = "status-" + task.Counter;
  var statusContainer = "<div id={0}></div>".args(statusId);

  var assignmentId = "assignment-" + task.Counter;
  var assignmentContainer = "<div id={0}></div>".args(assignmentId);
  
  var chatId = "chatTab-{0}".args(task.Counter);
  var chatContainer = "<div id={0}></div>".args(chatId);

  var tabs = new WAVE.GUI.Tabs({
    DIV: WAVE.id(root),
    tabs: [
      {
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
    ]
  });
  tabs.eventBind(WAVE.GUI.EVT_TABS_TAB_CHANGED, function (sender, args) {
    console.log(args);
    if (args == "tChat") {
      refreshChat(task);
    };
  });

  buildStatusTab(statusId, task);
  buildAssignmentTab(assignmentId, task);
  buildChatTab(chatId, task);

}