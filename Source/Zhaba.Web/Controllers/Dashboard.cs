using System;
using System.Collections.Generic;

using NFX;
using NFX.Wave;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages.Dashboard;

namespace Zhaba.Web.Controllers
{
  [SiteUserPermission]
  public class Dashboard : ZhabaController
  {
    [Action]
    public object Index()
    {
      return new DashboardPage();
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
  }
}
