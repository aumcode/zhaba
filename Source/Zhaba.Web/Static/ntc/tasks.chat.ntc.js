/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Chat = (function() {
    "use strict";
    var published = {},
        fScheduleTimer,
        fTasks
    
    ;
    
    function schedulerTask() {
        if (fScheduleTimer) clearTimeout(fScheduleTimer);
        WAVE.each(fTasks, function (task) {
           refreshChat(task); 
        });
        fScheduleTimer = setTimeout(schedulerTask, 20000);
    }

    published.refreshChat = function(task) {
        var link = "/project/{0}/issue/{1}/chatlist".args(task.C_Project, task.Counter);
        var data = chatFilterRec[task.Counter].data();
        WAVE.ajaxCall(
            'POST',
            link,
            data,
            function(resp) {
                var rec = JSON.parse(resp);
                createChatItems(task, rec);
            },
            ZHB.errorLog,
            ZHB.errorLog,
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    };
    
    published.init = function (init) {
        fTasks = init.tasks;
        schedulerTask();    
    };

    return published;
})();