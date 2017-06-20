using System;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.Security;
using NFX.Serialization.JSON;
using Zhaba.Data.Domains;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_user")]
  public class UserRow : ZhabaRowWithPKAndInUse
  {
    #region .ctor
      public UserRow() : base() { }
      public UserRow(RowPKAction action) : base(action) { }
    #endregion

    #region Fields
      private string m_UserRights;
      [NonSerialized] private Rights m_CachedRights;
        #endregion

      #region Properties
      [Field(required: true,
             minLength: ZhabaUserLogin.MIN_LEN,
             maxLength: ZhabaUserLogin.MAX_LEN,
             metadata: @"Placeholder='User Login' Hint='Enter your login'")]
      public string Login { get; set; }

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

      [Field(required: true,
             maxLength: ZhabaUserStatus.MAX_LEN,
             valueList: ZhabaUserStatus.VALUE_LIST)]
      public string Status { get; set; }

      [Field(required: true,
             maxLength: Domains.ZhabaPasswordHash.MAX_LEN,
             description: "Password",
             metadata: "Placeholder='Password' Password=true Hint='User password'")]
      public string Password { get; set; }

      [Field(required: true, nonUI: true, maxLength: Domains.ZhabaSecurityRights.MAX_LEN)]
      public string User_Rights
        {
          get { return m_UserRights; }
          set
          {
            m_UserRights = value;
            m_CachedRights = null;
          }
        }

      /// <summary>
      /// Returns parsed user Rights or Rights.None if value is not set
      /// </summary>
      public Rights Rights
      {
        get
        {
          if (m_CachedRights != null) return m_CachedRights;

          try
          {
            m_CachedRights = Domains.ZhabaSecurityRights.MapToRights(this.User_Rights);
            return m_CachedRights;
          }
          catch(Exception error)
          {
            var errRights = new CRUDFieldValidationException(Schema.Name, "User_Rights", "User '{0}' wrong rights: {1}".Args(Login, error.ToMessageWithType()), error);
            App.Log.Write(new NFX.Log.Message
            {
              Type = NFX.Log.MessageType.Error,
              Topic = CoreConsts.SECURITY_CATEGORY,
              From = "{0}.Rights.get()".Args(GetType().Name),
              Text = errRights.ToMessageWithType(),
              Exception = errRights,
              Parameters = User_Rights
            });
            return Rights.None;
          }
        }

        set
        {
          if (value == null) value = Rights.None;
          m_UserRights = Domains.ZhabaSecurityRights.MapToValue(value);
          m_CachedRights = value;
        }
      }
    #endregion

    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error!=null) return error;

      try
      {
        m_CachedRights = Domains.ZhabaSecurityRights.MapToRights(this.User_Rights);
      }
      catch(Exception errRights)
      {
        return new CRUDFieldValidationException(Schema.Name, "User_Rights", "User '{0}' wrong rights: {1}".Args(Login, errRights.ToMessageWithType()), errRights);
      }

      return null;
    }
  }
}
