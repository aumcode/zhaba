/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Components = (function () {
    "use strict";
    var published = {};

    published.buildComponentsTab = function (root, task) {
        var link = ZHB.URIS.ForPROJECT_ISSUE_COMPONENT(task.C_Project, task.Counter);

        WAVE.ajaxCall(
            'POST',
            link,
            null,
            function(resp) {
                var _rec = JSON.parse(resp);
                ZHB.Tasks.Components.Render.buildGrid(root, _rec.Rows);
            },
            function (resp) { console.log("error"); },
            function (resp) { console.log("fail"); },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    };

    return published;
})();