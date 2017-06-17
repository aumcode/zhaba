using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issuelog")]
  public class IssueLogRow  : ZhabaRowWithPK
  {
    #region .ctor
      public IssueLogRow() : base() { }
      public IssueLogRow(RowPKAction action) : base(action) { }
    #endregion

    #region Properties
      [Field(required: true, nonUI: true)]
      public ulong C_Issue { get; set; }

      [Field(required: true,
             description: "Milestone")]
      public ulong C_Milestone { get; set; }

      [Field(required: true,
             description: "Category")]
      public ulong C_Category { get; set; }

      [Field(maxLength: ZhabaDescription.MAX_LEN,
             kind: DataKind.Text,
             description: "Description",
             metadata: @"Placeholder='Description' ControlType='textarea'")]
      public string Description { get; set; }

      [Field(required: true,
             maxLength: ZhabaIssueStatus.MAX_LEN,
             valueList: ZhabaIssueStatus.VALUE_LIST,
             description: "Status")]
      public string Status { get; set; }

      [Field(description: "Assignee")]
      public ulong? C_Assignee { get; set; }

      [Field(required: true,
             kind: DataKind.DateTime,
             description: "Status Date")]
      public DateTime Status_Date { get; set; }

      [Field(required: true,
             description: "Operator")]
      public ulong C_Operator { get; set; }

      [Field(required: true,
             min: ZhabaIntPercent.MIN_VALUE,
             max: ZhabaIntPercent.MAX_VALUE,
             description: "Completeness")]
      public int Completeness { get; set; }

      [Field(kind: DataKind.Text,
             maxLength: ZhabaNote.MAX_LEN,
             description: "Note",
             metadata: @"Placeholder='Operator note' ControlType='textarea'")]
      public string Note { get; set; }

      [Field(description: "Meeting")]
      public ulong? C_Meeting { get; set; }
    #endregion

    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var qry = QUser.GetUserById<UserRow>(C_Operator);
      var oper = ZApp.Data.CRUD.LoadRow(qry);
      if (oper == null)
        return new CRUDFieldValidationException(this, "C_Operator", "Operator user not found");

      if (C_Assignee.HasValue)
      {
        qry = QUser.GetUserById<UserRow>(C_Assignee.Value);
        var assignee = ZApp.Data.CRUD.LoadRow(qry);
        if (assignee == null)
          return new CRUDFieldValidationException(this, "C_Assignee", "Assignee user not found");
      }
      
      if (C_Meeting.HasValue)
      {
        var mQry = QCommon.MeetingByID<MeetingRow>(C_Meeting.Value);
        var meeting = ZApp.Data.CRUD.LoadRow(mQry);
        if (meeting == null)
          return new CRUDFieldValidationException(this, "C_Meeting", "Non existing meeting");
      }

      return null;
    }
  }
}
