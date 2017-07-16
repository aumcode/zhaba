/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Components = (function () {
    "use strict";
    var published = {};

    published.buildComponentsTab = function (componentsId, task) {
        var link = "/project/{0}/issuecomponent?issue={1}".args(task.C_Project, task.Counter);
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