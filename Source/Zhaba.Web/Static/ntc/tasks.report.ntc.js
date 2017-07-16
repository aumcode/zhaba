/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Report = (function () {
    var published = {}
    ;
    
    published.openReport = function(e) {
        var pid = e.target.dataset.cproject;
        var iid = e.target.dataset.cissue;
        var report = e.target.dataset.report;
        var link = ZHB.URIS.ForPROJECT_ISSUE_REPORT(pid, iid, report);
        window.open(link);
    };
    
    return published;
})();