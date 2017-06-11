using System;
using System.Collections.Generic;

using NFX;
using NFX.Wave;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;
using Zhaba.Data.Filters;
using Zhaba.Data.Rows;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages.List;
using Zhaba.Web.Pages;
using Zhaba.Web.Controls;

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
      return DataSetup_ProjectItemDetails<MilestoneForm, MilestonePage>(id, form, URIS.ForPROJECT_MILESTONES(ProjectRow.Counter));
    }

    [Action]
    public object Issues(IssueListFilter filter)
    {
      return DataSetup_Index<IssueListFilter, IssueGrid, IssuesPage>(filter);
    }

    [Action]
    public object Issue(ulong? id, IssueForm form)
    {
      return DataSetup_ProjectItemDetails<IssueForm, IssuePage>(id, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
    }
  }
}
