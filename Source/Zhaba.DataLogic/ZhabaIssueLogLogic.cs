using System;
using NFX;
using Zhaba.Data;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Data.Domains;

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
    public void AddCreateIssueEvent(UserRow user, IssueRow issue, MilestoneRow milestone)
    {
      if (issue == null || user == null) throw new ZhabaDataException("Create Issue Log Error: user or issue is null");

      IssueLogRow OldRow = ZApp.Data.CRUD.LoadOneRow(QIssueLog.findLastIssueLogByIssue<IssueLogRow>(issue.Counter)) as IssueLogRow;
      IssueLogRow NewRow = new IssueLogRow();
      if (OldRow != null) {
          
      } 
      else 
      {
        NewRow.C_Operator = user.Counter;
        NewRow.C_Issue = issue.Counter;
        NewRow.C_Milestone = milestone.Counter;
        NewRow.Status = ZhabaIssueStatus.NEW;
        NewRow.Status_Date = DateTime.Now;
        NewRow.C_Category = 0;
        NewRow.C_Meeting = null;
        NewRow.Completeness = 0;
        NewRow.Note = null;
      }
      ZApp.Data.CRUD.Insert(NewRow);
    }

    public void AssigneeToIssue(UserRow oper, DateTime date, IssueRow issue, UserRow assignee, MilestoneRow milestone)
    {
      throw new NotImplementedException();
    }

    public void SetCompletness(UserRow user, int percent, string note)
    {
      throw new NotImplementedException();
    }

    public void UnAssigneeToIssue(UserRow oper, DateTime date, IssueRow issue, UserRow assignee, MilestoneRow milestone)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
