﻿var chatRec = {};
var chatFilterRec = {};

function createBody(root) {
  /***
  div { id="table" class = "rTable" }
  ***/
}

function computeDate(task) {
  return "{0} - {1}".args(WAVE.dateTimeToString(task.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE), WAVE.dateTimeToString(task.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE));
}

function createHeaders(root) {
  /***
  div 
  {
    class="rTableRow"
    div="ID" {class="rTableHead" style="width: 50px"}
    div="Status" {class="rTableHead" style="width: 100px"}
    div="Date" {class="rTableHead" style="width: 250px"}
    div="Assigned" {class="rTableHead" style="width: 100px"}
    div="Ares/Components" {class="rTableHead" style="width: 100px"}
    div="Project" {class="rTableHead" style="width: 100px"}
    div="Issue" {class="rTableHead"}
    div="Description"{class="rTableHead" }
  }
  ***/
}

function createRow(root, task) {
  /***
  div
  {
    id=?task.Counter
    class="expander rTableRow"
            
    div="?task.Counter"
    {
      class="issue_id rTableCell"
      align="right"

      data-cissue=?task.Counter
      data-cproject=?task.C_Project
      on-click=editIssue1
    }
    div 
    { 
      div="?task.Status"{ style="?getStatusStyle(task.Status)" align="center"}
      class="rTableCell completeness" 
      div 
      { 
        class="bar" 
        style="?getStatusBarStyle(task.Completeness)" 
      }
      div="?task.Completeness +'%'" 
      { 
        data-cproject=?task.C_Project
        data-cissue=?task.Counter
        data-progress=?task.Completeness
        data-description=?task.Description
        data-status=?task.statusId
        on-click=changeProgress1
      
        class="bar-value" 
        align="center"
      }
    }
    div
    {
      class="rTableCell"
      div="?computeDate(task)"{}
      div="?WAVE.dateTimeToString(task.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{}
    }

    div { id="?'assignee'+task.Counter" class="rTableCell" }
    div { id="?'ac'+task.Counter" class="rTableCell" }
    div="?task.ProjectName"{ class="rTableCell" }
    div="?task.Name"{ class="rTableCell" }
    div="?task.Description"{ class="rTableCell" }
  }
  ***/
}

function createRowDetails(root, id) {
  /***
  div
  {
    id="?'details-'+id"
    class="details rTableRow"
    div
    {
      class="rTableCell colspan"
      div
      {
        div { id="?'tabs-'+id" class="tab-control"}
      }
    }
  }
  ***/
}

function buildStatusButtons(root, task) {
  /***
  div
  {
    "?if(pmperm)" {
      "? for(var s=0, sl=task.NextState.length; s < sl; s++)" {
          "?if(s != 0)"{
            a = "?statuses[task.NextState[s]]" 
            {
              style="margin: 4px 4px 4px 0px"
              data-nextstate=?task.NextState[s]
              data-cproject=?task.C_Project
              data-counter=?task.Counter 
              on-click="changeStatusDialog1"  
              class="button"
            }
          }
          "?else"{
            a = "?statuses[task.NextState[s]]" 
            {
              style="margin: 4px 4px 4px 0px"
              data-nextstate=?task.NextState[s]
              data-cproject=?task.C_Project
              data-counter=?task.Counter 
              on-click="changeStatusDialog1"  
              class="button"
            }
          }
      }
    }
    a="report" 
    {
      data-cproject=?task.C_Project
      data-cissue=?task.Counter
      data-report='statusreport' 
      on-click="openReport"  
      class="button"
      style="margin: 4px 4px 4px 0px"
    }
  }
  ***/
}

function buildAssignmentButtons(root, task) {
  /***
  div
  {
    "? if(task.statusId !='X' && task.statusId !='C' && task.statusId !='D' && pmperm)" {
      a = "Add user" 
      { 
        data-counter=?task.Counter
        data-cproject=?task.C_Project
        data-nextstate="A"
        on-click=changeStatusDialog1
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

function createStatusHeader(root) {
  /***
  div 
  {
    class="rTableRow"
    div="ID" {class="rTableHead rDetailsTableHead"}
    div="Progress" {class="rTableHead rDetailsTableHead"}
    div="Status" {class="rTableHead rDetailsTableHead"}
    div="Start" {class="rTableHead rDetailsTableHead"}
    div="Plan/Due" {class="rTableHead rDetailsTableHead"}
    div="Complete" {class="rTableHead rDetailsTableHead"}
    div="Assigned" {class="rTableHead rDetailsTableHead"}
    div="Description"{ class="rTableHead rDetailsTableHead" }
  }
  ***/
}

