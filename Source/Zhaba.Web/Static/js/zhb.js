/*jshint devel: true,browser: true, sub: true */
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
        //todo setup fo only debug!
        console.log(response);
    };


    return published;
}());

﻿var chatRec = {};
var chatFilterRec = {};

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

function getStatusStyle(value) {
    return "status-tag {0}".args(value.toLowerCase());
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
}

function buildStatusButtons(root, task) {
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
          Ø2.addEventListener('click', editIssue1, false);
          Ø1.appendChild(Ø2);
          var Ø3 = WAVE.ce('a');
          Ø3.innerText = 'Resume';
          Ø3.setAttribute('style', 'margin: 4px 4px 4px 0px');
          Ø3.setAttribute('data-nextstate', resumeStatus);
          Ø3.setAttribute('data-cproject', task.C_Project);
          Ø3.setAttribute('data-counter', task.Counter);
          Ø3.addEventListener('click', getOtherForm1, false);
          Ø3.setAttribute('class', 'button');
          Ø1.appendChild(Ø3);
          var Ø4 = WAVE.ce('a');
          Ø4.innerText = 'Cancel';
          Ø4.setAttribute('style', 'margin: 4px 4px 4px 0px');
          Ø4.setAttribute('data-nextstate', 'X');
          Ø4.setAttribute('data-cproject', task.C_Project);
          Ø4.setAttribute('data-counter', task.Counter);
          Ø4.addEventListener('click', getOtherForm1, false);
          Ø4.setAttribute('class', 'button');
          Ø1.appendChild(Ø4);
        }
        var Ø5 = WAVE.ce('a');
        Ø5.innerText = 'report';
        Ø5.setAttribute('data-cproject', task.C_Project);
        Ø5.setAttribute('data-cissue', task.Counter);
        Ø5.setAttribute('data-report', 'statusreport');
        Ø5.addEventListener('click', openReport, false);
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
          Ø2.addEventListener('click', editIssue1, false);
          Ø1.appendChild(Ø2);
           for(var s=0, sl=task.NextState.length; s < sl; s++) {
            var Ø3 = WAVE.ce('a');
            Ø3.innerText = statuses[task.NextState[s]];
            Ø3.setAttribute('style', 'margin: 4px 4px 4px 0px');
            Ø3.setAttribute('data-nextstate', task.NextState[s]);
            Ø3.setAttribute('data-cproject', task.C_Project);
            Ø3.setAttribute('data-counter', task.Counter);
            Ø3.addEventListener('click', changeStatusDialog1, false);
            Ø3.setAttribute('class', 'button');
            Ø1.appendChild(Ø3);
          }
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
    }
}

function buildAssignmentButtons(root, task) {
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
      Ø1.setAttribute('class', 'rst-cell rst-details-head');
      Ø1.setAttribute('style', 'width: 5%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
      var Ø2 = WAVE.ce('div');
      Ø2.innerText = 'Progress';
      Ø2.setAttribute('class', 'rst-cell rst-details-head');
      Ø2.setAttribute('style', 'width: 5%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
      var Ø3 = WAVE.ce('div');
      Ø3.innerText = 'Status';
      Ø3.setAttribute('class', 'rst-cell rst-details-head');
      Ø3.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
      var Ø4 = WAVE.ce('div');
      Ø4.innerText = 'Start';
      Ø4.setAttribute('class', 'rst-cell rst-details-head');
      Ø4.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø4);
      var Ø5 = WAVE.ce('div');
      Ø5.innerText = 'Plan\x2FDue';
      Ø5.setAttribute('class', 'rst-cell rst-details-head');
      Ø5.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø5);
      var Ø6 = WAVE.ce('div');
      Ø6.innerText = 'Complete';
      Ø6.setAttribute('class', 'rst-cell rst-details-head');
      Ø6.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø6);
      var Ø7 = WAVE.ce('div');
      Ø7.innerText = 'Assigned';
      Ø7.setAttribute('class', 'rst-cell rst-details-head');
      Ø7.setAttribute('style', 'width: 30%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
      var Ø8 = WAVE.ce('div');
      Ø8.innerText = 'Description';
      Ø8.setAttribute('class', 'rst-cell rst-details-head');
      Ø8.setAttribute('style', 'width: 20%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
    }
    return Ø8;
}

