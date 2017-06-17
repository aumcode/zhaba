using System;

using NFX;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_agenda")]
  public class AgendaRow : ZhabaRowWithPK
  {
    #region .ctor
      public AgendaRow() : base() {}
      public AgendaRow(RowPKAction action) : base(action) {}
    #endregion

    #region Properties
      [Field(required: true,
             kind: DataKind.Text,
             minLength: ZhabaMnemonic.MIN_LEN,
             maxLength: ZhabaMnemonic.MAX_LEN,
             description: "Name",
             metadata: @"Placeholder='Name'")]
      public string Name { get; set; }

      [Field(maxLength: ZhabaDescription.MAX_LEN,
             kind: DataKind.Text,
             description: "Description",
             metadata: @"Placeholder='Description' ControlType='textarea'")]
      public string Description { get; set; }

      [Field(required: true,
             kind: DataKind.DateTime,
             description: "Start Date")]
      public DateTime Start_Date { get; set; }

      [Field(required: true,
             kind: DataKind.DateTime,
             description: "End Date")]
      public DateTime End_Date { get; set; }

      [Field(required: true,
             description: "Creator")]
      public ulong? C_Creator { get; set; }
    #endregion

    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var qry = QUser.GetUserById<UserRow>(C_Creator.Value);
      var user = ZApp.Data.CRUD.LoadRow(qry);
      if (user == null)
        return new CRUDFieldValidationException(this, "C_Creator", "Creator user not found");

      if (Start_Date > End_Date)
        return new CRUDFieldValidationException(this.Schema.Name, "Start_Date", "Start time is greater than end time");

      return null;
    }
  }
}
