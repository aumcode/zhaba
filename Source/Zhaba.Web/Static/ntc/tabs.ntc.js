﻿var chatRec={};

function createBody(root) {
  /***
  div { id="table" class = "rTable" }
  ***/
}

function createHeaders(root) {
  /***
  div 
  {
    class="rTableRow"
    div="ID" {class="rTableHead"}
    div="Progress" {class="rTableHead"}
    div="Status" {class="rTableHead"}
    div="Start" {class="rTableHead"}
    div="Plan/Due" {class="rTableHead"}
    div="Complete" {class="rTableHead"}
    div="Assigned" {class="rTableHead"}
    div="Project" {class="rTableHead"}
    div="Issue" {class="rTableHead"}
    div="Note"{ class="rTableHead" }
  }
  ***/
}

function createRow(root, task) {
  /***
  div
  {
    id=?task.Counter
    class="expander rTableRow"
            
    div="?task.Counter"{ class="issue_id rTableCell" align="right" }
    div 
    { 
      class="rTableCell completeness" 
      div 
      { 
        class="bar" 
        style="?getStatusBarStyle(task.Completeness)" 
      }
      div="?task.Completeness" 
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
    div="?task.Status"{ class="rTableCell" style="?getStatusStyle(task.Status)" align="center"}
    div="?WAVE.dateTimeToString(task.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div="?WAVE.dateTimeToString(task.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div="?WAVE.dateTimeToString(task.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div=?task.Assignee { class="rTableCell" }
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
        div { id="?'tabs-'+id" }
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
          a = "?statuses[task.NextState[s]]" 
          { 
            data-nextstate=?task.NextState[s]
            data-cproject=?task.C_Project
            data-counter=?task.Counter 
            on-click="changeStatusDialog1"  
            class="button"   
          }
      }
    }
  }
  ***/
}

function buildAssignmentButtons(root, task) {
  /***
  div
  {
    "? if(task.statusId !='X' && task.statusId !='C' && task.statusId !='D')" {
      a = "Add user" { href="?'javascript:changeStatusDialog(\"A\",'+task.C_Project+', '+task.Counter+')'"  class="button" }
    }
  }
  ***/
}

function createStatusHeader(root) {
  /***
  div 
  {
    class="rTableRow"
    div="ID" {class="rTableHead"}
    div="Progress" {class="rTableHead"}
    div="Status" {class="rTableHead"}
    div="Start" {class="rTableHead"}
    div="Plan/Due" {class="rTableHead"}
    div="Complete" {class="rTableHead"}
    div="Assigned" {class="rTableHead"}
    div="Project" {class="rTableHead"}
    div="Issue" {class="rTableHead"}
    div="Description"{ class="rTableHead" }
  }
  ***/
}

function createAssignmentHeader(root) {
  /***
  div 
  {
    class="rTableRow"
    div="ID" {class="rTableHead"}
    div="Progress" {class="rTableHead"}
    div="Status" {class="rTableHead"}
    div="Start" {class="rTableHead"}
    div="Plan/Due" {class="rTableHead"}
    div="Complete" {class="rTableHead"}
    div="Assigned" {class="rTableHead"}
    div="Project" {class="rTableHead"}
    div="Issue" {class="rTableHead"}
    div="Description"{ class="rTableHead" }
  }
  ***/
}

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
    div="?details.ProjectName"{ class="rTableCell" }
    div="?details.Name"{ class="rTableCell" }
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
    div="?assignment.UserLogin"{ class="rTableCell" align="right" }
    div="?assignment.UserOpenLogin" { class="rTableCell" }
    div="?assignment.UserCloseLogin"{ class="rTableCell" align="center"}
    div="?WAVE.dateTimeToString(assignment.OPEN_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div="?WAVE.dateTimeToString(assignment.CLOSE_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rTableCell" }
    div="?assignment.Note" {class="rTableHead"}
  }
  ***/
}

function createChatForm(root, task) {
  /***
  div
  {
    id="?'chatForm'+task.Counter"
   
    data-wv-rid="?'chatForm'+task.Counter"
    div { data-wv-fname="Note" class="fView" data-wv-ctl="textarea"}
    div 
    { 
      a="send" 
      { 
        class="button"
        data-cissue=?task.Counter
        data-cproject=?task.C_Project
        on-click=sendChatMessage1 
      }
    }
  }
  ***/
}

function createChatMessage(root, task) {
  /***
  div
  {
    id="?'chatMessage-'+task.Counter"
  }
  ***/
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

function sendChatMessage1(e) {
  var iid = e.target.dataset.cissue;
  var pid = e.target.dataset.cproject;    
  console.log(chatRec[iid]);
  var link = "project/{0}/issue/{1}/chat".args(pid,iid);
  WAVE.ajaxCall(
    'POST',
    link,
    chatRec[iid].data(),
    function (resp) { 
      refreshChat(iid);  
      console.log("success"); 
    },
    function (resp) { console.log("error"); console.log(resp); },
    function (resp) { console.log("fail"); console.log(resp); },
    WAVE.CONTENT_TYPE_JSON_UTF8, 
    WAVE.CONTENT_TYPE_JSON_UTF8
  ); 
}
  
function refreshChat(task) {
  var link = "/project/{0}/ussue/{1}/chatlist".args(task.C_Project, task.Counter);
  WAVE.ajaxCall(
    'GET',
    link,
    null,
    function (resp) {
      var rec = JSON.parse(resp);
      console.log(rec);
    },
    function (resp) { console.log("error"); console.log(resp); },
    function (resp) { console.log("fail"); console.log(resp); },
    WAVE.CONTENT_TYPE_JSON_UTF8,
    WAVE.CONTENT_TYPE_JSON_UTF8
  );    
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
  createChatForm(root, task);
  createChatMessage(root, task);
  chatForm(task);
  refreshChat(task);
};

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
    if (args = "tChat") {
      refreshChat(task);
    };
  });

  buildStatusTab(statusId, task);
  buildAssignmentTab(assignmentId, task);
  buildChatTab(chatId, task);
}