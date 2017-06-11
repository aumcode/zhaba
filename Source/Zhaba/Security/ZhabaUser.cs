using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFX.Security;

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

    public static readonly ZhabaUser Invalid = new ZhabaUser(new IDPasswordCredentials("", ""),
      new AuthenticationToken("", ""),
      UserStatus.Invalid, "Invalid", "Invalid", Rights.None);

    public Data.Rows.UserRow DataRow { get; set; }

    public bool IsAdmin
    {
      get
      {
        return Status == UserStatus.Admin;
      }
    }
  }
}
