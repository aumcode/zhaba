﻿var chatRec = {};
var chatFilterRec = {};

function createBody(root) {
  /***
  div { id="table" class = "rTable" }
  ***/
}

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
  /***
  div 
  {
    class="rTableRow"
    div="ID" {class="rTableHead" style="width: 50px"}
    div="Status" {class="rTableHead" style="width: 100px"}
    div="Date" {class="rTableHead" style="width: 210px"}
    div="Assigned" {class="rTableHead" style="width: 100px"}
    div="Areas/Components" {class="rTableHead" style="width: 100px"}
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
      div
      {
        align="center"
        div="?task.Status"{ class="tag inline" style="?getStatusStyle(task.Status)" }
        div="?task.Category_Name"{ class="tag inline" style="background-color: gray;" }
      }
      
      class="rTableCell completeness" 
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
      div 
      { 
        class="bar" 
        style="?getStatusBarStyle(task.Completeness)" 
      }
    }
    div
    {
      class="rTableCell"
      div="?buildDate(task)"{}
      div
      {
        div="?buildDueDate(task)"{ class="inline"}
        div="?task.Priority"{ class="tag inline-block" style="?computePriorityStyle(task.Priority)"}
      }
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
            
    div="?details.Counter"{ class="rDetailsTableCell" align="right" }
    div="?details.Completeness" { class="rDetailsTableCell" }
    div="?details.Status"{ class="rDetailsTableCell" align="center"}
    div="?WAVE.dateTimeToString(details.Start_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rDetailsTableCell" }
    div="?WAVE.dateTimeToString(details.Due_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rDetailsTableCell" }
    div="?WAVE.dateTimeToString(details.Complete_Date, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rDetailsTableCell" }
    div=?details.Assignee { class="rDetailsTableCell" }
    div="?details.Description"{ class="rDetailsTableCell" }
  }
  ***/
}

function createAssignmentGridRow(root, assignment) {
  /***
  div
  {
    id="?'assignmentRow-'+assignment.Counter"
    class="rTableRow"

    div="?assignment.Counter"{ class="rDetailsTableCell" align="right" }
    div="?assignment.UserFirstName + ' ' + assignment.UserLastName + '(' +assignment.UserLogin+')'"{ class="rDetailsTableCell" align="right" }
    div="?WAVE.dateTimeToString(assignment.Open_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rDetailsTableCell" }
    div="?assignment.OperatorOpenLogin" { class="rDetailsTableCell" }
    div="?WAVE.dateTimeToString(assignment.Close_TS, WAVE.DATE_TIME_FORMATS.SHORT_DATE)"{ class="rDetailsTableCell" }
    div="?assignment.OperatorCloseLogin"{ class="rDetailsTableCell" align="center"}
    div="?assignment.Note" {class="rDetailsTableCell"}
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

function buildArea(root, taskCounter, area, canRemove) {
  /***
  div="?area.Name" { class="tag inline-block" style="background-color: darkgreen"}
  ***/
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
    /***
    div="?component.Name + ' X'" 
    { 
      id="?'comp-' + component.Counter"
      class="tag inline-block" 
      style="background-color: darkblue; cursor: pointer"
      data-ccomp=?component.Counter
      data-cissue=?task.Counter
      data-cproject=?task.C_Project
      on-click=removeComp
    } 
    ***/
  }
  else
  {
    /***
    div="?component.Name" { class="tag inline-block" style="background-color: darkblue"} 
    ***/
  }
}

function buildAssignee(root, taskCounter, assignee, canRemove) {
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