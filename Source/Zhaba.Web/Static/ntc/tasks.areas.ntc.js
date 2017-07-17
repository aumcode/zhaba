/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Areas = (function () {
    "use strict";
    var published = {};

    published.buildAreasTab = function (areasId, task) {
        var link =  ZHB.URIS.ForPROJECT_ISSUE_AREA(task.C_Project, task.Counter);
        $.post(link,
            null,
            function (grid) {
                $("#" + areasId).html(grid);
            }).fail(function (error) {
            console.log(error);
        });
    };

    return published;
})();