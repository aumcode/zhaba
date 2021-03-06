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

      /***
       div
       {
         "?if(ZHB.Tasks.isPM)" {
           a="Edit Issue"
           {
             class="button"
             style="margin: 4px 4px 4px 0px"
             data-cissue=?task.Counter
             data-cproject=?task.C_Project
             data-detailsid=?detailsId
             on-click=ZHB.Tasks.Status.editIssue1
           }
           a="Resume"
           {
             style="margin: 4px 4px 4px 0px"
             data-nextstate=?resumeStatus
             data-cproject=?task.C_Project
             data-counter=?task.Counter
             on-click=ZHB.Tasks.Status.getOtherForm1
             class="button"
           }
           a="Cancel"
           {
             style="margin: 4px 4px 4px 0px"
             data-nextstate="X"
             data-cproject=?task.C_Project
             data-counter=?task.Counter
             on-click=ZHB.Tasks.Status.getOtherForm1
             class="button"
           }
         }
         a="Report"
         {
           data-cproject=?task.C_Project
           data-cissue=?task.Counter
           data-report='statusreport'
           on-click=ZHB.Tasks.Report.openReport
           class="button"
           style="margin: 4px 4px 4px 0px"
         }
       }
       ***/
    } else {
      /***
       div
       {
         "?if(ZHB.Tasks.isPM)" {

           a="Edit Issue"
           {
             class="button"
             style="margin: 4px 4px 4px 0px"
             data-cissue=?task.Counter
             data-cproject=?task.C_Project
             data-detailsid=?detailsId
             on-click=ZHB.Tasks.Status.editIssue1
           }

           "? for(var s=0, sl=task.NextState.length; s < sl; s++)" {
             a = "?ZHB.Tasks.Status.getStatusButtonName(task.NextState[s])"
             {
               style="margin: 4px 4px 4px 0px"
               data-nextstate=?task.NextState[s]
               data-cproject=?task.C_Project
               data-counter=?task.Counter
               on-click=ZHB.Tasks.Status.changeStatusDialog1
               class="button"
             }
           }
         }
         a="Report"
         {
           data-cproject=?task.C_Project
           data-cissue=?task.Counter
           data-report='statusreport'
           on-click=ZHB.Tasks.Report.openReport
           class="button"
           style="margin: 4px 4px 4px 0px"
         }
       }
       ***/
    }
  };

  published.createStatusHeader = function (root) {
    /***
     "?if(1==1)"
     {
       div="Status Date" {class="rst-cell rst-details-head" style="width: 9%"}
       div="Operator" {class="rst-cell rst-details-head" style="width: 10%"}
       div="%" {class="rst-cell rst-details-head" style="width: 3%"}
       div="Status" {class="rst-cell rst-details-head" style="width: 10%"}
       div="Start" {class="rst-cell rst-details-head" style="width: 6%"}
       div="Plan/Due" {class="rst-cell rst-details-head" style="width: 6%"}
       div="Complete" {class="rst-cell rst-details-head" style="width: 6%"}
       div="Assigned" {class="rst-cell rst-details-head" style="width: 20%"}
       div="Description"{ class="rst-cell rst-details-head"  style="width: 30%"}
     }
     ***/
  };


  published.createStatusGridRow = function (root, details) {
    /***
     "?if(1==1)"
     {
       div="?WAVE.dateTimeToString(details.Status_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE_TIME)"{ class="rst-cell rst-text-align-center rst-details-cell" style="width: 9%"}
       div="?details.Operator"{ class="rst-cell rst-text-align-center rst-details-cell" style="width: 10%"}
       div="?details.Completeness +'%'" { class="rst-cell rst-text-align-center rst-details-cell" style="width: 3%"}
       div
       {
         class="rst-cell rst-text-align-center rst-details-cell"
         style="width: 10%"
         div
         {
           align="center"
           div="?details.Priority"{ class="?'tag {0} inline-block'.args(ZHB.Tasks.Render.getPriorityStyle(details.Priority))" }
           div="?details.Status"{ class="?'tag {0} inline-block'.args(ZHB.Tasks.Render.getStatusStyle(details.Status))" }
           div="?details.Category_Name"{ class="tag tag-category inline-block" }
         }
       }
       div="?WAVE.dateTimeToString(details.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rst-cell rst-text-align-center rst-details-cell" style="width: 6%"}
       div="?WAVE.dateTimeToString(details.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rst-cell rst-text-align-center rst-details-cell" style="width: 6%"}
       div="?WAVE.dateTimeToString(details.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rst-cell rst-text-align-center rst-details-cell" style="width: 6%"}
       div=?details.Assignee { class="rst-cell rst-text-align-center rst-details-cell" style="width: 20%"}
       div="?details.Description"{ id="?'details-description'+details.Counter" class="rst-cell rst-details-cell" style="width: 30%"}
     }
     ***/
  };

  published.buildStatusBody = function (status) {
    /***
     div
     {
       id="statusForm"
       data-wv-rid="V2"
       div { data-wv-fname="Description" class="fView" }
     }
     ***/
  };

  published.buildStatusFooter = function () {
    /***
     div
     {
       align="right"
       style="margin: 5px"
       a="ok" {href="javascript:WAVE.GUI.currentDialog().ok()" style="margin: 5px; padding: 2px; width: 75px; height: 23px" class="button"}
       a="cancel" {href="javascript:WAVE.GUI.currentDialog().cancel()" style="margin: 5px; padding: 2px; width: 75px; height: 23px" class="button"}
     }

     ***/
  };

  published.buildIssueAssigneeStatusBody = function () {
    /***
     div
     {
       id="statusForm"
       data-wv-rid="V2"
       div { data-wv-fname="C_User" class="fView" data-wv-ctl="combo"}
       div { data-wv-fname="Open_TS" class="fView" }
       div { data-wv-fname="Close_TS" class="fView" }
       div { data-wv-fname="Description" class="fView" type="text" }
     }
     ***/

  };

  published.init = function (init) {

  };

  return published;
})();


