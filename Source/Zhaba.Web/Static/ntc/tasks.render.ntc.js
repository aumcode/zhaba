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
    };

    published.createRow = function (root, task) {
        var detailsId = "details-" + task.Counter;
        /***
         "?if(1 == 1)"
         {
           div="?task.Counter"
           {
             class="rst-cell issue_id rst-expander"
             style="width: 3%"
             align="right"
     
             data-cissue=?task.Counter
             data-cproject=?task.C_Project
             data-details-id=?detailsId
           }
           div
           {
             class="rst-cell completeness rst-expander"
             style="width: 10%"
             data-details-id=?detailsId
             div
             {
               div
               {
                 align="center"
                 div="?task.Status"{ class="?'tag {0} inline-block'.args(getStatusStyle(task.Status))" }
                 div="?task.Category_Name"{ class="tag tag-category inline-block" }
               }
     
               div="?task.Completeness +'%'"
               {
                 data-cproject=?task.C_Project
                 data-cissue=?task.Counter
                 data-progress=?task.Completeness
                 data-description=?task.Description
                 data-status=?task.statusId
                 data-checkuser="?ZHB.Tasks.checkUser(task)"
                 on-click=ZHB.Tasks.changeProgress1
                 class="bar-value"
                 align="center"
               }
               div
               {
                 class="bar"
                 style="?getStatusBarStyle(task.Completeness)"
               }
             }
           }
           div
           {
             class="rst-cell rst-text-align-center rst-expander"
             data-details-id=?detailsId
             style="width: 12%;"
             div="?task.Priority"{ class="?'tag {0} inline-block'.args(getPriorityStyle(task.Priority))" }
             div="?buildDate(task)"{ class="inline-block" }
             div="?WAVE.dateTimeToString(task.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ 
               "?if(!task.Complete_Date)"{
                div="?'in {0}d'.args(task.Remaining)"{ class="?'time-r {0} inline-block'.args(getTimeRemainingStyle(task.Remaining))"}
               }
             }
           }
     
           div { id="?'assignee'+task.Counter" class="rst-cell rst-expander" style="width: 10%" data-details-id=?detailsId}
           div { id="?'ac'+task.Counter" class="rst-cell rst-expander" style="width: 15%" data-details-id=?detailsId}
           div="?task.ProjectName"{ class="rst-cell rst-expander" style="width: 10%" data-details-id=?detailsId}
           div="?task.Name"{ class="rst-cell rst-expander" style="width: 20%" data-details-id=?detailsId}
           div="?task.Description"
           {
             id="?'description'+task.Counter"
             class="rst-cell rst-expander"
             style="width: 20%"
             data-details-id=?detailsId
           }
         }
         ***/
    };

    published.buildAreaTag = function (root, cIssue, cArea, areaName) {
        /***
         div="?areaName"
         {
           class="tag tag-area inline-block"
         }
         ***/
    };

    published.buildCompTag = function (root, cIssue, cComp, compName) {
        /***
         div="?compName"
         {
           class="tag tag-component inline-block"
         }
         ***/
    };

    published.buildAssignee = function (root, assignee) {
        /***
         div="?assignee.UserLogin" { class="tag tag-assignee inline-block"}
         ***/
    };

    published.createRowDetails = function (root, id) {
        /***
         div
         {
           id="?'details-'+id"
           class="rst-cell rst-full"
           div { id="?'tabs-'+id" class="tab-control"}
         }
         ***/
    };


    published.createGrid = function (root, gridId) {
        /***
         div
         {
           id=?gridId
           class="rst-table"
         }
         ***/
    };

    published.buildIssueBody = function () {
        /***
         div
         {
           id="editIssueForm"
           data-wv-rid="V6"
     
           div { class="fView" data-wv-fname="Name" }
           div { class="fView" data-wv-fname="Start_Date" }
           div { class="fView" data-wv-fname="Due_Date" }
           div { class="fView" data-wv-fname="C_Milestone" data-wv-ctl="combo" }
           div { class="fView" data-wv-fname="C_Category" data-wv-ctl="combo" }
           div { class="fView" data-wv-fname="Priority" }
           div { class="fView" data-wv-fname="Description" }
         }
         ***/
    };

    published.buildProgressBody = function () {
        /***
         div
         {
           id="progressForm"
           data-wv-rid="V3"
           div { data-wv-fname="Value" class="fView" }
           div { data-wv-fname="Description" class="fView" }
         }
         ***/
    };

    published.init = function (init) {

    };

    published.getPriorityStyle = function(priority) {
      return getPriorityStyle(priority);
    };

    return published;
})();