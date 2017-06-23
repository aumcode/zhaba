using NFX.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaba.Security.Permissions
{
  public class IssueManagerPermission : ZhabaPermission
  {
    #region CONST
    public const string PATH = "IssueManager";
    #endregion
    #region .ctor
    public IssueManagerPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion

    public override bool Check(ISession sessionInstance = null)
    {
      var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
      var user = session.User as ZhabaUser;
      return user.IsIssueManager;
    }
  }
}
