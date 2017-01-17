using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issuelog")]
  public class IssueLogRow  : ZhabaRowWithULongPK
  {
    public IssueLogRow() : base() { }
    public IssueLogRow(RowULongPKAction action) : base(action) { }


    public ulong C_Project { get; set; }

    [Field(required: true, nonUI: true)]
    public ulong? C_Issue { get; set; }

    [Field(maxLength: ZhabaDescription.MAX_LEN,
           kind: DataKind.Text,
           description: "Description",
           metadata: @"Placeholder='Description' ControlType='textarea'")]
    public string Description { get; set; }

    [Field(required: true,
           maxLength: ZhabaIssueStatusType.MAX_LEN,
           valueList: ZhabaIssueStatusType.VALUE_LIST,
           description: "Status")]
    public string Status { get; set; }

    [Field(required: true,
           minLength: ZhabaMnemonic.MIN_LEN,
           maxLength: ZhabaMnemonic.MAX_LEN,
           description: "Creator")]
    public string Creator { get; set; }

    [Field(required: true,
           kind: DataKind.DateTime,
           description: "Create Date")]
    public DateTime Creation_Date { get; set; }


    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var qry = QUser.GetUserByLogin<UserRow>(Creator);
      var creator = ZApp.Data.CRUD.LoadRow(qry);
      if (creator==null)
        return new CRUDFieldValidationException(this, "Creator", "Creator user not found");

      var iqry = QProject.IssueByID<IssueRow>(C_Project, C_Issue.Value);
      var issue = ZApp.Data.CRUD.LoadRow(iqry);
      if (issue==null)
        return new CRUDFieldValidationException(this, "C_Issue", "Unknown issue");

      return null;
    }
  }
}
