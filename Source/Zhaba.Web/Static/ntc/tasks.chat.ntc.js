/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Chat = (function () {
  "use strict";
  var published = {},
    fScheduleTimer,
    fTasks,
    fChatRec = {},
    fChatFilterRec = {},
    fTick = 5000,
    fTickDefault = 5000,
    fTickDelta = 10000;

  function schedulerChat() {
    if (fScheduleTimer) clearTimeout(fScheduleTimer);
    WAVE.each(fTasks, function (task) {
      ZHB.Tasks.Chat.refreshChat(task);
    });
    fScheduleTimer = setTimeout(schedulerChat, fTick);
  }

  function createChatItems(task, rec) {
    var id = 'chatMessage-' + task.Counter;
    if (document.getElementById(id) !== undefined) {
      document.getElementById(id).innerHTML = "";
      for (var i = 0, l = rec.Rows.length; i < l; i++) {
        var item = rec.Rows[i];
        ZHB.Tasks.Chat.Render.createChatItem(id, item);
        document.getElementById('chat-note' + item.Counter).innerHTML = WAVE.markup(item.Note);
        ZHB.Tasks.Chat.Render.createEditChatButton('chathedaeritem' + item.Counter, item, task);
      }
    }
  }

  function chatForm(task) {
    var link = ZHB.URIS.ForPROJECT_ISSUE_CHAT(task.C_Project, task.Counter, "");
    WAVE.ajaxCall(
      'GET',
      link,
      null,
      function (resp) {
        fChatRec[task.Counter] = new WAVE.RecordModel.Record(JSON.parse(resp));
        new WAVE.RecordModel.RecordView('chatForm' + task.Counter, fChatRec[task.Counter]);
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  function sendChatMessage(pid, iid, cid, _rec) {
    var task = {Counter: iid, C_Project: pid, createImageData: cid};
    var link = ZHB.URIS.ForPROJECT_ISSUE_CHAT(pid, iid, cid);
    WAVE.ajaxCall(
      'POST',
      link,
      _rec.data(),
      function (resp) {
        chatForm(task);
        ZHB.Tasks.Chat.refreshChat(task);
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  function chatFilterForm(task) {
    var link = ZHB.URIS.ForPROJECT_ISSUE_CHATLIST(task.C_Project, task.Counter);
    WAVE.ajaxCall(
      'GET',
      link,
      null,
      function (resp) {
        // debugger;
        fChatFilterRec[task.Counter] = new WAVE.RecordModel.Record(JSON.parse(resp));
        new WAVE.RecordModel.RecordView('ChatFilterForm' + task.Counter, fChatFilterRec[task.Counter]);

      },
      function (resp) {
        console.log("error");
      },
      function (resp) {
        console.log("fail");
      },
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  }

  published.setChatFilter = function (e) {
    e.stopPropagation();
    var iid = e.target.dataset.cissue;
    var pid = e.target.dataset.cproject;
    var task = {Counter: iid, C_Project: pid};
    ZHB.Tasks.Chat.refreshChat(task);
  }

  published.editChatItem = function (e) {
    e.stopPropagation();
    var chatId = e.target.dataset.chatid;
    var note = e.target.dataset.node;
    var iid = e.target.dataset.cissue;
    var pid = e.target.dataset.cproject;

    var link = ZHB.URIS.ForPROJECT_ISSUE_CHAT(pid, iid, chatId);

    WAVE.ajaxCall(
      'GET',
      link,
      null,
      function (resp) {
        var rec = new WAVE.RecordModel.Record(JSON.parse(resp));
        var dlg = WAVE.GUI.Dialog({
          header: " Edit note",
          body: ZHB.Tasks.Chat.Render.buildEditChatDialog(null, chatId),
          footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
          onShow: function () {
            var rv = new WAVE.RecordModel.RecordView("V22", rec);
          },
          onClose: function (dlg, result) {
            if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
            rec.validate();
            if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
            sendChatMessage(pid, iid, chatId, rec);
            return WAVE.GUI.DLG_CANCEL;
          }
        });
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.CONTENT_TYPE_JSON_UTF8,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );
  };

  published.sendChatMessage1 = function (e) {
    var iid = e.target.dataset.cissue;
    var pid = e.target.dataset.cproject;
    sendChatMessage(pid, iid, "", fChatRec[iid])
  };

  published.refreshChat = function (task) {
    if (fChatFilterRec[task.Counter]) {
      var link = ZHB.URIS.ForPROJECT_ISSUE_CHATLIST(task.C_Project, task.Counter);
      var data = fChatFilterRec[task.Counter].data();
      WAVE.ajaxCall(
        'POST',
        link,
        data,
        function (resp) {
          var rec = JSON.parse(resp);
          fTick = fTickDefault;
          createChatItems(task, rec);
        },
        function (resp) {
          ZHB.errorLog(resp);
          fTick += fTickDelta
        },
        function (resp) {
          ZHB.errorLog(resp);
          fTick += fTickDelta
        },
        WAVE.CONTENT_TYPE_JSON_UTF8,
        WAVE.CONTENT_TYPE_JSON_UTF8
      );
    }
  };

  published.buildChatTab = function (root, task) {
    ZHB.Tasks.Chat.Render.buildChatFilterForm(root, task);
    ZHB.Tasks.Chat.Render.buildChatForm(root, task);
    ZHB.Tasks.Chat.Render.buildChatMessage(root, task);
    chatForm(task);
    chatFilterForm(task);
  };

  published.init = function (init) {
    ZHB.Tasks.Chat.Render.init({});
    fTasks = init.tasks;
    schedulerChat();
  };

  return published;
})();