﻿function createAssignmentHeader(root) {
﻿  /***
   div 
   {
     class="rTableRow"
     div="ID" {class="rTableHead rDetailsTableHead"}
     div="User" {class="rTableHead rDetailsTableHead"}
     div="Assigned" {class="rTableHead rDetailsTableHead"}
     div="Operator" {class="rTableHead rDetailsTableHead"}
     div="Unassigned" {class="rTableHead rDetailsTableHead"}
     div="Operator" {class="rTableHead rDetailsTableHead"}
     div="Note" {class="rTableHead rDetailsTableHead"}
   }
   ***/
﻿}
function createStatusGridRow(root, details) {
  /***
  div
  {
    id="?'detailsRow-'+details.Counter"
    class="rTableRow"
            
    div="?details.Counter"{ class="rTableCell" align="right" }
    div="?details.Completeness" { class="rTableCell" }
    div="?details.Status"{ class="rTableCell" align="center"}
    div="?WAVE.dateTimeToString(details.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div="?WAVE.dateTimeToString(details.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div="?WAVE.dateTimeToString(details.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div=?details.Assignee { class="rTableCell" }
    div="?details.Description"{ class="rTableCell" }
  }
  ***/
}

function createAssignmentGridRow(root, assignment) {
  /***
  div
  {
    id="?'assignmentRow-'+assignment.Counter"
    class="rTableRow"

    div="?assignment.Counter"{ class="rTableCell" align="right" }
    div="?assignment.UserFirstName + ' ' + assignment.UserLastName + '(' +assignment.UserLogin+')'"{ class="rTableCell" align="right" }
    div="?WAVE.dateTimeToString(assignment.Open_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div="?assignment.OperatorOpenLogin" { class="rTableCell" }
    div="?WAVE.dateTimeToString(assignment.Close_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div="?assignment.OperatorCloseLogin"{ class="rTableCell" align="center"}
    div="?assignment.Note" {class="rTableCell"}
  }
  ***/
}

function buildChatForm(root, task) {
  /***
  div
  {
    id="?'chatForm'+task.Counter"

    class="fwDialogBody"
   
    data-wv-rid="?'chatForm'+task.Counter"
    div { data-wv-fname="Note" class="fView" data-wv-ctl="textarea"}
    div 
    { 
      a="send" 
      { 
        class="button"
        style="margin:4px 4px 4px 0px" 

        data-cissue=?task.Counter
        data-cproject=?task.C_Project

        on-click=sendChatMessage1 
      }
      a="report"
      {
        class="button"
        style="margin:4px 4px 4px 0px" 

        data-cproject=?task.C_Project
        data-cissue=?task.Counter
        data-report='chatreport'
        
        on-click=openReport
      }
    }
  }
  ***/
}

function buildChatMessage(root, task) {
  /***
  div
  {
    class="ChatDiv"
    id="?'chatMessage-'+task.Counter"
  }
  ***/
}

function createChatItem(root, item) {
  /***
  div
  {
    class="ChatItem"
    
    div="?item.Name +'('+item.Login+') :' + WAVE.dateTimeToString(item.Note_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE_TIME)" { class="fView ChatItemUser" }
    div="?item.Note" { class="fView ChatItemNote" }
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

function buildChatFilterForm(root, task) {
  /***
  div
  {
    id="?'ChatFilterForm'+task.Counter"
    data-wv-rid="?'chatFilterForm'+task.Counter"
    div { data-wv-fname="C_User" class="fView" data-wv-ctl="combo" style="display: inline-block; padding: 8px;"}
    div { data-wv-fname="Limit" class="fView" style="display: inline-block; padding: 8px;" }
    div
    {
      style="display: inline-block; padding: 8px;"
      a="filter"
      {
        data-cissue=?task.Counter
        data-cproject=?task.C_Project
        on-click=setChatFilter
        class="button"
      }
    }
  }
  ***/
}

function buildArea(root, area) {
    /***
    div="?area.Name+'; '" { class="fView" }
    ***/
}

function buildComponent(root, component) {
  /***
   div="?component.Name + '; '" { class="fView" } 
  ***/
}

﻿function buildAssignee(root, assignee) {
﻿  /***
    div="?assignee.UserLogin + '; '" { class="fView" } 
   ***/
﻿}

function buildAreasAndComponents(root, task) {
  for (var i=0, l=task.Areas.length; i < l; i++) {
    buildArea(root,  task.Areas[i]);    
  }
  for (var i=0, l=task.Components.length; i < l; i++) {
    buildComponent(root,  task.Components[i]);
  }
}

function buildAssigneeList(root, task) {
  // debugger;
  for (var i=0, l=task.AssigneeList.length; i<l; i++) {
    buildAssignee(root, task.AssigneeList[i]);
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
  buildChatFilterForm(root, task);
  buildChatForm(root, task);
  buildChatMessage(root, task);
  // buildChatReport(root, task);
  chatForm(task);
  chatFilterForm(task)
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