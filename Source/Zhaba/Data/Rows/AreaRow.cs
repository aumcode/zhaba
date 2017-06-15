using System;
using System.Collections.Generic;

using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_area")]
  public class AreaRow : ZhabaRowProjectBase
  {
    public AreaRow() : base() { }
    public AreaRow(RowPKAction action) : base(action) { }


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
  }
}