function createAssignmentHeader(root) {
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
}

function createStatusGridRow(root, details) {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    if(1==1) {
      var Ø1 = WAVE.ce('div');
      Ø1.innerText = details.Counter;
      Ø1.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø1.setAttribute('align', 'right');
      Ø1.setAttribute('style', 'width: 5%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
      var Ø2 = WAVE.ce('div');
      Ø2.innerText = details.Completeness +'%';
      Ø2.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø2.setAttribute('style', 'width: 5%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø2);
      var Ø3 = WAVE.ce('div');
      Ø3.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø3.setAttribute('style', 'width: 10%');
    var Ø4 = WAVE.ce('div');
    Ø4.setAttribute('align', 'center');
    var Ø5 = WAVE.ce('div');
    Ø5.innerText = details.Status;
    Ø5.setAttribute('class', 'tag {0} inline'.args(getStatusStyle(details.Status)));
    Ø4.appendChild(Ø5);
    var Ø6 = WAVE.ce('div');
    Ø6.innerText = details.Category_Name;
    Ø6.setAttribute('class', 'tag tag-category inline');
    Ø4.appendChild(Ø6);
    Ø3.appendChild(Ø4);
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø3);
      var Ø7 = WAVE.ce('div');
      Ø7.innerText = WAVE.dateTimeToString(details.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
      Ø7.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø7.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø7);
      var Ø8 = WAVE.ce('div');
      Ø8.innerText = WAVE.dateTimeToString(details.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
      Ø8.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø8.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø8);
      var Ø9 = WAVE.ce('div');
      Ø9.innerText = WAVE.dateTimeToString(details.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
      Ø9.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø9.setAttribute('style', 'width: 10%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø9);
      var Ø10 = WAVE.ce('div');
      Ø10.innerText = details.Assignee;
      Ø10.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø10.setAttribute('style', 'width: 30%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø10);
      var Ø11 = WAVE.ce('div');
      Ø11.innerText = details.Description;
      Ø11.setAttribute('id', 'details-description'+details.Counter);
      Ø11.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø11.setAttribute('style', 'width: 20%');
      if (WAVE.isObject(Ør)) Ør.appendChild(Ø11);
    }
    return Ø11;
}

function createAssignmentGridRow(root, assignment, task) {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    if(1==1) {
      var Ø1 = WAVE.ce('div');
      Ø1.setAttribute('class', 'rst-cell rst-text-align-center rst-details-cell');
      Ø1.setAttribute('align', 'right');
      Ø1.setAttribute('style', 'width: 1%');
    if(!assignment.Close_TS) {
      var Ø2 = WAVE.ce('a');
      Ø2.innerText = 'x';
      Ø2.setAttribute('class', 'button-delete');
      Ø2.setAttribute('href', '#');
      Ø2.setAttribute('data-cproject', task.C_Project);
      Ø2.setAttribute('data-cissue', task.Counter);
      Ø2.setAttribute('data-cassignee', assignment.Counter);
      Ø2.addEventListener('click', editAssignee, false);
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
    Ø3.setAttribute('id', 'chat-note'+item.Counter);
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

function createEditChatButton(root, item, task) {
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
        Ø1.addEventListener('click', editChatItem, false);
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    }
}

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

function chatForm(task) {
    var link = 'project/{0}/issue/{1}/chat?id='.args(task.C_Project, task.Counter);
    WAVE.ajaxCall(
        'GET',
        link,
        null,
        function(resp) {
            chatRec[task.Counter] = new WAVE.RecordModel.Record(JSON.parse(resp));
            new WAVE.RecordModel.RecordView('chatForm' + task.Counter, chatRec[task.Counter]);

        },
        function(resp) { console.log("error"); },
        function(resp) { console.log("fail"); },
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
        function(resp) {
            // debugger;
            chatFilterRec[task.Counter] = new WAVE.RecordModel.Record(JSON.parse(resp));
            new WAVE.RecordModel.RecordView('ChatFilterForm' + task.Counter, chatFilterRec[task.Counter]);

        },
        function(resp) { console.log("error"); },
        function(resp) { console.log("fail"); },
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
    var link = "project/{0}/issue/{1}/chat?id={2}".args(pid, iid, cid);
    WAVE.ajaxCall(
        'POST',
        link,
        _rec.data(),
        function(resp) {
            chatForm(task);
            ZHB.Tasks.Chat.refreshChat(task);

        },
        function(resp) {
            console.log("error");
            console.log(resp);
        },
        function(resp) {
            console.log("fail");
            console.log(resp);
        },
        WAVE.CONTENT_TYPE_JSON_UTF8,
        WAVE.CONTENT_TYPE_JSON_UTF8
    );
}

function createChatItems(task, rec) {
    var id = 'chatMessage-' + task.Counter;
    document.getElementById(id).innerHTML = "";
    for (var i = 0, l = rec.Rows.length; i < l; i++) {
        var item = rec.Rows[i];
        createChatItem(id, item);
        document.getElementById('chat-note' + item.Counter).innerHTML = WAVE.markup(item.Note);
        createEditChatButton('chathedaeritem' + item.Counter, item, task);
    }
    /*
      for (var i = 0, l = rec.Rows.length; i < l; i++) {
        var item = rec.Rows[i];
      }
    */

}


function createGrid(root, gridId) {
    var Ør = arguments[0];
    if (WAVE.isString(Ør))
      Ør = WAVE.id(Ør);
    var Ø1 = WAVE.ce('div');
    Ø1.setAttribute('id', gridId);
    Ø1.setAttribute('class', 'rst-table');
    if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
    return Ø1;
}

function buildStatusTab(root, task) {
    var gridID = "status-grid-" + task.Counter;
    buildStatusButtons(root, task);
    createGrid(root, gridID);
    createStatusHeader(gridID);
    for (var j = 0, l = task.Details.length; j < l; j++) {
        var d = task.Details[j];
        createStatusGridRow(gridID, d);
        document.getElementById('details-description' + d.Counter).innerHTML = WAVE.markup(d.Description != null ? d.Description : '');
    }
}

function buildAssignmentTab(root, task) {
    var gridID = "assignment-grid-" + task.Counter;
    buildAssignmentButtons(root, task);
    createGrid(root, gridID);
    createAssignmentHeader(gridID);
    for (var j = 0, l = task.Assignments.length; j < l; j++)
        createAssignmentGridRow(gridID, task.Assignments[j], task);
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


function openReport(e) {
    var pid = e.target.dataset.cproject;
    var iid = e.target.dataset.cissue;
    var report = e.target.dataset.report;
    var link = "/project/{0}/issue/{1}/{2}".args(pid, iid, report);
    window.open(link);
}

function setChatFilter(e) {
    var iid = e.target.dataset.cissue;
    var pid = e.target.dataset.cproject;
    var task = { Counter: iid, C_Project: pid };
    ZHB.Tasks.Chat.refreshChat(task);
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

    var link = "project/{0}/issue/{1}/chat?id={2}".args(pid, iid, chatId);

    WAVE.ajaxCall(
        'GET',
        link,
        null,
        function(resp) {
            var rec = new WAVE.RecordModel.Record(JSON.parse(resp));
            var dlg = WAVE.GUI.Dialog({
                header: " Edit note",
                body: buildEditChatDialog(null, chatId),
                footer: buildStatusFooter(),
                onShow: function() {
                    var rv = new WAVE.RecordModel.RecordView("V22", rec);
                },
                onClose: function(dlg, result) {
                    if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                    rec.validate();
                    if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED
                    sendChatMessage(pid, iid, chatId, rec);
                    return WAVE.GUI.DLG_CANCEL;
                }
            });
        },
        function(resp) { console.log("error"); },
        function(resp) { console.log("fail"); },
        WAVE.CONTENT_TYPE_JSON_UTF8,
        WAVE.CONTENT_TYPE_JSON_UTF8
    );


}

function buildAreasTab(areasId, task) {
    var link = "/project/{0}/issuearea?issue={1}".args(task.C_Project, task.Counter);
    $.post(link,
        null,
        function(grid) {
            $("#" + areasId).html(grid);
        }).
    fail(function(error) {
        console.log(error);
    });
}

function buildComponentsTab(componentsId, task) {
    var link = "/project/{0}/issuecomponent?issue={1}".args(task.C_Project, task.Counter);
    $.post(link,
        null,
        function(grid) {
            $("#" + componentsId).html(grid);
        }).
    fail(function(error) {
        console.log(error);
    });
}

function editAssignee(e) {
    var pid = e.target.dataset.cproject;
    var iid = e.target.dataset.cissue;
    var id = e.target.dataset.cassignee;
    var link = "/project/{0}/issue/{1}/issueassign?id={2}".args(pid, iid, id);
    WAVE.ajaxCall(
        'GET',
        link,
        null,
        function(resp) {
            var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
            _rec.fieldByName('Open_TS').readonly(true);
            _rec.fieldByName('C_User').readonly(true);
            var dlg = WAVE.GUI.Dialog({
                header: "Unassignee for Issue [{0}]".args(iid),
                body: buildIssueAssigneeStatusBody(),
                footer: buildStatusFooter(),
                onShow: function() {
                    var rv = new WAVE.RecordModel.RecordView("V2", _rec);
                },
                onClose: function(dlg, result) {
                    if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                    _rec.validate();
                    if (!_rec.valid()) return WAVE.GUI.DLG_UNDEFINED
                    WAVE.ajaxCall(
                        'POST',
                        link,
                        _rec.data(),
                        function(resp) {
                            ZHB.Tasks.scheduleFetch();
                        },
                        function(resp) { console.log("error"); },
                        function(resp) { console.log("fail"); },
                        WAVE.CONTENT_TYPE_JSON_UTF8,
                        WAVE.CONTENT_TYPE_JSON_UTF8
                    );
                    return WAVE.GUI.DLG_CANCEL;
                }

            });
        },
        function(resp) { console.log("error"); },
        function(resp) { console.log("fail"); },
        WAVE.CONTENT_TYPE_JSON_UTF8,
        WAVE.CONTENT_TYPE_JSON_UTF8
    );
}

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
        if (chk.checked) {
          var el = WAVE.id(acId);
          if (el) {
            var link='/project/{0}/area?counter={1}'.args(cProject,cArea);
            WAVE.ajaxCall(
              'GET',
              link,
              null,
              function (resp) {
                var data = new WAVE.RecordModel.Record(JSON.parse(resp));
                var areaName=data.data().Name;
                ZHB.Tasks.Render.buildAreaTag(acId, cIssue, cArea, areaName);
              },
              function (resp) { console.log("error"); },
              function (resp) { console.log("fail"); },
              WAVE.CONTENT_TYPE_JSON_UTF8,
              WAVE.CONTENT_TYPE_JSON_UTF8
            );
          }
        } else {
          WAVE.removeElem(areaId);
        }

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
        if (chk.checked) {
          var el = WAVE.id(acId);
          if (el) {
            var link='/project/{0}/component?counter={1}'.args(cProject,cComponent);
            WAVE.ajaxCall(
              'GET',
              link,
              null,
              function (resp) {
                var data = new WAVE.RecordModel.Record(JSON.parse(resp));
                var compName=data.data().Name;
                ZHB.Tasks.Render.buildCompTag(acId, cIssue, cComponent, compName);
              },
              function (resp) { console.log("error"); },
              function (resp) { console.log("fail"); },
              WAVE.CONTENT_TYPE_JSON_UTF8,
              WAVE.CONTENT_TYPE_JSON_UTF8
            );
          }
        } else {
         WAVE.removeElem(compId);
        }
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

        buildStatusTab(statusId, task);
        buildAssignmentTab(assignmentId, task);
        buildChatTab(chatId, task);
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
    
    published.init = function(init) {
        initFilter(init.filter);
        published.isPM = init.pmPerm;
        getTasks();
        ZHB.Tasks.Chat.init({tasks: fTasks});
    };

    published.scheduleFetch = function() { scheduleFetch(); };
    
    return published;
})();

/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Render = (function () {
    "use strict";
    var published = {}
    ;
    
    published.createRow = function(root, task) {
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
        Ø5.setAttribute('class', 'tag {0} inline'.args(getStatusStyle(task.Status)));
        Ø4.appendChild(Ø5);
        var Ø6 = WAVE.ce('div');
        Ø6.innerText = task.Category_Name;
        Ø6.setAttribute('class', 'tag tag-category inline');
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
        Ø12.innerText = buildDueDate(task);
        Ø9.appendChild(Ø12);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø9);
          var Ø13 = WAVE.ce('div');
          Ø13.setAttribute('id', 'assignee'+task.Counter);
          Ø13.setAttribute('class', 'rst-cell rst-expander');
          Ø13.setAttribute('style', 'width: 10%');
          Ø13.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø13);
          var Ø14 = WAVE.ce('div');
          Ø14.setAttribute('id', 'ac'+task.Counter);
          Ø14.setAttribute('class', 'rst-cell rst-expander');
          Ø14.setAttribute('style', 'width: 15%');
          Ø14.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø14);
          var Ø15 = WAVE.ce('div');
          Ø15.innerText = task.ProjectName;
          Ø15.setAttribute('class', 'rst-cell rst-expander');
          Ø15.setAttribute('style', 'width: 10%');
          Ø15.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø15);
          var Ø16 = WAVE.ce('div');
          Ø16.innerText = task.Name;
          Ø16.setAttribute('class', 'rst-cell rst-expander');
          Ø16.setAttribute('style', 'width: 20%');
          Ø16.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø16);
          var Ø17 = WAVE.ce('div');
          Ø17.innerText = task.Description;
          Ø17.setAttribute('id', 'description'+task.Counter);
          Ø17.setAttribute('class', 'rst-cell rst-expander');
          Ø17.setAttribute('style', 'width: 20%');
          Ø17.setAttribute('data-details-id', detailsId);
          if (WAVE.isObject(Ør)) Ør.appendChild(Ø17);
        }
        return Ø17;
    };
    
    published.buildAreaTag = function (root, cIssue, cArea, areaName) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.innerText = areaName;
        Ø1.setAttribute('id', 'issue-' + cIssue + '-areatag-'+cArea);
        Ø1.setAttribute('data-cissue', cIssue);
        Ø1.setAttribute('data-carea', cArea);
        Ø1.setAttribute('class', 'tag tag-area inline-block');
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };
    
    published.buildCompTag = function(root, cIssue, cComp, compName) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.innerText = compName;
        Ø1.setAttribute('id', 'issue-' + cIssue + '-comptag-' + cComp);
        Ø1.setAttribute('data-cissue', cIssue);
        Ø1.setAttribute('data-ccomponent', cComp);
        Ø1.setAttribute('class', 'tag tag-component inline-block');
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };
    
    published.buildAssignee = function(root, assignee) {
        var Ør = arguments[0];
        if (WAVE.isString(Ør))
          Ør = WAVE.id(Ør);
        var Ø1 = WAVE.ce('div');
        Ø1.innerText = assignee.UserLogin;
        Ø1.setAttribute('class', 'tag tag-assignee inline-block');
        if (WAVE.isObject(Ør)) Ør.appendChild(Ø1);
        return Ø1;
    };
    
    published.createRowDetails = function(root, id) {
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
    
    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Chat = (function() {
    "use strict";
    var published = {},
        fScheduleTimer,
        fTasks
    
    ;
    
    function schedulerTask() {
        if (fScheduleTimer) clearTimeout(fScheduleTimer);
        WAVE.each(fTasks, function (task) {
           refreshChat(task); 
        });
        fScheduleTimer = setTimeout(schedulerTask, 20000);
    }

    published.refreshChat = function(task) {
        var link = "/project/{0}/issue/{1}/chatlist".args(task.C_Project, task.Counter);
        var data = chatFilterRec[task.Counter].data();
        WAVE.ajaxCall(
            'POST',
            link,
            data,
            function(resp) {
                var rec = JSON.parse(resp);
                createChatItems(task, rec);
            },
            ZHB.errorLog,
            ZHB.errorLog,
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    };
    
    published.init = function (init) {
        fTasks = init.tasks;
        schedulerTask();    
    };

    return published;
})();
/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Areas = (function() {
    "use strict";
    var published = {};



    return published;
})();