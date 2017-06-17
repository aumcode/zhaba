using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;
using Zhaba.Data.QueryBuilders;

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

    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var mQry = QCommon.MeetingByID<MeetingRow>(C_Meeting);
      var meeting = ZApp.Data.CRUD.LoadRow(mQry);
      if (meeting == null)
        return new CRUDFieldValidationException(this, "C_Meeting", "Non existing meeting");

      var pQry = QUser.GetUserById<UserRow>(C_Participant);
      var participant = ZApp.Data.CRUD.LoadRow(pQry);
      if (participant == null)
        return new CRUDFieldValidationException(this, "C_Participant", "Participant user not found");

      return null;
    }
  }
}
