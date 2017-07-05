using System;
using NFX;
using NFX.DataAccess.CRUD;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issuelog")]
  public class IssueChatRow :ZhabaRowWithPK
  {
    #region .ctor

    public IssueChatRow()
    {
      
    }

    public IssueChatRow(RowPKAction action) : base(action)
    {
      
    }

    #endregion
    
    #region Fields

    [Field(required: true, nonUI: true)]
    public ulong C_Issue { get; set; }
    
    [Field(required: true,
      description: "User",
      metadata: @"Placeholder='User'"
    )]
    public ulong C_User { get; set; }

    [Field(
      required: true,
      kind: DataKind.DateTime,
      description: "Note datetime",
      metadata: @"Placeholder='Note datetime'"
    )]
    public DateTime Note_Date { get; set; } = App.TimeSource.UTCNow;
    
    [Field(
      required: false,
      kind: DataKind.Text,
      description: "Note",
      metadata: @"Placeholder='Note'"
      )]
    public string Note { get; set; }
    
    #endregion
  }
}