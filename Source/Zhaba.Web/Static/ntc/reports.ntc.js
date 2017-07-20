/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Reports = (function () {
  var published = {}
  ;

  function viewReport(data) {
    debugger;
    
    var asOf = WAVE.dateTimeToString(data.AsOf, WAVE.DATE_TIME_FORMATS.SHORT_DATE);
    var cProject = data.C_Project;
    
    var link = ZHB.URIS.ForREPORTS_DUEITEMS_VIEW(asOf, cProject);
    var strWindowFeatures = "location=yes,scrollbars=yes,status=yes";

    var win = window.open(link, "_blank", strWindowFeatures);
    
/*    WAVE.ajaxCall(
      'GET',
      link, 
      data,
      function (resp) {
        var win = window.open(link, "_blank", strWindowFeatures);
        win.document.write(resp)
      },
      ZHB.errorLog,
      ZHB.errorLog,
      WAVE.TUNDEFINED,
      WAVE.CONTENT_TYPE_JSON_UTF8
    );*/

  }

  published.DueItemsReportForm = function (e) {
    var link = ZHB.URIS.ForREPORTS_DUEITEMS();
    WAVE.ajaxCall(
      'GET',
      link,
      null,
      function (resp) {
        var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
        var dlg = new WAVE.GUI.Dialog({
          header: "Due Items Report",
          body: ZHB.Reports.Render.DueItemsReportForm(),
          footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
          onShow: function () {
            var rv = new WAVE.RecordModel.RecordView("V_DueItemsReport", _rec);
          },
          onClose: function (dlg, result) {
            if (result === WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
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