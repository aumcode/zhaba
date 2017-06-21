"use strict";

var ZHB = (function () {

  var published = {};

  published.ControlScripts = {};//ns for dynamic control scripts

  function project_setup(pid) { return "/project/" + pid; }

  function common_setup() { return "/common"; }

  published.URIS = {
    ForCOMPONENT : function(id) {
      return common_setup() + "/component?id={0}".args(id);
    },

    ForAREA : function (id) {
      return common_setup() + "/area?id={0}".args(id);
    },

    ForPROJECT : function (id) {
      return common_setup() + "/project?id={0}".args(id);
    },

    ForPROJECT_MILESTONE: function (pid, id) {
      return project_setup(pid) + "/milestone?id=" + id;
    },

    ForPROJECT_COMPONENT: function (pid, id) {
      return project_setup(pid) + "/component?counter=" + id;
    },

    ForPROJECT_AREA: function (pid, id) {
      return project_setup(pid) + "/area?counter=" + id;
    },

    ForPROJECT_ISSUE: function (pid, id) {
      return project_setup(pid) + "/issue?id=" + id;
    },

    ForPROJECT_ISSUE_AREA: function(project, issue) {
      return project_setup(project) + "/issuearea?issue=" + issue;
    },

    ForUSER: function (id) {
        return common_setup() + "/user?id={0}".args(id);
    }
  };

  if (!String.prototype.args) {
    String.prototype.args = function () {
      var argts = arguments;
      return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof argts[number] != 'undefined'
          ? argts[number]
          : match
        ;
      });
    };
  }

  return published;
}());