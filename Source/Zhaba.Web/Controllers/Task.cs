using NFX.Wave.MVC;

namespace Zhaba.Web.Controllers
{
  public class Task : Controller
  {
    [Action]
    public object Issues()
    {
      //var qry = QTask.AllProjects<ProjectRow>();
      //var projects = ZApp.Data.CRUD.LoadEnumerable(qry);

      //if (WorkContext.RequestedJSON)
      //  return projects;
      //else
      //  return new ProjectsPage(projects);
      return null;
    }
  }
}
