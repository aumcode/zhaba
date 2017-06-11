using NFX.ApplicationModel;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_user")]
  public class UserRow : ZhabaRowWithULongPK
  {
    public UserRow() : base() { }
    public UserRow(RowULongPKAction action) : base(action) { }

    [Field(required: true,
           minLength: ZhabaMnemonic.MIN_LEN,
           maxLength: ZhabaMnemonic.MAX_LEN,
           metadata: @"Placeholder='User Login' Hint='Enter your Screen Name or E-Mail'")]
    public string Login { get; set; }

    [Field(required: true,
           minLength: ZhabaName.MIN_LEN,
           maxLength: ZhabaName.MAX_LEN,
           description: "First Name",
           metadata: @"Placeholder='First Name'")]
    public string First_Name { get; set; }

    [Field(required: true,
           minLength: ZhabaName.MIN_LEN,
           maxLength: ZhabaName.MAX_LEN,
           description: "Last Name",
           metadata: @"Placeholder='Last Name'")]
    public string Last_Name { get; set; }

    [Field(required: true,
           maxLength: ZhabaUserRoleType.MAX_LEN,
           valueList: ZhabaUserRoleType.VALUE_LIST)]
    public string Role { get; set; }

    [Field(required: true)]
    public string Password_Hash { get; set; }

    [Field(required: true)]
    public string Password_Salt { get; set; }
  }
}
