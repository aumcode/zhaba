using System;
using System.Collections.Generic;

using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_project")]
  public class ProjectRow : ZhabaRowWithPK
  {
    public ProjectRow() : base() { }
    public ProjectRow(RowPKAction action) : base(action) { }

    [Field(required: true,
           maxLength: 50,
           description: "Name",
           metadata: @"Placeholder='Name'")]
    public string Name { get; set; }

    [Field(maxLength: ZhabaDescription.MAX_LEN,
           kind: DataKind.Text,
           description: "Description",
           metadata: @"Placeholder='Description'")]
    public string Description { get; set; }
  }
}
