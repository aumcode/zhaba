using NFX.DataAccess.CRUD;
using System;
using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issueassign")]
  public class IssueAssignRow : ZhabaRowWithPK
  {
    #region .ctor
    public IssueAssignRow() : base() { }
    public IssueAssignRow(RowPKAction action) : base(action) { }
    #endregion

    #region Field
    [Field(required: true, description: "Issue")]
    public ulong C_Issue { get; set; }
    [Field(required: true, description: "Assignee")]
    public ulong C_User { get; set; }
    [Field(required: true, kind: DataKind.DateTime, description: "Issue assignment time for the user")]
    public DateTime Open_TS { get; set; }

    [Field(required: false, kind: DataKind.DateTime, description: "Issue close time for the user")]
    public DateTime? Close_TS { get; set; }
    [Field(required: true, description: "Operator assignee")]
    public ulong C_Open_Oper { get; set; }
    [Field(required: false, description: "Operator assignee")]
    public ulong? C_Close_Oper { get; set; }


    [Field(required: false, description: "Open meeting")]
    public ulong? C_Open_Meeting { get; set; }
    [Field(required: false, description: "Close meeting")]
    public ulong? C_Close_Meeting { get; set; }
    [Field(required: false,
           kind: DataKind.Text,
           minLength: ZhabaMnemonic.MIN_LEN,
           maxLength: ZhabaMnemonic.MAX_LEN,
           description: "Note",
           metadata: @"Placeholder='Note'")]
    public string Note { get; set; }

    #endregion
  }
}
