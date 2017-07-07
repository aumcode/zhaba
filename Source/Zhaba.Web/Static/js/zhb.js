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

function createBody(root) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('id', 'table');
  Ø1.setAttribute('class', 'rTable');
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createHeaders(root) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('class', 'rTableRow');
  var Ø2 = WAVE.ce('div');
  Ø2.innerText = 'ID';
  Ø2.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø2);
  var Ø3 = WAVE.ce('div');
  Ø3.innerText = 'Progress';
  Ø3.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø3);
  var Ø4 = WAVE.ce('div');
  Ø4.innerText = 'Status';
  Ø4.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø4);
  var Ø5 = WAVE.ce('div');
  Ø5.innerText = 'Start';
  Ø5.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø5);
  var Ø6 = WAVE.ce('div');
  Ø6.innerText = 'Plan\x2FDue';
  Ø6.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø6);
  var Ø7 = WAVE.ce('div');
  Ø7.innerText = 'Complete';
  Ø7.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø7);
  var Ø8 = WAVE.ce('div');
  Ø8.innerText = 'Assigned';
  Ø8.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø8);
  var Ø9 = WAVE.ce('div');
  Ø9.innerText = 'Project';
  Ø9.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø9);
  var Ø10 = WAVE.ce('div');
  Ø10.innerText = 'Issue';
  Ø10.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø10);
  var Ø11 = WAVE.ce('div');
  Ø11.innerText = 'Note';
  Ø11.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø11);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createRow(root, task) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('id', task.Counter);
  Ø1.setAttribute('class', 'expander rTableRow');
  var Ø2 = WAVE.ce('div');
  Ø2.innerText = task.Counter;
  Ø2.setAttribute('class', 'issue_id rTableCell');
  Ø2.setAttribute('align', 'right');
  Ø1.appendChild(Ø2);
  var Ø3 = WAVE.ce('div');
  Ø3.setAttribute('class', 'rTableCell completeness');
  var Ø4 = WAVE.ce('div');
  Ø4.setAttribute('class', 'bar');
  Ø4.setAttribute('style', getStatusBarStyle(task.Completeness));
  Ø3.appendChild(Ø4);
  var Ø5 = WAVE.ce('div');
  Ø5.innerText = task.Completeness;
  Ø5.setAttribute('data-cproject', task.C_Project);
  Ø5.setAttribute('data-cissue', task.Counter);
  Ø5.setAttribute('data-progress', task.Completeness);
  Ø5.setAttribute('data-description', task.Description);
  Ø5.setAttribute('data-status', task.statusId);
  Ø5.addEventListener('click', changeProgress1, false);
  Ø5.setAttribute('class', 'bar-value');
  Ø5.setAttribute('align', 'center');
  Ø3.appendChild(Ø5);
  Ø1.appendChild(Ø3);
  var Ø6 = WAVE.ce('div');
  Ø6.innerText = task.Status;
  Ø6.setAttribute('class', 'rTableCell');
  Ø6.setAttribute('style', getStatusStyle(task.Status));
  Ø6.setAttribute('align', 'center');
  Ø1.appendChild(Ø6);
  var Ø7 = WAVE.ce('div');
  Ø7.innerText = WAVE.dateTimeToString(task.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  Ø7.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø7);
  var Ø8 = WAVE.ce('div');
  Ø8.innerText = WAVE.dateTimeToString(task.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  Ø8.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø8);
  var Ø9 = WAVE.ce('div');
  Ø9.innerText = WAVE.dateTimeToString(task.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  Ø9.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø9);
  var Ø10 = WAVE.ce('div');
  Ø10.innerText = task.Assignee;
  Ø10.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø10);
  var Ø11 = WAVE.ce('div');
  Ø11.innerText = task.ProjectName;
  Ø11.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø11);
  var Ø12 = WAVE.ce('div');
  Ø12.innerText = task.Name;
  Ø12.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø12);
  var Ø13 = WAVE.ce('div');
  Ø13.innerText = task.Description;
  Ø13.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø13);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createRowDetails(root, id) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('id', 'details-'+id);
  Ø1.setAttribute('class', 'details rTableRow');
  var Ø2 = WAVE.ce('div');
  Ø2.setAttribute('class', 'rTableCell colspan');
  var Ø3 = WAVE.ce('div');
  var Ø4 = WAVE.ce('div');
  Ø4.setAttribute('id', 'tabs-'+id);
  Ø3.appendChild(Ø4);
  Ø2.appendChild(Ø3);
  Ø1.appendChild(Ø2);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function buildStatusButtons(root, task) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  if(pmperm) {
     for(var s=0, sl=task.NextState.length; s < sl; s++) {
      var Ø2 = WAVE.ce('a');
      Ø2.innerText = statuses[task.NextState[s]];
      Ø2.setAttribute('data-nextstate', task.NextState[s]);
      Ø2.setAttribute('data-cproject', task.C_Project);
      Ø2.setAttribute('data-counter', task.Counter);
      Ø2.addEventListener('click', changeStatusDialog1, false);
      Ø2.setAttribute('class', 'button');
      Ø1.appendChild(Ø2);
    }
  }
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function buildAssignmentButtons(root, task) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
   if(task.statusId !='X' && task.statusId !='C' && task.statusId !='D') {
    var Ø2 = WAVE.ce('a');
    Ø2.innerText = 'Add user';
    Ø2.setAttribute('href', 'javascript:changeStatusDialog("A",'+task.C_Project+', '+task.Counter+')');
    Ø2.setAttribute('class', 'button');
    Ø1.appendChild(Ø2);
  }
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createStatusHeader(root) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('class', 'rTableRow');
  var Ø2 = WAVE.ce('div');
  Ø2.innerText = 'ID';
  Ø2.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø2);
  var Ø3 = WAVE.ce('div');
  Ø3.innerText = 'Progress';
  Ø3.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø3);
  var Ø4 = WAVE.ce('div');
  Ø4.innerText = 'Status';
  Ø4.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø4);
  var Ø5 = WAVE.ce('div');
  Ø5.innerText = 'Start';
  Ø5.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø5);
  var Ø6 = WAVE.ce('div');
  Ø6.innerText = 'Plan\x2FDue';
  Ø6.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø6);
  var Ø7 = WAVE.ce('div');
  Ø7.innerText = 'Complete';
  Ø7.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø7);
  var Ø8 = WAVE.ce('div');
  Ø8.innerText = 'Assigned';
  Ø8.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø8);
  var Ø9 = WAVE.ce('div');
  Ø9.innerText = 'Project';
  Ø9.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø9);
  var Ø10 = WAVE.ce('div');
  Ø10.innerText = 'Issue';
  Ø10.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø10);
  var Ø11 = WAVE.ce('div');
  Ø11.innerText = 'Description';
  Ø11.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø11);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

