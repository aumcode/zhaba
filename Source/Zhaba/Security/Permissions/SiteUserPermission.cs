using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX.Security;
using NFX.ApplicationModel;

namespace Zhaba.Security.Permissions
{
  /// <summary>
  /// Denotes regular Zhaba site registered user
  /// </summary>
  public sealed class SiteUserPermission : ZhabaPermission
  {
     public SiteUserPermission() : base(NFX.Security.AccessLevel.VIEW) { }

     public override string Description
     {
         get { return "Denotes regular Zhaba site registered user"; }
     }

     public override bool Check(ISession sessionInstance = null)
     {
        var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
        var user = session.User;

        return user.IsAuthenticated;
     }
  }
}
