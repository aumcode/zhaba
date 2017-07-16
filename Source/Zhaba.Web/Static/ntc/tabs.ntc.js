﻿function getStatusBarStyle(value) {
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
  /***
   "?if(1 == 1)"
   {
     div="ID" {class="rst-cell rst-head" style="width: 3%"}
     div="Status" {class="rst-cell rst-head" style="width: 10%"}
     div="Date" {class="rst-cell rst-head" style="width: 12%"}
     div="Assigned" {class="rst-cell rst-head" style="width: 10%"}
     div="Areas/Components" {class="rst-cell rst-head" style="width: 15%"}
     div="Project" {class="rst-cell rst-head" style="width: 10%"}
     div="Issue" {class="rst-cell rst-head" style="width: 20%"}
     div="Description"{class="rst-cell rst-head" style="width: 20%"}
   }
   ***/
}


function createAssignmentHeader(root) {
  /***
   "?if(1==1)"
   {
     div="*" {class="rst-cell rst-details-head" style="width: 1%"}
     div="ID" {class="rst-cell rst-details-head" style="width: 5%"}
     div="User" {class="rst-cell rst-details-head" style="width: 10%"}
     div="Assigned" {class="rst-cell rst-details-head" style="width: 15%"}
     div="Operator" {class="rst-cell rst-details-head" style="width: 10%"}
     div="Unassigned" {class="rst-cell rst-details-head" style="width: 15%"}
     div="Operator" {class="rst-cell rst-details-head" style="width: 10%"}
     div="Note" {class="rst-cell rst-details-head" style="width: 34%"}
   }
   ***/
}


function createAssignmentGridRow(root, assignment, task) {
  /***
   "?if(1==1)"
   {
     div
     {
       class="rst-cell rst-text-align-center rst-details-cell"
       align="right"
       style="width: 1%"
       "?if(!assignment.Close_TS)" {
         a="x"
         {
           class="button-delete"
           href="#"

           data-cproject=?task.C_Project
           data-cissue=?task.Counter
           data-cassignee=?assignment.Counter

           on-click=ZHB.Tasks.Status.editAssignee

         }
       }
     }
     div="?assignment.Counter"{ class="rst-cell rst-text-align-center rst-details-cell" align="right" style="width: 5%"}
     div="?assignment.UserFirstName + ' ' + assignment.UserLastName + '(' +assignment.UserLogin+')'"{ class="rst-cell rst-text-align-center rst-details-cell" align="right" style="width: 10%"}
     div="?WAVE.dateTimeToString(assignment.Open_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rst-cell rst-text-align-center rst-details-cell" style="width: 15%"}
     div="?assignment.OperatorOpenLogin" { class="rst-cell rst-text-align-center rst-details-cell" style="width: 10%"}
     div="?WAVE.dateTimeToString(assignment.Close_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rst-cell rst-text-align-center rst-details-cell" style="width: 15%"}
     div="?assignment.OperatorCloseLogin"{ class="rst-cell rst-text-align-center rst-details-cell" align="center" style="width: 10%"}
     div="?assignment.Note" {class="rst-cell rst-details-cell" style="width: 34%"}
   }
   ***/
}


function buildChatReport(root, task) {
  /***
   div
   {
     a="report"
     {
       data-cproject=?task.C_Project
       data-cissue=?task.Counter
       data-report='chatreport'
       on-click=openReport
       class="button"

     }
   }
   ***/
}

function buildAssignmentButtons(root, task) {
  /***
   div
   {
     "? if(task.statusId !='X' && task.statusId !='C' && task.statusId !='D' && ZHB.Tasks.isPM)" {
       a = "Add user"
       {
         data-counter=?task.Counter
         data-cproject=?task.C_Project
         data-nextstate="A"
         on-click=ZHB.Tasks.Status.changeStatusDialog1
         class="button"
         style="margin:4px 4px 4px 0px"
       }
     }
     a="report"
     {
       class="button"
       style="margin:4px 4px 4px 0px"
       data-cproject=?task.C_Project
       data-cissue=?task.Counter
       data-report='assignmentreport'
       on-click="openReport"
     }
   }
   ***/
}


function buildAssignmentTab(root, task) {
  var gridID = "assignment-grid-" + task.Counter;
  buildAssignmentButtons(root, task);
  ZHB.Tasks.Render.createGrid(root, gridID);
  createAssignmentHeader(gridID);
  for (var j = 0, l = task.Assignments.length; j < l; j++)
    createAssignmentGridRow(gridID, task.Assignments[j], task);
}

function openReport(e) {
  var pid = e.target.dataset.cproject;
  var iid = e.target.dataset.cissue;
  var report = e.target.dataset.report;
  var link = "/project/{0}/issue/{1}/{2}".args(pid, iid, report);
  window.open(link);
}

function buildAreasTab(areasId, task) {
  var link = "/project/{0}/issuearea?issue={1}".args(task.C_Project, task.Counter);
  $.post(link,
    null,
    function (grid) {
      $("#" + areasId).html(grid);
    }).fail(function (error) {
    console.log(error);
  });
}

function buildComponentsTab(componentsId, task) {
  var link = "/project/{0}/issuecomponent?issue={1}".args(task.C_Project, task.Counter);
  $.post(link,
    null,
    function (grid) {
      $("#" + componentsId).html(grid);
    }).fail(function (error) {
    console.log(error);
  });
}