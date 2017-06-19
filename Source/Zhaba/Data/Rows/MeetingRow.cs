using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_meeting")]
  public class MeetingRow : ZhabaRowWithPK
  {
    #region .ctor
      public MeetingRow() : base() {}
      public MeetingRow(RowPKAction action) : base(action) {}
    #endregion

    #region Properties
      [Field(required: true, description: "Agenda")]
      public ulong C_Agenda { get; set; }

      [Field(required: true,
             kind: DataKind.DateTime,
             description: "Date")]
      public DateTime Date { get; set; }

      [Field(required: true,
             description: "Host")]
      public ulong C_Host { get; set; }

      [Field(kind: DataKind.Text,
             maxLength: ZhabaNote.MAX_LEN,
             description: "Note",
             metadata: @"Placeholder='Enter some note' ControlType='textarea'")]
      public string Note { get; set; }
    #endregion
  }
}
