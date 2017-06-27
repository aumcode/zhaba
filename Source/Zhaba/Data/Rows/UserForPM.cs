using NFX.DataAccess.CRUD;
using System;
using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table("tbl_user")]
  public class UserForPM : ZhabaRowWithPKAndInUse
  {
    #region .ctor
    public UserForPM() : base() { }
    public UserForPM(RowPKAction action) : base(action) { }
    #endregion

    #region Properties
    [Field(required: true,
              minLength: ZhabaHumanName.MIN_LEN,
              maxLength: ZhabaHumanName.MAX_LEN,
              description: "First Name",
              metadata: @"Placeholder='User First Name'")]
    public string First_Name { get; set; }

    [Field(required: true,
           minLength: ZhabaHumanName.MIN_LEN,
           maxLength: ZhabaHumanName.MAX_LEN,
           description: "Last Name",
           metadata: @"Placeholder='User Last Name'")]
    public string Last_Name { get; set; }

    [Field(required: true,
           kind: DataKind.EMail,
           minLength: ZhabaEMail.MIN_LEN,
           maxLength: ZhabaEMail.MAX_LEN,
           description: "EMail",
           metadata: @"Placeholder='User EMail'")]
    public string EMail { get; set; }

    public string FullName 
    { 
      get 
      {
        return First_Name + " " + Last_Name;
      } 
    }
    #endregion

  }
}
