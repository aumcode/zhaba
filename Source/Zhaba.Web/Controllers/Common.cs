using NFX;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;
using Zhaba.Data.Filters;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages;
using Zhaba.Web.Controls.Grids;
using Zhaba.Web.Pages.List;

namespace Zhaba.Web.Controllers
{
  [SiteUserPermission]
  public class Common : ZhabaDataSetupController
  {
    [Action]
    public object Index()
    {
      return new Redirect(URIS.DASHBOARD);
    }
    
    [Action]
    public object Users(UserListFilter filter)
    {
      return DataSetup_Index<UserListFilter, UserGrid, UsersPage>(filter);
    }

    [Action]
    public object User(ulong? id, UserRegistrationForm form)
    {
        return DataSetup_ItemDetails<UserRegistrationForm, UserRegistrationPage>(new object[] { id }, form, URIS.COMMON_USERS);
    }

    [Action]
    public object Projects(ProjectListFilter filter)
    {
      return DataSetup_Index<ProjectListFilter, ProjectGrid, ProjectsPage>(filter);
    }

    [Action]
    public object Project(ulong? id, ProjectForm form)
    {
      return DataSetup_ItemDetails<ProjectForm, ProjectPage>(new object[] { id }, form, URIS.COMMON_PROJECTS);
    }
  }
}
