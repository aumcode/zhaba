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
    [UserManagerPermission]
    public object Users(UserListFilter filter)
    {
      return DataSetup_Index<UserListFilter, UserGrid, UsersPage>(filter);
    }

    [Action]
    [UserManagerPermission]
    public object User(ulong? id, UserForm form)
    {
      return DataSetup_ItemDetails<UserForm, UserRegistrationPage>(new object[] { id }, form, URIS.COMMON_USERS);
    }

    [Action]
    public object Projects(ProjectListFilter filter)
    {
      return DataSetup_Index<ProjectListFilter, ProjectGrid, ProjectsPage>(filter);
    }

    [Action]
    [ProjectManagerPermission]
    public object Project(ulong? id, ProjectForm form)
    {
      return DataSetup_ItemDetails<ProjectForm, ProjectPage>(new object[] { id }, form, URIS.COMMON_PROJECTS);
    }
    
    [Action]
    public object Categories(CategoryListFilter filter)
    {
      return DataSetup_Index<CategoryListFilter, CategoryGrid, CategoriesPage>(filter);
    }

    [Action]
    [CategoryEditPermission]
    public object Category(ulong? id, CategoryForm form)
    {
        return DataSetup_ItemDetails<CategoryForm, CategoryPage>(new object[] { id}, form, URIS.COMMON_CATEGORIES);
    }

    [Action]
    [CategoryEditPermission]
    public object deletecategory(ulong? id, CategoryForm form)
    {
        return DataSetup_DeleteItem<CategoryForm>(new object[] { id }, form, URIS.COMMON_CATEGORIES);
    }
  }
}
