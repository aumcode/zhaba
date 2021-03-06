/**
 * Created by mad on 13.07.2017.
 */
function linkIssueArea(event, cProject, cIssue, cArea) {
  event = event || window.event;

  var chk = event.currentTarget;

  var formData = new FormData();
  formData.append('link', chk.checked);

  var areaId = 'issue-' + cIssue + '-areatag-' + cArea;
  var acId = 'ac'+cIssue;

  $.ajax({
      url: ZHB.URIS.ForPROJECT_LINK_ISSUE_AREA(cProject, cIssue, cArea),
      type: 'POST',
      dataType: "json",
      data: formData,
      processData: false,
      contentType: false,
      success : function (resp) {
          var el = WAVE.id(acId);
          if (el) ZHB.Tasks.refreashTag(cProject, cIssue);
      }
    })
    .fail(function (xhr, txt, err) {
      WAVE.GUI.toast("The link could not be updated: " + xhr.status + "<br>" + err, "error");
      fetchData();
    });
}

function linkIssueComponent(event, cProject, cIssue, cComponent) {
  event = event || window.event;

  var chk = event.currentTarget;

  var formData = new FormData();
  formData.append('link', chk.checked);

  var compId = 'issue-' + cIssue + '-comptag-' + cComponent;
  var acId = 'ac'+cIssue;

  $.ajax({
      url: ZHB.URIS.ForPROJECT_LINK_ISSUE_COMPONENT(cProject, cIssue, cComponent),
      type: 'POST',
      dataType: "json",
      data: formData,
      processData: false,
      contentType: false,
      success : function (resp) {
          var el = WAVE.id(acId);
          if (el) ZHB.Tasks.refreashTag(cProject, cIssue);
      }
    })
    .fail(function (xhr, txt, err) {
      WAVE.GUI.toast("The link could not be updated: " + xhr.status + "<br>" + err, "error");
      fetchData();
    });
}

WAVE.onReady(function() {
  ZHB.ControlScripts.TableGrid = {
    rowSelect: function(tableElm, rowElm, key, data) {
      var tasksPage = WAVE.id('tasksPage');
      if (!tasksPage) {
        tableElm.SELECTED_ROW_KEY = key;
        tableElm.SELECTED_ROW_DATA = data;
        $(tableElm).find("tr").removeClass("selectedGridTableRow");
        $(rowElm).addClass("selectedGridTableRow");
        if (tableElm.onGridRowSelection)
          tableElm.onGridRowSelection(tableElm, key, data);
      }
    }
  };
});

