/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Reports = (function () {
    var published = {}
    ;
    
    function viewReport(data) {
        debugger;
        var link = ZHB.URIS.ForREPORTS_DUEITEMS();
        WAVE.ajaxCall(
            'POST',
            link,
            data,
            function (resp) {
                var w = window.open('about:blank', 'windowname');
                w.document.write(resp);
            },
            ZHB.errorLog,
            ZHB.errorLog,
            WAVE.TUNDEFINED,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }
    
    published.DueItemsReportForm = function (e) {
        var link = ZHB.URIS.ForREPORTS_DUEITEMS();
        WAVE.ajaxCall(
            'GET',
            link, 
            null,
            function(resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                var dlg = new WAVE.GUI.Dialog({
                    header: "Due Items Report",
                    body:ZHB.Reports.Render.DueItemsReportForm(),
                    footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
                    onShow: function() {
                        var rv = new WAVE.RecordModel.RecordView("V_DueItemsReport", _rec);
                    },
                    onClose: function(dlg, result) {
                        if(result===WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                        _rec.validate();
                        if (!_rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
                        viewReport(_rec.data());
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
    
    return published;
})();