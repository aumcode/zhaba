using NFX;
using NFX.Wave;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;
using Zhaba.Data.Filters;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
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
    [PMPermission]
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
    [PMPermission]
    public object Issue(ulong? id, IssueForm form)
    {
      return DataSetup_ItemDetails<IssueForm, IssuePage>(new object[] { ProjectRow, id }, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
    }

    [Action]
    [PMPermission]
    public object IssueArea(IssueAreaListFilter filter, ulong? issue)
    {
      if (!issue.HasValue)
        throw HTTPStatusException.BadRequest_400("Missing issue");

      var qry = QProject.IssueByID<IssueRow>(ProjectRow.Counter, issue.Value);
      var issueRow = ZApp.Data.CRUD.LoadRow(qry);
      if (issueRow == null)
        throw HTTPStatusException.NotFound_404("Issue Row missing/deleted");

      filter.____SetIssue(issueRow);

      return DataSetup_Index<IssueAreaListFilter, IssueAreaGrid, IssueAreaPage>(filter);
    }

    [Action]
    [PMPermission]
    public object IssueComponent(IssueComponentListFilter filter, ulong? issue)
    {
      if (!issue.HasValue)
        throw HTTPStatusException.BadRequest_400("Missing issue");

      var qry = QProject.IssueByID<IssueRow>(ProjectRow.Counter, issue.Value);
      var issueRow = ZApp.Data.CRUD.LoadRow(qry);
      if (issueRow == null)
        throw HTTPStatusException.NotFound_404("Issue Row missing/deleted");

      filter.____SetIssue(issueRow);

      return DataSetup_Index<IssueComponentListFilter, IssueComponentGrid, IssueComponentPage>(filter);
    }

    [Action("linkissuecomponent", 1, "match { methods=POST accept-json=true }")]
    [PMPermission]
    public object LinkIssueComponent(ulong? issue, ulong? component, bool link)
    {
      if (!issue.HasValue)
        throw HTTPStatusException.BadRequest_400("Missing issue");

      if (!component.HasValue)
        throw HTTPStatusException.BadRequest_400("Missing area");

      var issueArea = new IssueComponentRow
      {
        C_Project = ProjectRow.Counter,
        C_Issue = issue.Value,
        C_Component = component.Value
      };

      var verror = issueArea.Validate(App.DataStore.TargetName);
      if (verror != null) throw verror;

      var affected = 0;
      if (link)
      {
        affected = ZApp.Data.CRUD.Insert(issueArea);
      }
      else
      {
        affected = ZApp.Data.CRUD.Delete(issueArea);
        if (affected <= 0)
          throw HTTPStatusException.NotFound_404("Issue-Area link not found");
      }

      return NFX.Wave.SysConsts.JSON_RESULT_OK;
    }

    [Action("linkissuearea", 1, "match { methods=POST accept-json=true }")]
    [PMPermission]
    public object LinkIssueArea(ulong? issue, ulong? area, bool link)
    {
      if (!issue.HasValue)
        throw HTTPStatusException.BadRequest_400("Missing issue");

      if (!area.HasValue)
        throw HTTPStatusException.BadRequest_400("Missing area");

      var issueArea = new IssueAreaRow
      {
        C_Project = ProjectRow.Counter,
        C_Issue = issue.Value,
        C_Area = area.Value
      };

      var verror = issueArea.Validate(App.DataStore.TargetName);
      if (verror != null) throw verror;

      var affected = 0;
      if (link)
      {
        affected = ZApp.Data.CRUD.Insert(issueArea);
      }
      else
      {
        affected = ZApp.Data.CRUD.Delete(issueArea);
        if (affected <= 0)
          throw HTTPStatusException.NotFound_404("Issue-Area link not found");
      }

      return NFX.Wave.SysConsts.JSON_RESULT_OK;
    }

    [Action]
    public object Components(ComponentListFilter filter)
    {
      return DataSetup_Index<ComponentListFilter, ComponentGrid, ComponentsPage>(filter);
    }

    [Action("component", 0, "match { methods=POST,GET }")]
    [PMPermission]
    public object Component(ulong? counter, ComponentForm form)
    {
      return DataSetup_ItemDetails<ComponentForm, ComponentPage>(new object[] { ProjectRow, counter }, form, URIS.ForPROJECT_COMPONENTS(ProjectRow.Counter));
    }

    [Action("component", 0, "match { methods=DELETE accept-json=true }")]
    [PMPermission]
    public object Component_DELETE(ulong counter)
    {
      //return DataSetup_DeleteItem(new object[] { ProjectRow, counter }, null, URIS.ForPROJECT_COMPONENTS(ProjectRow.Counter));
      
      ZApp.Data.CRUD.ExecuteWithoutFetch(QProject.DeleteComponentByID(ProjectRow.Counter, counter));
      return NFX.Wave.SysConsts.JSON_RESULT_OK;
    }

    [Action]
    public object Areas(AreaListFilter filter)
    {
      return DataSetup_Index<AreaListFilter, AreaGrid, AreasPage>(filter);
    }

    [Action("area", 0, "match { methods=POST,GET }")]
    [PMPermission]
    public object Area(ulong? counter, AreaForm form)
    {
      return DataSetup_ItemDetails<AreaForm, AreaPage>(new object[] { ProjectRow, counter }, form, URIS.ForPROJECT_AREAS(ProjectRow.Counter));
    }

    [Action("area", 0, "match { methods=DELETE accept-json=true }")]
    [PMPermission]
    public object Area_DELETE(ulong counter)
    {
      ZApp.Data.CRUD.ExecuteWithoutFetch(QProject.DeleteAreaByID(ProjectRow.Counter, counter));
      return NFX.Wave.SysConsts.JSON_RESULT_OK;
    }
  }
}
