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
    public IssueLogRow() : base() { }
    public IssueLogRow(RowPKAction action) : base(action) { }


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


    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var qry = QUser.GetUserById<UserRow>(C_Operator);
      var oper = ZApp.Data.CRUD.LoadRow(qry);
      if (oper == null)
        return new CRUDFieldValidationException(this, "C_Operator", "Operator user not found");

      qry = QUser.GetUserById<UserRow>(C_Assignee.Value);
      var assignee = ZApp.Data.CRUD.LoadRow(qry);
      if (assignee==null)
        return new CRUDFieldValidationException(this, "C_Assignee", "Assignee user not found");

      return null;
    }
  }
}
