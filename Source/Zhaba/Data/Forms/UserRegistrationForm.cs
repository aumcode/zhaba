using System;
using System.Text;
using System.Security.Cryptography;

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
  public class UserRegistrationForm : ZhabaForm
  {
    public static readonly string PASSWORD_SESSION_VAR = typeof(UserRegistrationForm).FullName + "-PASSWORD";
    public static readonly string PASSWORD_FAKE = "_*-~!@#$%^'&*()_+`}{|<>?";

    public UserRegistrationForm()
    {
      FormMode = FormMode.Insert; // Always Insert
    }

    public UserRegistrationForm(ulong? id)
    {
        if(id.HasValue)
        {
          FormMode = FormMode.Edit;
          var qry = QCommon.ProjectByID<UserRow>(id.Value);
          var row = ZApp.Data.CRUD.LoadRow(qry);
          if (row != null)
            row.CopyFields(this);
          else
            throw HTTPStatusException.NotFound_404("Project");

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
    
    [Field(typeof(UserRow), "Password", minLength: Sizes.PASSWORD_MIN_LEN, maxLength: Sizes.PASSWORD_MAX_LEN)]
    public string Password { get; set; }
    
    [Field(required: true,
           minLength: Sizes.PASSWORD_MIN_LEN,
           maxLength: Sizes.PASSWORD_MAX_LEN,
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

     var row = new UserRow(RowPKAction.Default);
     CopyFields(row);

     var typedPassword = ZhabaSession[PASSWORD_SESSION_VAR].AsString();
     if (typedPassword.IsNullOrWhiteSpace())
       return new CRUDFieldValidationException(this, "Password", "Password required");

      using (var password = IDPasswordCredentials.PlainPasswordToSecureBuffer(typedPassword))
        row.Password = App.SecurityManager.PasswordManager.ComputeHash(PasswordFamily.Text, password).ToString();

     // fill user rights with empty config as default
/*     var rightsCfg = new NFX.Environment.MemoryConfiguration();
     rightsCfg.Create();
     rightsCfg.Root.AddChildNode(NFX.Security.Rights.CONFIG_ROOT_SECTION);
     row.User_Rights = rightsCfg.ToLaconicString(NFX.CodeAnalysis.Laconfig.LaconfigWritingOptions.Compact);*/

     row.User_Rights = "{z:{rights:{}}}";

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