﻿function createAssignmentHeader(root) {
﻿  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('class', 'rTableRow');
  var Ø2 = WAVE.ce('div');
  Ø2.innerText = 'ID';
  Ø2.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø2);
  var Ø3 = WAVE.ce('div');
  Ø3.innerText = 'Operator';
  Ø3.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø3);
  var Ø4 = WAVE.ce('div');
  Ø4.innerText = 'UserOpenLogin';
  Ø4.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø4);
  var Ø5 = WAVE.ce('div');
  Ø5.innerText = 'UserCloseLogin';
  Ø5.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø5);
  var Ø6 = WAVE.ce('div');
  Ø6.innerText = 'Assigned';
  Ø6.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø6);
  var Ø7 = WAVE.ce('div');
  Ø7.innerText = 'Unassigned';
  Ø7.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø7);
  var Ø8 = WAVE.ce('div');
  Ø8.innerText = 'Note';
  Ø8.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø8);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
﻿}
function createStatusGridRow(root, details) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('id', 'detailsRow-'+details.Counter);
  Ø1.setAttribute('class', 'rTableRow');
  var Ø2 = WAVE.ce('div');
  Ø2.innerText = details.Counter;
  Ø2.setAttribute('class', 'rTableCell');
  Ø2.setAttribute('align', 'right');
  Ø1.appendChild(Ø2);
  var Ø3 = WAVE.ce('div');
  Ø3.innerText = details.Completeness;
  Ø3.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø3);
  var Ø4 = WAVE.ce('div');
  Ø4.innerText = details.Status;
  Ø4.setAttribute('class', 'rTableCell');
  Ø4.setAttribute('align', 'center');
  Ø1.appendChild(Ø4);
  var Ø5 = WAVE.ce('div');
  Ø5.innerText = WAVE.dateTimeToString(details.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  Ø5.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø5);
  var Ø6 = WAVE.ce('div');
  Ø6.innerText = WAVE.dateTimeToString(details.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  Ø6.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø6);
  var Ø7 = WAVE.ce('div');
  Ø7.innerText = WAVE.dateTimeToString(details.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  Ø7.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø7);
  var Ø8 = WAVE.ce('div');
  Ø8.innerText = details.Assignee;
  Ø8.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø8);
  var Ø9 = WAVE.ce('div');
  Ø9.innerText = details.ProjectName;
  Ø9.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø9);
  var Ø10 = WAVE.ce('div');
  Ø10.innerText = details.Name;
  Ø10.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø10);
  var Ø11 = WAVE.ce('div');
  Ø11.innerText = details.Description;
  Ø11.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø11);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createAssignmentGridRow(root, assignment) {
  var Ør = arguments[0];
  if (WAVE.isString(Ør))
    Ør = WAVE.id(Ør);
  var Ø1 = WAVE.ce('div');
  Ø1.setAttribute('id', 'assignmentRow-'+assignment.Counter);
  Ø1.setAttribute('class', 'rTableRow');
  var Ø2 = WAVE.ce('div');
  Ø2.innerText = assignment.Counter;
  Ø2.setAttribute('class', 'rTableCell');
  Ø2.setAttribute('align', 'right');
  Ø1.appendChild(Ø2);
  var Ø3 = WAVE.ce('div');
  Ø3.innerText = assignment.UserLogin;
  Ø3.setAttribute('class', 'rTableCell');
  Ø3.setAttribute('align', 'right');
  Ø1.appendChild(Ø3);
  var Ø4 = WAVE.ce('div');
  Ø4.innerText = assignment.UserOpenLogin;
  Ø4.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø4);
  var Ø5 = WAVE.ce('div');
  Ø5.innerText = assignment.UserCloseLogin;
  Ø5.setAttribute('class', 'rTableCell');
  Ø5.setAttribute('align', 'center');
  Ø1.appendChild(Ø5);
  var Ø6 = WAVE.ce('div');
  Ø6.innerText = WAVE.dateTimeToString(assignment.OPEN_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  Ø6.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø6);
  var Ø7 = WAVE.ce('div');
  Ø7.innerText = WAVE.dateTimeToString(assignment.CLOSE_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
  Ø7.setAttribute('class', 'rTableCell');
  Ø1.appendChild(Ø7);
  var Ø8 = WAVE.ce('div');
  Ø8.innerText = assignment.Note;
  Ø8.setAttribute('class', 'rTableHead');
  Ø1.appendChild(Ø8);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createChatForm(root, task) {
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
  Ø4.setAttribute('data-cissue', task.Counter);
  Ø4.setAttribute('data-cproject', task.C_Project);
  Ø4.addEventListener('click', sendChatMessage1, false);
  Ø3.appendChild(Ø4);
  Ø1.appendChild(Ø3);
  if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
  return Ø1;
}

function createChatMessage(root, task) {
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
  Ø2.addEventListener('click', openChatReport, false);
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
      debugger;
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
  var task = { Counter: iid, C_Project: pid };
  console.log(chatRec[iid]);
  var link = "project/{0}/issue/{1}/chat".args(pid,iid);
  WAVE.ajaxCall(
    'POST',
    link,
    chatRec[iid].data(),
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
}

function buildStatusTab(root, task) {
  buildStatusButtons(root, task);
  createStatusHeader(root);
  for (var j = 0, l = task.Details.length; j < l; j++)
    createStatusGridRow(root, task.Details[j]);
}

function buildAssignmentTab(root, task) {
  buildAssignmentButtons(root, task);
  createAssignmentHeader(root);
  for (var j = 0, l = task.Assignments.length; j < l; j++)
    createAssignmentGridRow(root, task.Assignments[j]);
}

function buildChatTab(root, task) {
  buildChatReport(root, task);
  buildChatFilterForm(root, task);
  createChatForm(root, task);
  createChatMessage(root, task);
  chatForm(task);
  chatFilterForm(task)
  // refreshChat(task);
}

function openChatReport(e) {
  var pid = e.target.dataset.cproject;
  var iid = e.target.dataset.cissue;
  var link = "/project/{0}/issue/{1}/chatreport".args(pid, iid);
  window.open(link);
}

function setChatFilter(e) {
  var iid = e.target.dataset.cissue;
  var pid = e.target.dataset.cproject;
  var task = { Counter: iid, C_Project: pid };
  refreshChat(task);
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