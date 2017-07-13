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
  }
  else if (value == 65) {
    red = 255;
    green = 255;
  }
  else if (value > 65) {
    red = 255 - ((value - 65) * 7);
    green = 255;
  }
  else if (value == 100) {
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
    div="ID" {class="cell head" style="width: 3%"}
    div="Status" {class="cell head" style="width: 10%"}
    div="Date" {class="cell head" style="width: 12%"}
    div="Assigned" {class="cell head" style="width: 10%"}
    div="Areas/Components" {class="cell head" style="width: 15%"}
    div="Project" {class="cell head" style="width: 10%"}
    div="Issue" {class="cell head" style="width: 20%"}
    div="Description"{class="cell head" style="width: 20%"}
  }
  ***/
}

function createRow(root, task) {
  var detailsId = "details-" + task.Counter;
  /***
  "?if(1 == 1)" 
  {
    div="?task.Counter"
    {
      class="cell issue_id expander"
      style="width: 3%"
      align="right"
      
      data-cissue=?task.Counter
      data-cproject=?task.C_Project
      data-detailsid=?detailsId
    }
    div 
    { 
      class="cell completeness expander"
      style="width: 10%"
      data-detailsid=?detailsId
      div
      {
        div
        {
          align="center"
          div="?task.Status"{ class="?'tag {0} inline'.args(getStatusStyle(task.Status))" }
          div="?task.Category_Name"{ class="tag category-tag inline" }
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
          div="?task.Priority"{ class="?'tag {0} inline-block'.args(getPriorityStyle(task.Priority))"}
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
      class="cell text-align-center expander"
      data-detailsid=?detailsId
      style="width: 12%;"
      div="?buildDate(task)"{ }
      div="?buildDueDate(task)"{ }
    }

    div { id="?'assignee'+task.Counter" class="cell expander" style="width: 10%" data-detailsid=?detailsId}
    div { id="?'ac'+task.Counter" class="cell expander" style="width: 15%" data-detailsid=?detailsId}
    div="?task.ProjectName"{ class="cell expander" style="width: 10%" data-detailsid=?detailsId}
    div="?task.Name"{ class="cell expander" style="width: 20%" data-detailsid=?detailsId}
    div="?task.Description"
    { 
      id="?'description'+task.Counter"
      class="cell expander" 
      style="width: 20%" 
      data-detailsid=?detailsId
    }
  }
  ***/
}

function createRowDetails(root, id) {
  /***
  div
  {
    id="?'details-'+id"
    class="cell full"
    div { id="?'tabs-'+id" class="tab-control"}
  }
  ***/
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

    /***
    div
    {
      "?if(pmperm)" {
        a="Edit Issue" 
        {
          class="button"
          style="margin: 4px 4px 4px 0px"
          data-cissue=?task.Counter
          data-cproject=?task.C_Project
          data-detailsid=?detailsId
          on-click=editIssue1
        }
        a="Resume" 
        {
          style="margin: 4px 4px 4px 0px"
          data-nextstate=?resumeStatus
          data-cproject=?task.C_Project
          data-counter=?task.Counter 
          on-click="getOtherForm1"  
          class="button"
        }
        a="Cancel" 
        {
          style="margin: 4px 4px 4px 0px"
          data-nextstate="X"
          data-cproject=?task.C_Project
          data-counter=?task.Counter 
          on-click="getOtherForm1"  
          class="button"
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
  } else {
    /***
    div
    {
      "?if(pmperm)" {
      
        a="Edit Issue" 
        {
          class="button"
          style="margin: 4px 4px 4px 0px"
          data-cissue=?task.Counter
          data-cproject=?task.C_Project
          data-detailsid=?detailsId
          on-click=editIssue1
        }
        
        "? for(var s=0, sl=task.NextState.length; s < sl; s++)" {
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
  "?if(1==1)"
  {
    div="ID" {class="cell detailsHead" style="width: 5%"}
    div="Progress" {class="cell detailsHead" style="width: 5%"}
    div="Status" {class="cell detailsHead" style="width: 10%"}
    div="Start" {class="cell detailsHead" style="width: 10%"}
    div="Plan/Due" {class="cell detailsHead" style="width: 10%"}
    div="Complete" {class="cell detailsHead" style="width: 10%"}
    div="Assigned" {class="cell detailsHead" style="width: 30%"}
    div="Description"{ class="cell detailsHead"  style="width: 20%"}
  }
  ***/
}

