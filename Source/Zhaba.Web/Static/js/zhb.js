"use strict";

var ZHB = (function () {

  var published = {};

  published.ControlScripts = {};//ns for dynamic control scripts

  function project_setup(pid) { return "/project/{0}".args(pid); }

  function common_setup() { return "/common"; }

  published.URIS = {
    ForCOMPONENT: function (id) {
      return common_setup() + "/component?id={0}".args(id);
    },

    ForAREA: function (id) {
      return common_setup() + "/area?id={0}".args(id);
    },

    ForPROJECT: function (id) {
      return common_setup() + "/project?id={0}".args(id);
    },

    ForCATEGORY: function (id) {
      return common_setup() + "/category?id={0}".args(id);
    },

    ForPROJECT_MILESTONE: function (pid, id) {
      return project_setup(pid) + "/milestone?id={0}".args(id);
    },

    ForPROJECT_COMPONENT: function (pid, id) {
      return project_setup(pid) + "/component?counter={0}".args(id);
    },

    ForPROJECT_AREA: function (pid, id) {
      return project_setup(pid) + "/area?counter={0}".args(id);
    },

    ForPROJECT_ISSUE: function (pid, id) {
      return project_setup(pid) + "/issue?id={0}".args(id);
    },

    ForPROJECT_DELETE_COMPONENT: function (project, counter) {
      return project_setup(project) + "/component?counter={0}".args(counter);
    },

    ForPROJECT_DELETE_AREA: function (project, counter) {
      return project_setup(project) + "/area?counter={0}".args(counter);
    },

    ForPROJECT_ISSUE_AREA: function (project, issue) {
      return project_setup(project) + "/issuearea?issue={0}".args(issue);
    },

    ForPROJECT_ISSUE_COMPONENT: function (project, issue) {
      return project_setup(project) + "/issuecomponent?issue={0}".args(issue);
    },

    ForPROJECT_LINK_ISSUE_AREA: function (project, issue, area) {
      return project_setup(project) + "/linkissuearea?issue={0}&area={1}".args(issue, area);
    },

    ForPROJECT_LINK_ISSUE_COMPONENT: function (project, issue, component) {
      return project_setup(project) + "/linkissuecomponent?issue={0}&component={1}".args(issue, component);
    },

    ForUSER: function (id) {
      return common_setup() + "/user?id={0}".args(id);
    },

    ForDELETE_CATEGORY: function (id) {
        return common_setup() + "/category?id={0}".args(id);
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