using System;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Security;

using Zhaba.Data.Rows;
using Zhaba.Security;
using Zhaba.Data.QueryBuilders;
using NFX.Wave;

namespace Zhaba.Data.Forms
{
  public class UserForm : ZhabaForm
  {
    public static readonly string PASSWORD_SESSION_VAR = typeof(UserForm).FullName + "-PASSWORD";
    public static readonly string PASSWORD_FAKE = "_*-~!@#$%^'&*()_+`}{|<>?";

    public UserForm()
    {
      FormMode = FormMode.Insert; // Always Insert
    }

    public UserForm(ulong? id)
    {
        if(id.HasValue)
        {
          FormMode = FormMode.Edit;
          var qry = QCommon.UserByID<UserRow>(id.Value);
          var row = ZApp.Data.CRUD.LoadRow(qry);
          if (row != null)
            row.CopyFields(this);
          else
            throw HTTPStatusException.NotFound_404("Project");

          Password = PASSWORD_FAKE;
          ConfirmPassword = PASSWORD_FAKE;
          ZhabaSession[PASSWORD_SESSION_VAR] = Password;
          this.RoundtripBag[ITEM_ID_BAG_PARAM] = id.Value;
        }
        else
        {
          FormMode = FormMode.Insert; // Always Insert
        }
        
    }

    [Field(typeof(UserRow))]
    public string Login { get; set; }

    [Field(typeof(UserRow))]
    public string First_Name { get; set; }

    [Field(typeof(UserRow))]
    public string Last_Name { get; set; }

    [Field(typeof(UserRow))]
    public string EMail { get; set; }

    [Field(typeof(UserRow))]
    public string Status { get; set; }

    [Field(typeof(UserRow))]
    public string User_Rights { get; set; }

    [Field(typeof(UserRow), "Password", minLength: Sizes.PASSWORD_MIN_LEN)]
    public string Password { get; set; }
    
    [Field(required: true,
           minLength: Sizes.PASSWORD_MIN_LEN,
           description: "Confirm Password",
           metadata: "Placeholder='Confirm Password' Password=true Stored=true")]
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
        int score = 0;
        using (var password = IDPasswordCredentials.PlainPasswordToSecureBuffer(Password))
          score = App.SecurityManager.PasswordManager.CalculateStrenghtScore(PasswordFamily.Text, password);
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

     var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
     UserRow row = FormMode == FormMode.Edit && id.HasValue ? ZApp.Data.CRUD.LoadRow(QCommon.UserByID<UserRow>(id.Value)) : new UserRow(RowPKAction.Default);
     CopyFields(row, fieldFilter: (n, f) => f.Name != "Password");

     var typedPassword = ZhabaSession[PASSWORD_SESSION_VAR].AsString();
     if (typedPassword.IsNullOrWhiteSpace())
       return new CRUDFieldValidationException(this, "Password", "Password required");

     if (typedPassword != PASSWORD_FAKE || FormMode == FormMode.Insert)
     {
       row.Password = Password;
       using (var password = IDPasswordCredentials.PlainPasswordToSecureBuffer(typedPassword))
         row.Password = App.SecurityManager.PasswordManager.ComputeHash(PasswordFamily.Text, password).ToString();
     }

     row.User_Rights = row.User_Rights.IsNullOrEmpty() ? "{z:{rights:{}}}" : row.User_Rights;

     var verror = row.ValidateAndPrepareForStore();
     if (verror != null) return verror;

     try
     {
       ZApp.Data.CRUD.Upsert(row);
       saveResult = row;
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
