using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using NFX.Wave;
using NFX.Wave.MVC;
using Zhaba.Data.Filters;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Security.Permissions;
using Zhaba.Web.Controls;
using Zhaba.Web.Pages.Dashboard;
using Zhaba.Web.Pages.List;
using ProjectsPage = Zhaba.Web.Pages.Dashboard.ProjectsPage;

namespace Zhaba.Web.Controllers
{
  [SiteUserPermission]
  public class Dashboard : ZhabaDataSetupController
  {
    [Action]
    public object Index()
    {
      return new TasksPage();
    }

    [Action]
    public object Projects()
    {
      var qry = QProject.AllProjects<ProjectRow>();
      var projects = ZApp.Data.CRUD.LoadEnumerable(qry);

      if (WorkContext.RequestedJSON)
        return projects;
      else
        return new ProjectsPage(projects);
    }

    [Action("tasks", 0, "match { methods=POST,GET accept-json=true}")]
    public object tasks_GET()
    {
      var filter = new TaskListFilter();
      if (!new PMPermission().Check()) filter.C_USER = ZhabaUser.DataRow.Counter;
      object tasks;
      filter.Save(out tasks);
      return new JSONResult(tasks, JSONWritingOptions.CompactRowsAsMap);
    }

    /// <summary>
    /// Create json string from object model
    /// </summary>
    /// <param name="model">model</param>
    /// <param name="validationError">error</param>
    /// <returns>JSON string</returns>
    public static string FormJSON<TModel>(TModel model, Exception validationError = null)
        where TModel : TypedRow
    {
      var generator = new NFX.Wave.Client.RecordModelGenerator();
      return generator.RowToRecordInitJSON(model, validationError).ToJSON();
    }

    [Action("taskFilter", 0, "match { methods=GET accept-json=true}")]
    public object taskFilter_GET()
    {
      var filter = new TaskListFilter();
      if (!new PMPermission().Check()) filter.C_USER = ZhabaUser.DataRow.Counter;
      return FormJSON(filter);
    }

    [Action("taskFilter", 0, "match { methods=POST accept-json=true}")]
    public object taskFilter_POST(TaskListFilter filter)
    {
      object data;
      if (!new PMPermission().Check()) filter.C_USER = ZhabaUser.DataRow.Counter;
      filter.Save(out data);
      return new JSONResult(data, JSONWritingOptions.CompactRowsAsMap);
    }

    [Action("changeProgress", 0, "match { methods=POST accept-json=true}")]
    public void changeProgress_POST(ulong issueCounter, int value, string description)
    {
      ZApp.Data.Issue.ChangeProgess(ZhabaUser.DataRow.Counter, issueCounter, value, description);
    }
  }
}
