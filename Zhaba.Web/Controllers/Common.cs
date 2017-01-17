using System;
using System.Collections.Generic;

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
    public object Components(ComponentListFilter filter)
    {
      return DataSetup_Index<ComponentListFilter, ComponentGrid, ComponentsPage>(filter);
    }

    [Action]
    public object Component(ulong? id, ComponentForm form)
    {
      return DataSetup_ItemDetails<ComponentForm, ComponentPage>(id, form, URIS.COMMON_COMPONENTS);
    }

    [Action]
    public object Areas(AreaListFilter filter)
    {
      return DataSetup_Index<AreaListFilter, AreaGrid, AreasPage>(filter);
    }

    [Action]
    public object Area(ulong? id, AreaForm form)
    {
      return DataSetup_ItemDetails<AreaForm, AreaPage>(id, form, URIS.COMMON_AREAS);
    }

    [Action]
    public object Users()
    {
      //return DataSetup_Index<UserListFilter, UserGrid, UsersPage>(filter);
      return null;
    }

    [Action]
    public object Projects(ProjectListFilter filter)
    {
      return DataSetup_Index<ProjectListFilter, ProjectGrid, ProjectsPage>(filter);
    }

    [Action]
    public object Project(ulong? id, ProjectForm form)
    {
      return DataSetup_ItemDetails<ProjectForm, ProjectPage>(id, form, URIS.COMMON_PROJECTS);
    }

  }
}
