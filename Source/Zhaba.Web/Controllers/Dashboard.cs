using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using NFX.Wave;
using NFX.Wave.MVC;

using Zhaba.Data.Filters;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages.Dashboard;
using ProjectsPage = Zhaba.Web.Pages.Dashboard.ProjectsPage;

namespace Zhaba.Web.Controllers
{
  [SiteUserPermission]
  public class Dashboard : ZhabaDataSetupController
  {
    [Action]
    public object Index()
    {
      var filter = new TaskListFilter();
      if (!new PMPermission().Check()) filter.C_USER = ZhabaUser.DataRow.Counter;

      return MakePage<TasksPage>(FormJSON(filter));
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

    [Action("projects", 0, "match { methods=GET accept-json=true}")]
    public object projects_GET()
    {
      var qry = QProject.AllProjects<ProjectRow>();
      var projects = ZApp.Data.CRUD.LoadEnumerable(qry);
      return new JSONResult(projects, JSONWritingOptions.CompactRowsAsMap);
    }

    [Action("tasks", 0, "match { methods=POST accept-json=true}")]
    public object tasks_POST(TaskListFilter filter)
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

    [Action("changestatus", 0, "match { methods=POST accept-json=true}")]
    public void changeStatus_POST(ulong C_Project, ulong C_Issue, string status, string note, ulong? C_User)
    {
      ZApp.Data.Issue.ChangeStatus(ZhabaUser.DataRow.Counter, C_Project, C_Issue, status, note);
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

  }
}
