using NFX.Wave;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;
using Zhaba.Data.Filters;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages.List;
using Zhaba.Web.Pages;
using Zhaba.Web.Controls;
using Zhaba.Web.Controls.Grids;

namespace Zhaba.Web.Controllers
{
  [SiteUserPermission]
  public class Project : ProjectControllerBase
  {
    [Action]
    public object Select()
    {
      WorkContext.NeedsSession(onlyExisting: true);

      if (IsValidUser)
        this.ZhabaWebSession.SelectedProject = ProjectRow;
      else
        throw HTTPStatusException.Forbidden_403();

      return new Redirect(URIS.DASHBOARD);
    }

    [Action]
    public object Index()
    {
      return new Redirect(URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
    }

    [Action]
    public object Milestones(MilestoneListFilter filter)
    {
      return DataSetup_Index<MilestoneListFilter, MilestoneGrid, MilestonesPage>(filter);
    }

    [Action]
    public object Milestone(ulong? id, MilestoneForm form)
    {
      return DataSetup_ItemDetails<MilestoneForm, MilestonePage>(new object[] { ProjectRow, id }, form, URIS.ForPROJECT_MILESTONES(ProjectRow.Counter));
    }

    [Action]
    public object Issues(IssueListFilter filter)
    {
      return DataSetup_Index<IssueListFilter, IssueGrid, IssuesPage>(filter);
    }

    [Action]
    public object Issue(ulong? id, IssueForm form)
    {
      return DataSetup_ItemDetails<IssueForm, IssuePage>(new object[] { ProjectRow, id }, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
    }

    [Action]
    public object Components(ComponentListFilter filter)
    {
      return DataSetup_Index<ComponentListFilter, ComponentGrid, ComponentsPage>(filter);
    }
    
    [Action]
    public object Component(ulong? counter, ComponentForm form)
    {
      return DataSetup_ItemDetails<ComponentForm, ComponentPage>(new object[] { ProjectRow, counter }, form, URIS.ForPROJECT_COMPONENTS(ProjectRow.Counter));
    }

    [Action]
    public object Areas(AreaListFilter filter)
    {
      return DataSetup_Index<AreaListFilter, AreaGrid, AreasPage>(filter);
    }

    [Action]
    public object Area(ulong? counter, AreaForm form)
    {
      return DataSetup_ItemDetails<AreaForm, AreaPage>(new object[] { ProjectRow, counter }, form, URIS.ForPROJECT_AREAS(ProjectRow.Counter));
    }
  }
}
