/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Areas = (function () {
    "use strict";
    var published = {};

    published.buildAreasTab = function (root, task) {
        var link =  ZHB.URIS.ForPROJECT_ISSUE_AREA(task.C_Project, task.Counter);
        
        WAVE.ajaxCall(
            'POST',
            link, 
            null,
            function(resp) {
                var _rec = JSON.parse(resp);
                ZHB.Tasks.Areas.Render.buildGrid(root, _rec.Rows);
            },
            function (resp) { console.log("error"); },
            function (resp) { console.log("fail"); },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );

    };

    return published;
})();