﻿function createAssignmentHeader(root) {
﻿  /***
   "?if(1==1)"
   {
     div="ID" {class="cell detailsHead" style="width: 5%"}
     div="User" {class="cell detailsHead" style="width: 10%"}
     div="Assigned" {class="cell detailsHead" style="width: 15%"}
     div="Operator" {class="cell detailsHead" style="width: 10%"}
     div="Unassigned" {class="cell detailsHead" style="width: 15%"}
     div="Operator" {class="cell detailsHead" style="width: 10%"}
     div="Note" {class="cell detailsHead" style="width: 35%"}
   }
   ***/
﻿}
function createStatusGridRow(root, details) {
  /***
  "?if(1==1)"
  {      
    div="?details.Counter"{ class="cell text-align-center detailsCell" align="right" style="width: 5%"}
    div="?details.Completeness +'%'" { class="cell text-align-center detailsCell" style="width: 5%"}
    div 
    { 
      class="cell text-align-center detailsCell"
      style="width: 10%"
      div
      {
        align="center"
        div="?details.Status"{ class="?'tag {0} inline'.args(getStatusStyle(details.Status))" }
        div="?details.Category_Name"{ class="tag category-tag inline" }
      }
    }
    div="?WAVE.dateTimeToString(details.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="cell text-align-center detailsCell" style="width: 10%"}
    div="?WAVE.dateTimeToString(details.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="cell text-align-center detailsCell" style="width: 10%"}
    div="?WAVE.dateTimeToString(details.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="cell text-align-center detailsCell" style="width: 10%"}
    div=?details.Assignee { class="cell text-align-center detailsCell" style="width: 30%"}
    div="?details.Description"{ id="?'details-description'+details.Counter" class="cell text-align-center detailsCell" style="width: 20%"}
  }
  ***/
}

function createAssignmentGridRow(root, assignment) {
  /***
  "?if(1==1)"
  {
    div="?assignment.Counter"{ class="cell text-align-center detailsCell" align="right" style="width: 5%"}
    div="?assignment.UserFirstName + ' ' + assignment.UserLastName + '(' +assignment.UserLogin+')'"{ class="cell text-align-center detailsCell" align="right" style="width: 10%"}
    div="?WAVE.dateTimeToString(assignment.Open_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="cell text-align-center detailsCell" style="width: 15%"}
    div="?assignment.OperatorOpenLogin" { class="cell text-align-center detailsCell" style="width: 10%"}
    div="?WAVE.dateTimeToString(assignment.Close_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="cell text-align-center detailsCell" style="width: 15%"}
    div="?assignment.OperatorCloseLogin"{ class="cell text-align-center detailsCell" align="center" style="width: 10%"}
    div="?assignment.Note" {class="cell detailsCell" style="width: 35%"}
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
    
    div="?item.Name +'('+item.Login+') :' + WAVE.dateTimeToString(item.Note_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE_TIME)" 
    {
      id="?'chathedaeritem'+item.Counter"
      class="fView ChatItemUser" 
    }
    div="?item.Note" { id="?'chat-note'+item.Counter" class="fView ChatItemNote" }
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

function buildAssignee(root, assignee) {
﻿   /***
    div="?assignee.UserLogin" { class="tag inline-block" style="background-color: brown"} 
   ***/
﻿}

﻿function createEditChatButton(root, item, task) {
﻿  if(item.HasEdit) {
﻿    /***
      
      a="edit"
      {
        class="button"
        
        data-chatid=?item.Counter
        data-note=?item.Note
        data-cproject=?task.C_Project
        data-cissue=?task.Counter
        
        on-click=editChatItem
      }
      
      ***/
﻿  }    
﻿}

function buildEditChatDialog(root, item) {
  /***
   div
   {
     data-wv-rid="V22"

     div { data-wv-fname="Note" class="fView" data-wv-ctl="textarea"}
   }
   ***/    
}

function buildAreasAndComponents(root, task) {
﻿  for (var i=0, l=task.Areas.length; i < l; i++) {
﻿    buildAreaTag(root, task.Counter, task.Areas[i].Counter, task.Areas[i].Name);
﻿  }
﻿  for (var i=0, l=task.Components.length; i < l; i++) {
﻿    buildCompTag(root, task.Counter, task.Components[i].Counter, task.Components[i].Name);
﻿  }
﻿}

function buildAssigneeList(root, task) {
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
    var item = rec.Rows[i];
    createChatItem(id, item);
    document.getElementById('chat-note'+item.Counter).innerHTML = WAVE.markup(item.Note);
    createEditChatButton('chathedaeritem'+item.Counter, item, task);
  }
/*
  for (var i = 0, l = rec.Rows.length; i < l; i++) {
    var item = rec.Rows[i];
  }
*/

}

﻿function createGrid(root, gridId) {
  /***
  div
  {
    id=?gridId
    class="table"
  }
  ***/
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

function buildAreasTab(areasId,  task) {
  var link = "/project/{0}/issuearea?issue={1}".args(task.C_Project, task.Counter);
  $.post(link,
    null,
    function(grid) {
      $("#"+areasId).html(grid);
    }).
    fail(function (error) {
      console.log(error);  
    });  
}

function ﻿buildComponentsTab(componentsId, task) {
  var link = "/project/{0}/issuecomponent?issue={1}".args(task.C_Project, task.Counter);
  $.post(link,
      null,
      function(grid) {
        $("#"+componentsId).html(grid);
      }).
    fail(function (error) {
      console.log(error);  
    });  
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
  tabs.eventBind(WAVE.GUI.EVT_TABS_TAB_CHANGED, function (sender, args) {
    if (args == "tChat") {
      refreshChat(task);
    };
  });

  buildStatusTab(statusId, task);
  buildAssignmentTab(assignmentId, task);
  buildChatTab(chatId, task);
  buildAreasTab(areasId,  task);
  buildComponentsTab(componentsId, task)

}