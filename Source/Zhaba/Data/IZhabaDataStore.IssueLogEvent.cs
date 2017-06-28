﻿using System;

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

    [Field(required: true)]
    public ulong C_Category { get; set; }

  }

  public sealed class CreateIssueEvent : IssueLogEvent
  {
    [Field(required: true)]
    public ulong C_Milestone { get; set; }

    [Field(required: true)]
    public ulong Priority { get; set; }

  }

  public sealed class CloseIssueEvent : IssueLogEvent
  {

  }

  public sealed class ProceedIssueEvent : IssueLogEvent
  {
    [Field(required: true)]
    public int Completeness { get; set; }

    [Field(required: false)]
    public string Description { get; set; }
  }

  public sealed class AssignIssueEvent :IssueLogEvent
  {
    [Field(required: false)]
    public string Note { get; set; }
  }

}
