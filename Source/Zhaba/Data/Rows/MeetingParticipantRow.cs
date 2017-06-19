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
  [Table(name: "tbl_meetingparticipant")]
  public class MeetingParticipantRow : TypedRow
  {
    public MeetingParticipantRow() : base() {}

    #region Properties
      [Field(required: true, key: true, description: "Meeting")]
      public ulong C_Meeting { get; set; }

      [Field(required: true, key: true, description: "Participant")]
      public ulong C_Participant { get; set; }

      [Field(required: true,
             maxLength: ZhabaMeetingParticipationType.MAX_LEN,
             valueList: ZhabaMeetingParticipationType.VALUE_LIST,
             description: "Participation Type")]
      public ulong Participation_Type { get; set; }

      [Field(kind: DataKind.Text,
             maxLength: ZhabaNote.MAX_LEN,
             description: "Admin Note",
             metadata: @"Placeholder='Admin Note' ControlType='textarea'")]
      public string Admin_Note { get; set; }

      [Field(kind: DataKind.Text,
             maxLength: ZhabaNote.MAX_LEN,
             description: "Participant Note",
             metadata: @"Placeholder='Participant Note' ControlType='textarea'")]
      public string Participant_Note { get; set; }
    #endregion
  }
}
