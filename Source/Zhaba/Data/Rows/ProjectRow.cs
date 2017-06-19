using System;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_project")]
  public class ProjectRow : ZhabaRowWithPKAndInUse
  {
    #region .ctor
      public ProjectRow() : base() { }
      public ProjectRow(RowPKAction action) : base(action) { }
    #endregion

    #region Properties
      [Field(required: true,
             minLength: ZhabaMnemonic.MIN_LEN,
             maxLength: ZhabaMnemonic.MAX_LEN,
             description: "Name",
             metadata: @"Placeholder='Name'")]
      public string Name { get; set; }

      [Field(maxLength: ZhabaDescription.MAX_LEN,
             kind: DataKind.Text,
             description: "Description",
             metadata: @"Placeholder='Description'")]
      public string Description { get; set; }

      [Field(required: true,
             description: "Creator")]
      public ulong? C_Creator { get; set; }
    #endregion
  }
}
