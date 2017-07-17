/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Components = (function () {
    "use strict";
    var published = {};

    published.buildComponentsTab = function (componentsId, task) {
        var link = ZHB.URIS.ForPROJECT_ISSUE_COMPONENT(task.C_Project, task.Counter);
        $.post(link,
            null,
            function (grid) {
                $("#" + componentsId).html(grid);
            }).fail(function (error) {
            console.log(error);
        });
    };

    return published;
})();