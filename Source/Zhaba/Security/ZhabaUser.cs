using System;

using NFX;
using NFX.Security;
using Zhaba.Security.Permissions;

namespace Zhaba.Security
{
  [Serializable]
  public sealed class ZhabaUser : User
  {
    #region .ctor
    public ZhabaUser(Credentials credentials,
                     AuthenticationToken token,
                     UserStatus status,
                     string name,
                     string descr,
                     Rights rights) : base(credentials, token, status, name, descr, rights)
    {
    }

    public ZhabaUser(Credentials credentials,
                     AuthenticationToken token,
                     string name,
                     Rights rights) : this(credentials, token, UserStatus.User, name, null, rights)
    {
    }
    #endregion

    public static readonly ZhabaUser Invalid =
      new ZhabaUser(new IDPasswordCredentials("", ""),
                    new AuthenticationToken("", ""),
                    UserStatus.Invalid, "Invalid", "Invalid", Rights.None);

    /// <summary>
    /// References user data. Be carefull modifying this data directly as it contains a row instance wich is not thread-safe
    /// </summary>
    public Data.Rows.UserRow DataRow { get; set; }

    public bool IsAdmin
    {
      get
      {
        return Status > UserStatus.User;
      }
    }

    public bool IsSystem
    {
      get
      {
        return Status == UserStatus.System;
      }
    }
  }
}
