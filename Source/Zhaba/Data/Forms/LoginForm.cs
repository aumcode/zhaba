using System;
using System.Text;
using System.Security.Cryptography;

using NFX;
using NFX.DataAccess.CRUD;

namespace Zhaba.Data.Forms
{
  public class LoginForm : ZhabaFormWithCSRFCheck
  {
    [Field(required: true,
           description: "User ID",
           metadata: @"Placeholder='User ID'
                       Hint='Enter your Screen Name or E-Mail'")]
    public string ID { get; set; }

    [Field(required: true,
           description: "User Password",
           metadata: @"Placeholder='User Password'
                       Hint='Enter your password'
                       Password=true")]
    public string Password { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      throw new NotSupportedException();
    }

  }
}
