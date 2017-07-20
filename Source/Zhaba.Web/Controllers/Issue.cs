using NFX;
using NFX.Wave;
using NFX.Wave.MVC;
using System;
using System.Reflection;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages;

using Zhaba.Data.Filters;
using Zhaba.Web.Pages.Reports;

namespace Zhaba.Web.Controllers
{
  [SiteUserPermission]
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
        var pForm = arg as IssueFormBase;
        if (pForm != null)
        {
          pForm.____SetIssue(issue);
          break;
        }
        var pForm1 = arg as IssueFilterBase;
        if (pForm1 != null)
        {
          pForm1.____SetIssue(issue);
          break;
        }
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

    [Action("defer", 0, "match { methods=GET accept-json=true }")]
    [PMPermission]
    public object Defer()
    {
      ZApp.Data.Issue.DeferIssue(ProjectRow.Counter, IssueRow.Counter, ZhabaUser.DataRow.Counter);
      return NFX.Wave.SysConsts.JSON_RESULT_OK;
    }

    [Action("issueassign", 0, "match { methods=POST,GET }")]
    [PMPermission]
    public object IssueAssign(ulong? id, IssueAssignForm form)
    {
      id = id == 0 ? null : id;
      var result = DataSetup_ItemDetails<IssueAssignForm, IssueAssignPage>(new object[] { ProjectRow, IssueRow, id }, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
      return result;
    }

    [Action("issuecancel", 0, "match { methods=POST,GET }")]
    [PMPermission]
    public object IssueCancel(ulong? id, CancelNoteEditForm form)
    {
      id = id == 0 ? null : id;
      var result = DataSetup_ItemDetails<CancelNoteEditForm, NoteEditPage>(new object[] { ProjectRow, IssueRow, id }, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
      return result;
    }

    [Action("statusnote", 0, "match { methods=POST,GET }")]
    [PMPermission]
    public object StatusNote(ulong? id, NoteEditForm form)
    {
      return DataSetup_ItemDetails<NoteEditForm, NoteEditPage>(new object[] { ProjectRow, IssueRow, id }, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
    }

    [Action("chat", 0, "match { methods=POST,GET }")]
    public object Chat(ulong? id, IssueChatForm form)
    {
      return DataSetup_ItemDetails<IssueChatForm, IssueChatPage>(new object[] { ProjectRow, IssueRow, id }, form, URIS.ForPROJECT_ISSUES(ProjectRow.Counter));
    }
    
    [Action("chatlist", 0, "match { methods=GET}")]
    public object ChatList_GET()
    {
      var filter = new IssueChatFilter {Limit = 5};
      return Dashboard.FormJSON(filter);    
    }

    [Action("chatlist", 0, "match { methods=POST accept-json=true}")]
    public object ChatList_POST(IssueChatFilter filter)
    {
      object data;
      filter.Save(out data);
      return new JSONResult(data, JSONWritingOptions.CompactRowsAsMap);    
    }

    [Action("chatreport", 0, "match { methods=GET}")]
    public object ChatReport_GET()
    {
      IssueChatReport report = new IssueChatReport(ProjectRow, IssueRow);
      return report;
    }

    [Action("statusreport", 0, "match { methods=GET}")]
    public object StatusReport_GET()
    {
      IssueStatusReport report = new IssueStatusReport(ProjectRow, IssueRow);
      return report;
    }
    
    [Action("assignmentreport", 0, "match { methods=GET}")]
    public object AssignmentReport_GET()
    {
      IssueAssignmentReport report = new IssueAssignmentReport(ProjectRow, IssueRow);
      return report;
    }

    [Action("assignmentreport", 0, "match { methods=GET}")]
    public object areas()
    {
      
      return null;
    }
    
    #region .pvt

    protected object DataSetup_PopUp(object[] args, IssueAssignForm form, string postRedirect)
    {
      Exception error = null;

      if (WorkContext.IsPOST)
      {
        Row row;
        error = form.Save(out row);
        if (error == null)
        {
          if (WorkContext.RequestedJSON)
            return JSON_OK_ROW_ID(row);
          else
            return new Redirect(postRedirect);
        }
      }
      else
        form = (IssueAssignForm)Activator.CreateInstance(typeof(IssueAssignForm), args);
        

      if (WorkContext.RequestedJSON)
        return new ClientRecord(form, error);
      else
        return MakePage<IssueAssignDashboard>(form, error);
    }
    

    #endregion
  }
}
