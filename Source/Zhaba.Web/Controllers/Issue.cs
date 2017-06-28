using NFX;
using NFX.Wave;
using NFX.Wave.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages;

namespace Zhaba.Web.Controllers
{
  public class Issue : ProjectControllerBase
  {
    [ThreadStatic]
    private static IssueRow ts_IssueRow;

    protected IssueRow IssueRow { get { return ts_IssueRow; } }

    protected override bool BeforeActionInvocation(WorkContext work, string action, MethodInfo method, object[] args, ref object result)
    {
      var ret = base.BeforeActionInvocation(work, action, method, args, ref result);

      var projID = work.MatchedVars[URIS.PROJECT_ID_PARAM].AsNullableULong();
      if (!projID.HasValue)
        throw HTTPStatusException.BadRequest_400("Missing " + URIS.PROJECT_ID_PARAM);

      var issueID = work.MatchedVars[URIS.ISSUE_ID_PARAM].AsNullableULong();
      if (!issueID.HasValue)
        return ret;

      //3. Fetch Project row data
      var qry = QProject.IssueByID<IssueRow>(projID.Value, issueID.Value);
      var issue = ZApp.Data.CRUD.LoadRow(qry);
      if (issue == null)
        throw HTTPStatusException.NotFound_404("Project missing/deleted");

      //4. Inject thread context
      ts_IssueRow = issue;

      //5. Bind regular params
      var handled = base.BeforeActionInvocation(work, action, method, args, ref result);

      //6. Inject Project Row in forms and filters
      foreach (var arg in args)
      {
        var pForm = arg as IssueAssignForm;
        if (pForm != null)
        {
          pForm.____SetIssue(issue);
          continue;
        }
/*
        var pFilter = arg as ProjectFilterBase;
        if (pFilter != null)
        {
          pFilter.____SetProject(project);
          continue;
        }
*/
      }

      return ret;
    }

    [Action]
    [PMPermission]
    public object Index(ulong? id, IssueForm form)
    {
      return DataSetup_ItemDetails<IssueForm, IssuePage>(new object[] { ProjectRow, id }, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
    }

    [Action("close", 0, "match { methods=DELETE accept-json=true }")]
    [PMPermission]
    public object Index_DELETE()
    {
      ZApp.Data.Issue.CloseIssue(ProjectRow.Counter, IssueRow.Counter, ZhabaUser.DataRow.Counter);
      return NFX.Wave.SysConsts.JSON_RESULT_OK;
    }

    [Action("reopen", 0, "match { methods=GET accept-json=true }")]
    [PMPermission]
    public object Index_REOPEN()
    {
      ZApp.Data.Issue.ReOpenIssue(ProjectRow.Counter, IssueRow.Counter, ZhabaUser.DataRow.Counter);
      return NFX.Wave.SysConsts.JSON_RESULT_OK;
    }

    [Action("issueassign", 0, "match { methods=POST,GET }")]
    [PMPermission]
    public object IssueAssign(ulong? id, IssueAssignForm form)
    {
      return DataSetup_ItemDetails<IssueAssignForm, IssueAssignPage>(new object[] { ProjectRow, IssueRow, id }, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
    }

  }
}
