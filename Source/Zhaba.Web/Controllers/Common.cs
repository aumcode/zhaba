using NFX;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;
using Zhaba.Data.Filters;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages;
using Zhaba.Web.Controls.Grids;
using Zhaba.Web.Pages.List;
using Zhaba.Data.QueryBuilders;
using System;

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
    [PMPermission]
    public object Project(ulong? id, ProjectForm form)
    {
      return DataSetup_ItemDetails<ProjectForm, ProjectPage>(new object[] { id }, form, URIS.COMMON_PROJECTS);
    }
    
    [Action]
    public object Categories(CategoryListFilter filter)
    {
      return DataSetup_Index<CategoryListFilter, CategoryGrid, CategoriesPage>(filter);
    }

    [Action("category", 0, "match { methods=POST,GET }")]
    [AdminPermission]
    public object Category(ulong? id, CategoryForm form)
    {
        return DataSetup_ItemDetails<CategoryForm, CategoryPage>(new object[] { id}, form, URIS.COMMON_CATEGORIES);
    }

    [Action("category", 0, "match { methods=DELETE accept-json=true }")]
    [AdminPermission]
    public object Category_DELETE(ulong id)
    {
      ZApp.Data.CRUD.ExecuteWithoutFetch(QCategory.DeleteCategoryByID(id));
      return NFX.Wave.SysConsts.JSON_RESULT_OK;
    }

    [Action("usersforpm", 0, "match { method=GET accept-json=true}")]
    [PMPermission]
    public object UsersForPM(UserListFilter filter) 
    {
      throw new NotImplementedException();
    }
  }
}
