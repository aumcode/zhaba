﻿/*jshint devel: true,browser: true, sub: true */
/*global WAVE, $ */


var ZHB = (function() {
    "use strict";

    var published = {};

    published.ControlScripts = {}; //ns for dynamic control scripts

    function project_setup(pid) { return "/project/{0}".args(pid); }

    function issue_setup(pid, iid) { return project_setup(pid) + "/issue/{0}".args(iid); }

    function common_setup() { return "/common"; }

    published.URIS = {
        ForCOMPONENT: function(id) {
            return common_setup() + "/component?id={0}".args(id);
        },

        ForAREA: function(id) {
            return common_setup() + "/area?id={0}".args(id);
        },

        ForPROJECT_SELECT: function(pid) {
            return project_setup(pid) + "/select";
        },

        ForPROJECT: function(id) {
            return common_setup() + "/project?id={0}".args(id);
        },

        ForCATEGORY: function(id) {
            return common_setup() + "/category?id={0}".args(id);
        },

        ForPROJECT_MILESTONE: function(pid, id) {
            return project_setup(pid) + "/milestone?id={0}".args(id);
        },

        ForPROJECT_COMPONENT: function(pid, id) {
            return project_setup(pid) + "/component?counter={0}".args(id);
        },

        ForPROJECT_AREA: function(pid, id) {
            return project_setup(pid) + "/area?counter={0}".args(id);
        },

        ForPROJECT_ISSUE: function(pid, id) {
            return project_setup(pid) + "/issue?id={0}".args(id);
        },

        ForPROJECT_DELETE_COMPONENT: function(project, counter) {
            return project_setup(project) + "/component?counter={0}".args(counter);
        },

        ForPROJECT_DELETE_AREA: function(project, counter) {
            return project_setup(project) + "/area?counter={0}".args(counter);
        },

        ForPROJECT_ISSUE_AREA: function(project, issue) {
            return project_setup(project) + "/issuearea?issue={0}".args(issue);
        },

        ForPROJECT_ISSUE_COMPONENT: function(project, issue) {
            return project_setup(project) + "/issuecomponent?issue={0}".args(issue);
        },

        ForPROJECT_LINK_ISSUE_AREA: function(project, issue, area) {
            return project_setup(project) + "/linkissuearea?issue={0}&area={1}".args(issue, area);
        },

        ForPROJECT_LINK_ISSUE_COMPONENT: function(project, issue, component) {
            return project_setup(project) + "/linkissuecomponent?issue={0}&component={1}".args(issue, component);
        },

        ForUSER: function(id) {
            return common_setup() + "/user?id={0}".args(id);
        },

        ForDELETE_CATEGORY: function(id) {
            return common_setup() + "/category?id={0}".args(id);
        },

        ForISSUE_ISSUEASSIGN: function(pid, iid, id) {
            return issue_setup(pid, iid) + "/issueassign?id={0}".args(id);
        },

        ForISSUE_ISSUECANCEL: function (pid, iid, id) {
          return issue_setup(pid, iid) + "/issuecancel?id={0}".args(id);
        },

        ForISSUE_STATUSNOTE: function(pid, iid, status) {
            return issue_setup(pid, iid) + "/statusnote?status={0}".args(status);
        },

        ForDELETE_ISSUE: function(pid, iid) {
            return issue_setup(pid, iid) + "/close";
        },

        ForREOPEN_ISSUE: function(pid, iid) {
            return issue_setup(pid, iid) + "/reopen";
        },

        ForDEFER_ISSUE: function(pid, iid) {
            return issue_setup(pid, iid) + "/defer";
        },

        ForDASHBOARD_TASKS: function() {
            return "/dashboard/tasks";
        },
        
        ForPROJECT_ISSUE_CHATLIST: function(pid, iid) {
            return issue_setup(pid, iid)+"/chatlist";
        },
        ForPROJECT_ISSUE_CHAT: function(pid, iid, cid) {
            return issue_setup(pid, iid)+"/chat?id={0}".args(cid);
        },
        ForDASHBOARD_CHANGESTATUS: function() {
            return "/dashboard/changestatus";
        },
        ForPROJECT_ISSUE_REPORT : function (pid, iid, report) {
            return issue_setup(pid, iid) +"/{0}".args(report);
        },
        ForREPORTS : function(report) {
            return "/reports/{0}".args(report);    
        },
        ForREPORTS_DUEITEMS : function () {
            return ZHB.URIS.ForREPORTS("dueitems");
        },
        ForREPORTS_DUEITEMS_VIEW : function (asOf, cProject) {
            return ZHB.URIS.ForREPORTS("dueitemsview")+"?AsOf={0}&C_Project={1}".args(asOf,  cProject);
        }
        
    };

    if (!String.prototype.args) {
        String.prototype.args = function() {
            var argts = arguments;
            return this.replace(/{(\d+)}/g, function(match, number) {
                return typeof argts[number] != 'undefined' ?
                    argts[number] :
                    match;
            });
        };
    }

    published.errorLog = function(response) {
        WAVE.GUI.toast("Something went wrong, see console for details");
        console.log(response);
    };
    
    published.debugLog = function(response) {
      //todo setup for debug only!
        console.log(response);
    };


    return published;
}());

/***@ etc.ntc.js ***/
/***@ reports.ntc.js ***/
/***@ reports.render.ntc.js ***/
/***@ tasks.ntc.js ***/
/***@ tasks.render.ntc.js ***/
/***@ tasks.status.ntc.js ***/
/***@ tasks.status.render.ntc.js ***/
/***@ tasks.assignment.ntc.js ***/
/***@ tasks.assignment.render.ntc.js ***/
/***@ tasks.chat.ntc.js ***/
/***@ tasks.chat.render.ntc.js ***/
/***@ tasks.areas.ntc.js ***/
/***@ tasks.areas.render.ntc.js ***/
/***@ tasks.components.ntc.js ***/
/***@ tasks.components.render.ntc.js ***/
/***@ tasks.report.ntc.js ***/