/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Assignment.Render = (function () {
    var published = {}
    ;

    published.buildAssignmentButtons = function (root, task) {
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
             on-click=ZHB.Tasks.Report.openReport
           }
         }
         ***/
    }

    published.createAssignmentHeader = function (root) {
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
    };

    published.createAssignmentGridRow = function (root, assignment, task) {
        /***
         "?if(1==1)"
         {
           div
           {
             class="rst-cell rst-text-align-center rst-details-cell"
             align="right"
             style="width: 1%"
             "?if(!assignment.Close_TS && ZHB.Tasks.isPM)" {
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

    published.init = function (init) {

    };

    return published;
})();