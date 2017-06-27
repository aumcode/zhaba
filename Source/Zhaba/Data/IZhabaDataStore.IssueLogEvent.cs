using System;

using NFX.DataAccess;
using NFX.ApplicationModel.Pile;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Rows;

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

  }

  public sealed class CreateIssueEvent : IssueLogEvent
  {
    [Field(required: true)]
    public ulong C_Milestone { get; set; }

  }

  public sealed class CloseIssueEvent : IssueLogEvent
  {

  }

}
