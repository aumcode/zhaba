/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Chat.Render = (function () {
    "use strict";
    var published = {}
    ;
    
    published.buildChatFilterForm = function(root, task) {
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
              on-click=ZHB.Tasks.Chat.setChatFilter
              class="button"
            }
          }
        }
        ***/
    };
    
    published.buildChatForm = function(root, task) {
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
        
              on-click=ZHB.Tasks.Chat.sendChatMessage1
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
    };
    
    published.buildChatMessage = function(root, task) {
        /***
        div
        {
          class="ChatDiv"
          id="?'chatMessage-'+task.Counter"
        }
        ***/
    };
    
    published.createChatItem = function(root, item) {
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
    };
    
    published.createEditChatButton = function(root, item, task) {
        if (item.HasEdit) {
          /***
        
            a="edit"
            {
              class="button"
        
              data-chatid=?item.Counter
              data-note=?item.Note
              data-cproject=?task.C_Project
              data-cissue=?task.Counter
        
              on-click=ZHB.Tasks.Chat.editChatItem
            }
        
            ***/
        }
    };
    
    published.init = function (init) {
            
    };
    
    return published;
})();