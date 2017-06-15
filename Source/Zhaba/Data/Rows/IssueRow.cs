using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issue")]
  public class IssueRow : ZhabaRowProjectBase
  {
    public IssueRow() : base() { }
    public IssueRow(RowPKAction action) : base(action) { }


    [Field(required: true,
           kind: DataKind.Text,
           minLength: ZhabaMnemonic.MIN_LEN,
           maxLength: ZhabaMnemonic.MAX_LEN,
           description: "Name",
           metadata: @"Placeholder='Name'")]
    public string Name { get; set; }

    [Field(description: "Parent Issue")]
    public ulong? C_Parent { get; set; }

  }
}

