/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $, ZHB */

ZHB.Tasks.Status = (function () {
    "use strict";
    var published = {},
        fStatuses = {N: 'New', R: 'Reopen', A: 'Assign', D: 'Done', F: 'Defer', C: 'Close', X: 'Cancel'}
    ;

    function buildIsueDialog(rec, pid, iid) {
        var dlg = new WAVE.GUI.Dialog({
            header: 'Issue',
            body: ZHB.Tasks.Render.buildIssueBody(),
            footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
            onShow: function () {
                var rv = new WAVE.RecordModel.RecordView("V6", rec);
            },
            onClose: function (dlg, result) {
                if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                rec.validate();
                if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
                var link = '/project/{0}/issue?id={1}'.args(pid, iid);
                WAVE.ajaxCall(
                    'POST',
                    link,
                    rec.data(),
                    function (resp) {
                        var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                        if (_rec.validationError()) {
                            WAVE.GUI.toast(_rec.validationError(), 'error');
                        } else {

                            ZHB.Tasks.scheduleFetch();
                            WAVE.GUI.currentDialog().close();
                        }
                    },
                    ZHB.errorLog,
                    ZHB.errorLog,
                    WAVE.CONTENT_TYPE_JSON_UTF8,
                    WAVE.CONTENT_TYPE_JSON_UTF8
                );
                return WAVE.GUI.DLG_UNDEFINED;
            }
        });
    }

    function editIssue(pid, iid) {
        var link = ZHB.URIS.ForPROJECT_ISSUE(pid, iid);//  '/project/{0}/issue?id={1}'.args(pid, iid);
        WAVE.ajaxCall(
            'GET',
            link,
            null,
            function (resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                buildIsueDialog(_rec, pid, iid);
            },
            ZHB.errorLog,
            ZHB.errorLog,
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function changeStatus(pid, iid, status, note) {
        var data = {
            C_Project: pid,
            C_Issue: iid,
            status: status,
            note: note
        }
        WAVE.ajaxCall(
            'POST',
            ZHB.URIS.ForDASHBOARD_CHANGESTATUS(),
            data,
            function (resp) {
                ZHB.Tasks.scheduleFetch();

            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function changeAssignStatus(pid, iid, status, data) {
        var link = ZHB.URIS.ForISSUE_ISSUEASSIGN(pid, iid, null);
        WAVE.ajaxCall(
            'POST',
            link,
            data,
            function (resp) {
                ZHB.Tasks.scheduleFetch();

            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function buildIssueAssignPopUpDialog(rec, pid, iid) {
        var dlg = new WAVE.GUI.Dialog({
            header: 'Change state to ' + fStatuses[status],
            body: ZHB.Tasks.Status.Render.buildIssueAssigneeStatusBody(),
            footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
            onShow: function () {
                var rv = new WAVE.RecordModel.RecordView("V2", rec);
            },
            onClose: function (dlg, result) {
                if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                rec.validate();
                if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED
                changeAssignStatus(pid, iid, status, rec.data());
                return WAVE.GUI.DLG_CANCEL;
            }
        });
    }

    function buildPopUpDialog(status, rec, pid, iid) {
        var dlg = new WAVE.GUI.Dialog({
            header: 'Change state to ' + fStatuses[status],
            body: ZHB.Tasks.Status.Render.buildStatusBody(),
            footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
            onShow: function () {
                var rv = new WAVE.RecordModel.RecordView("V2", rec);
            },
            onClose: function (dlg, result) {
                if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                rec.validate();
                if (!rec.valid()) return WAVE.GUI.DLG_UNDEFINED;
                var note = rec.data().Description;
                changeStatus(pid, iid, status, note);
                return WAVE.GUI.DLG_CANCEL;
            }
        });
    }

    function getIssueAssignForm(pid, iid, iaid) {
        var link = ZHB.URIS.ForISSUE_ISSUEASSIGN(pid, iid, iaid);
        WAVE.ajaxCall(
            'GET',
            link,
            null,
            function (resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                buildIssueAssignPopUpDialog(_rec, pid, iid);
            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    }

    function getOtherForm(status, pid, iid) {
        var link = ZHB.URIS.ForISSUE_STATUSNOTE(pid, iid, status);
        WAVE.ajaxCall(
            'GET',
            link,
            null,
            function (resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                buildPopUpDialog(status, _rec, pid, iid);
            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );

    }

    function changeStatusDialog(status, pid, iid) {
        if (status == 'A') {
            getIssueAssignForm(pid, iid, "");
        } else {
            getOtherForm(status, pid, iid);
        }
    }

    published.addIssue = function (pid) {
        editIssue(pid, "");
    };

    published.editIssue1 = function (e) {
        e.stopPropagation();
        editIssue(e.target.dataset.cproject, e.target.dataset.cissue);
    };

    published.buildStatusTab = function (root, task) {
        var gridID = "status-grid-" + task.Counter;
        ZHB.Tasks.Status.Render.buildStatusButtons(root, task);
        ZHB.Tasks.Render.createGrid(root, gridID);
        ZHB.Tasks.Status.Render.createStatusHeader(gridID);
        for (var j = 0, l = task.Details.length; j < l; j++) {
            var d = task.Details[j];
            ZHB.Tasks.Status.Render.createStatusGridRow(gridID, d);
            document.getElementById('details-description' + d.Counter).innerHTML = WAVE.markup(d.Description != null ? d.Description : '');
        }
    };

    published.changeStatusDialog1 = function (e) {
        changeStatusDialog(e.target.dataset.nextstate, e.target.dataset.cproject, e.target.dataset.counter)
    };


    published.getOtherForm1 = function (e) {
        var status = e.target.dataset.nextstate;
        var pid = e.target.dataset.cproject;
        var iid = e.target.dataset.counter;
        getOtherForm(status, pid, iid);
    };

    published.editAssignee = function (e) {
        var pid = e.target.dataset.cproject;
        var iid = e.target.dataset.cissue;
        var id = e.target.dataset.cassignee;
        var link = "/project/{0}/issue/{1}/issueassign?id={2}".args(pid, iid, id);
        WAVE.ajaxCall(
            'GET',
            link,
            null,
            function (resp) {
                var _rec = new WAVE.RecordModel.Record(JSON.parse(resp));
                _rec.fieldByName('Open_TS').readonly(true);
                _rec.fieldByName('C_User').readonly(true);
                var dlg = WAVE.GUI.Dialog({
                    header: "Unassignee for Issue [{0}]".args(iid),
                    body: ZHB.Tasks.Status.Render.buildIssueAssigneeStatusBody(),
                    footer: ZHB.Tasks.Status.Render.buildStatusFooter(),
                    onShow: function () {
                        var rv = new WAVE.RecordModel.RecordView("V2", _rec);
                    },
                    onClose: function (dlg, result) {
                        if (result == WAVE.GUI.DLG_CANCEL) return WAVE.GUI.DLG_CANCEL;
                        _rec.validate();
                        if (!_rec.valid()) return WAVE.GUI.DLG_UNDEFINED
                        WAVE.ajaxCall(
                            'POST',
                            link,
                            _rec.data(),
                            function (resp) {
                                ZHB.Tasks.scheduleFetch();
                            },
                            function (resp) {
                                console.log("error");
                            },
                            function (resp) {
                                console.log("fail");
                            },
                            WAVE.CONTENT_TYPE_JSON_UTF8,
                            WAVE.CONTENT_TYPE_JSON_UTF8
                        );
                        return WAVE.GUI.DLG_CANCEL;
                    }

                });
            },
            function (resp) {
                console.log("error");
            },
            function (resp) {
                console.log("fail");
            },
            WAVE.CONTENT_TYPE_JSON_UTF8,
            WAVE.CONTENT_TYPE_JSON_UTF8
        );
    };
    
    published.getStatusButtonName = function (status) {
        return fStatuses[status];
    };

    published.init = function (init) {
        ZHB.Tasks.Status.Render.init({});
    };

    return published;
})();