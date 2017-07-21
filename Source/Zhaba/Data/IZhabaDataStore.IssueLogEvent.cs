using System;
using NFX.DataAccess.CRUD;

namespace Zhaba.Data
{


  public abstract class IssueLogEvent : TypedRow
  {
    [Field(required: true)]
    public ulong C_Issue { get; set; }

    [Field(required: true)]
    public ulong C_User { get; set; }

    [Field(required: true)]
    public DateTime DateUTC { get; set; }

    [Field(required: false)]
    public string Description { get; set; }


  }

  public sealed class CreateIssueEvent : IssueLogEvent
  {
    [Field(required: true)]
    public ulong C_Milestone { get; set; }

    [Field(required: true)]
    public ulong Priority { get; set; }

    [Field(required: true)]
    public ulong C_Category { get; set; }
    
    [Field(required: true)]
    public DateTime Start_Date { get; set; }

    [Field(required: true)]
    public DateTime Due_Date { get; set; }
  }

  public sealed class EditIssueEvent : IssueLogEvent
  {
    [Field(required: true)]
    public ulong C_Milestone { get; set; }

    [Field(required: true)]
    public ulong Priority { get; set; }

    [Field(required: true)]
    public ulong C_Category { get; set; }

    [Field(required: true)]
    public DateTime Start_Date { get; set; }

    [Field(required: true)]
    public DateTime Due_Date { get; set; }

  }

  public sealed class AssignIssueEvent : IssueLogEvent
  {

  }

  public sealed class CloseIssueEvent : IssueLogEvent
  {

  }

  public sealed class ReopenIssueEvent : IssueLogEvent
  {

  }

  public sealed class ResumeIssueEvent : IssueLogEvent
  {
    public string NextStatus { get; set; }
  }

  public sealed class DoneIssueEvent : IssueLogEvent
  {

  }

  public sealed class DeferIssueEvent : IssueLogEvent
  {

  }

  public sealed class CancelIssueEvent : IssueLogEvent
  {

  }

  public sealed class ProceedIssueEvent : IssueLogEvent
  {
    [Field(required: true)]
    public int Completeness { get; set; }
    
  }

  public sealed class ChangePriorityIssueEvent : IssueLogEvent
  {
    [Field(required: true)]
    public ulong C_Milestone { get; set; }

    [Field(required: true)]
    public ulong Priority { get; set; }
  }

  public sealed class ChangeCategoryIssueEvent : IssueLogEvent
  {
    [Field(required: true)]
    public ulong C_Category { get; set; } 
  }
  
  public sealed class ChangeIssueAreaEvent : IssueLogEvent
  {

  }

  public sealed class ChangeIssueComponentEvent : IssueLogEvent
  {

  }

}
