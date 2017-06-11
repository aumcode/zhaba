using System;
using System.Text;
using System.Security.Cryptography;

using NFX;
using NFX.DataAccess;
using NFX.Wave;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Rows;
using Zhaba.Security;

namespace Zhaba.Data.Forms
{
  public class SignUpForm : ZhabaForm
  {
    public static readonly string PASSWORD_SESSION_VAR = typeof(SignUpForm).FullName + "-PASSWORD";
    public static readonly string PASSWORD_FAKE = "_*-~!@#$%^'&*()_+`}{|<>?";

    public SignUpForm()
    {
      FormMode = FormMode.Insert; // Always Insert on sugn up
    }


    [Field(typeof(UserRow))]
    public string Login { get; set; }

    [Field(typeof(UserRow))]
    public string First_Name { get; set; }

    [Field(typeof(UserRow))]
    public string Last_Name { get; set; }

    [Field(typeof(UserRow))]
    public string Role { get; set; }

    [Field(required: true, metadata:
            @"Placeholder ='User Password'
              Hint='Enter your password'
              Password=true")]
    public string Password { get; set; }

    [Field(required: true, metadata:
            @"Placeholder='Confirm Password'
              Hint='Confirm your password'
              Password=true")]
    public string ConfirmPassword { get; set; }



    public override Exception Validate(string targetName)
    {
      if (ZhabaSession[PASSWORD_SESSION_VAR] == null && Password == PASSWORD_FAKE)
        return new CRUDFieldValidationException(this, "Password", "This password is not allowed");

      if (Password != ConfirmPassword)
      {
        Password = "";
        ConfirmPassword = "";
        ZhabaSession[PASSWORD_SESSION_VAR] = "";
        return new CRUDFieldValidationException(this, "Confirm_Password", "Password needs to be the same in both boxes. Please re-enter");
      }

      if (Password != PASSWORD_FAKE)
      {
        var score = NFX.Security.PasswordUtils.PasswordStrengthPercent(Password, NFX.Security.PasswordUtils.TOP_SCORE_NORMAL);
        if (score < Consts.MINIMUM_PASSWORD_STRENGTH_SCORE_PCT)
        {
          Password = "";
          ConfirmPassword = "";
          return new CRUDFieldValidationException(this, "Password", "The password is not strong enough. Try to mix numbers, letters of different case and punctuation marks");
        }

        ZhabaSession[PASSWORD_SESSION_VAR] = Password;
        Password = PASSWORD_FAKE; //fill with garbage
        ConfirmPassword = PASSWORD_FAKE;
      }

      var baseError = base.Validate(targetName);
      if (baseError != null) return baseError;

      Login = Login.ToUpperInvariant().Trim();

      return null;
    }


   protected override Exception DoSave(out object saveResult)
   {
     saveResult = null;

     var row = new UserRow(RowULongPKAction.Default);
     CopyFields(row);

     var typedPassword = ZhabaSession[PASSWORD_SESSION_VAR].AsString();
     if (typedPassword.IsNullOrWhiteSpace())
       return new CRUDFieldValidationException(this, "Password", "Password required");

     var salt = "SALT";
     row.Password_Hash = SecurityUtils.HashUserPassword(typedPassword, salt);
     row.Password_Salt = salt;

     var verror = row.ValidateAndPrepareForStore();
     if (verror != null) return verror;

     saveResult = row;

     try
     {
       ZApp.Data.CRUD.Insert(row);
     }
     catch (Exception error)
     {
       var eda = error as DataAccessException;
       if (eda != null && eda.KeyViolation != null)
         return new CRUDFieldValidationException(this, "Login", "This value is already used");

       throw error;
     }

     return null;
   }

  }
}
