using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issue")]
  public class IssueRow : ZhabaRowWithULongPK
  {
    public IssueRow() : base() { }
    public IssueRow(RowULongPKAction action) : base(action) { }


    [Field(required: true, nonUI: true)]
    public ulong? C_Project { get; set; }

    [Field]
    public ulong? C_Milestone { get; set; }

    [Field]
    public ulong? C_Area { get; set; }

    [Field]
    public ulong? C_Component { get; set; }

    [Field(required: true,
           kind: DataKind.Text,
           minLength: ZhabaName.MIN_LEN,
           maxLength: ZhabaName.MAX_LEN,
           description: "Name",
           metadata: @"Placeholder='Name'")]
    public string Name { get; set; }

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
           minLength: ZhabaMnemonic.MIN_LEN,
           maxLength: ZhabaMnemonic.MAX_LEN,
           description: "Owner")]
    public string Owner { get; set; }

    [Field(required: true,
           kind: DataKind.DateTime,
           description: "Create Date")]
    public DateTime Creation_Date { get; set; }

    [Field(required: true,
           kind: DataKind.DateTime,
           description: "Change Date")]
    public DateTime Change_Date { get; set; }


    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var qry = QUser.GetUserByLogin<UserRow>(Creator);
      var creator = ZApp.Data.CRUD.LoadRow(qry);
      if (creator==null)
        return new CRUDFieldValidationException(this, "Creator", "Creator user not found");

      qry = QUser.GetUserByLogin<UserRow>(Owner.ToUpperInvariant());
      var owner = ZApp.Data.CRUD.LoadRow(qry);
      if (owner==null)
        return new CRUDFieldValidationException(this, "Owner", "Owner user not found");

      return null;
    }
  }
}

