using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

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


      [Field(required: true,
             min: ZhabaIntPercent.MIN_VALUE,
             max: ZhabaIntPercent.MAX_VALUE,
             description: "Proirity")]
      public ulong Priority { get; set; }
    #endregion
  }
}
