using System;
using NFX;
using Zhaba.Data;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Data.Domains;
using System.Reflection;

namespace Zhaba.DataLogic
{
  internal class ZhabaIssueLogLogic : LogicBase, IIssueLogic
  {
    #region .ctor
    public ZhabaIssueLogLogic(ZhabaDataStore store) : base(store)
    {
    }
    #endregion

    #region Public

    public void WriteLogEvent(IssueLogEvent evt)
    {
      if (evt == null) throw new ZhabaDataException(StringConsts.ARGUMENT_ERROR+"WriteEvent(evt == null)");

      var error = evt.Validate();
      if(error != null ) throw new ZhabaDataException(StringConsts.ARGUMENT_ERROR + "WriteEvent(!evt.Validate())", error);
      
      var etp = evt.GetType();
      var mi = this.GetType().GetMethod("write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { etp }, null);
      if (mi == null) throw new ZhabaDataException(StringConsts.ARGUMENT_ERROR + "WriteEvent(evt'{0}' not matched)".Args(etp.Name), error);

      try
      {
        mi.Invoke(this, new object[] { evt });
      }
      catch (Exception e)
      {
        var t = e is TargetInvocationException ?  e.InnerException : e ;
        throw new ZhabaDataException(StringConsts.ISSUELOG_EVENT_INVOCATION_ERROR.Args(t.ToMessageWithType()), t);
      }
    }

    #endregion

    #region Private

    private void write(CreateIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.NEW);
      newRow.C_Category = evt.C_Category;
      newRow.C_Milestone = evt.C_Milestone;
      newRow.Completeness = 0;
      newRow.Priority = evt.Priority;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(EditIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt);
      newRow.C_Category = evt.C_Category;
      newRow.C_Milestone = evt.C_Milestone;
      newRow.Completeness = 0;
      newRow.Priority = evt.Priority;
      newRow.Status = newRow.Status ?? ZhabaIssueStatus.NEW;
      ZApp.Data.CRUD.Insert(newRow);
    }
    private void write(ProceedIssueEvent evt) 
    {
      IssueLogRow newRow = NewIssueLog(evt);
      newRow.Completeness = evt.Completeness;
      newRow.Description = evt.Description;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(AssignIssueEvent evt)
    {
        IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.ASSIGNED);
        newRow.Note = evt.Note;
        ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(CloseIssueEvent evt) 
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.CLOSED);
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(ReopenIssueEvent evt) 
    {
      IssueLogRow newRow = NewIssueLog(evt,ZhabaIssueStatus.REOPEN);
      newRow.C_Category = evt.C_Category;
      newRow.C_Milestone = evt.C_Milestone;
      newRow.Completeness = 0;
      newRow.Priority = evt.Priority;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(DeferIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.DEFER);
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(DoneIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.DONE);
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(ChangePriorityIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt);
      newRow.Priority = evt.Priority;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(ChangeCategoryIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt);
      newRow.C_Category = evt.C_Category;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private IssueLogRow NewIssueLog(IssueLogEvent evt, string status = null)
    {
      IssueLogRow result = new IssueLogRow(RowPKAction.CtorGenerateNewID) 
      {
        C_Issue = evt.C_Issue,
        C_Operator = evt.C_User,
        Status_Date = evt.DateUTC
      };
      IssueLogRow oldRow = ZApp.Data.CRUD.LoadRow<IssueLogRow>(QIssueLog.FindLastIssueLogByIssue<IssueLogRow>(evt.C_Issue));
      if (oldRow != null)
      {
        oldRow.CopyFields(result,
          fieldFilter: (n, f) =>
              f.Name != "Counter" &&
              f.Name != "C_Issue" &&
              f.Name != "C_Operator" &&
              f.Name != "Status_Date"
        );
      }
      if (status != null) result.Status = status;
      return result;
    }

    #endregion
  }
}
