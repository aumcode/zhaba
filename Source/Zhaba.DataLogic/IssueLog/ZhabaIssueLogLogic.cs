using System;
using NFX;
using Zhaba.Data;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Data.Domains;
using System.Reflection;

namespace Zhaba.DataLogic
{
  internal class ZhabaIssueLogLogic : LogicBase, IIssueLogLogic
  {
    #region .ctor
    public ZhabaIssueLogLogic(ZhabaDataStore store) : base(store)
    {
    }
    #endregion

    #region Public

    public void WriteEvent(IssueLogEvent evt)
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
      IssueLogRow NewRow = new IssueLogRow(RowPKAction.CtorGenerateNewID) { Status = ZhabaIssueStatus.NEW };
      IssueLogRow OldRow = ZApp.Data.CRUD.LoadRow<IssueLogRow>(QIssueLog.FindLastIssueLogByIssue<IssueLogRow>(evt.C_Issue));
      if (OldRow != null)
      {
        OldRow.CopyFields(NewRow, fieldFilter: (n, f) => f.Name != "Counter");
      }
      NewRow.C_Issue = evt.C_Issue;
      NewRow.C_Operator = evt.C_User;
      NewRow.Status_Date = evt.DateUTC;
      NewRow.C_Category = evt.C_Category;
      NewRow.C_Milestone = evt.C_Milestone;
      NewRow.Completeness = 0;
      NewRow.Priority = evt.Priority;
      ZApp.Data.CRUD.Insert(NewRow);
    }

    private void write(ProceedIssueEvent evt) 
    {
      IssueLogRow OldRow = ZApp.Data.CRUD.LoadRow<IssueLogRow>(QIssueLog.FindLastIssueLogByIssue<IssueLogRow>(evt.C_Issue));
      if (OldRow == null) return;

      IssueLogRow NewRow = new IssueLogRow(RowPKAction.CtorGenerateNewID) { Status = ZhabaIssueStatus.DONE };
      OldRow.CopyFields(NewRow, fieldFilter: (n, f) => f.Name != "Counter");
      NewRow.C_Issue = evt.C_Issue;
      NewRow.C_Operator = evt.C_User;
      NewRow.Status_Date = evt.DateUTC;

      NewRow.Completeness = evt.Completeness;
      NewRow.Description = evt.Description;

      ZApp.Data.CRUD.Insert(NewRow);

    }

    private void write(AssignIssueEvent evt)
    {
      IssueLogRow OldRow = ZApp.Data.CRUD.LoadRow<IssueLogRow>(QIssueLog.FindLastIssueLogByIssue<IssueLogRow>(evt.C_Issue));
      if (OldRow == null) return;

      IssueLogRow NewRow = new IssueLogRow(RowPKAction.CtorGenerateNewID) { Status = ZhabaIssueStatus.ASSIGNED };
      OldRow.CopyFields(NewRow, fieldFilter: (n, f) => f.Name != "Counter" && f.Name != "Status");
      NewRow.C_Issue = evt.C_Issue;
      NewRow.C_Operator = evt.C_User;
      NewRow.Status_Date = evt.DateUTC;

      NewRow.Note = evt.Note;

      ZApp.Data.CRUD.Insert(NewRow);
    }

    #endregion
  }
}
