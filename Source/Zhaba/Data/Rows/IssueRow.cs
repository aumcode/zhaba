using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issue")]
  public class IssueRow : ZhabaRowProjectBase
  {
    public IssueRow() : base() { }
    public IssueRow(RowPKAction action) : base(action) { }


    [Field(required: true,
           kind: DataKind.Text,
           minLength: ZhabaName.MIN_LEN,
           maxLength: ZhabaName.MAX_LEN,
           description: "Name",
           metadata: @"Placeholder='Name'")]
    public string Name { get; set; }

    [Field]
    public ulong? C_Parent { get; set; }

  }
}

