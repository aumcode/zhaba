using System;
using NFX.ApplicationModel;
using NFX.Security;

namespace Zhaba.Security.Permissions
{
  /// <summary>
  /// Acces for edit category
  /// </summary>
  public sealed class AdminPermission : ZhabaPermission
  {
    #region .ctor
    public AdminPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion
    public override string Description
    {
      get { return "Administrator"; }
    }

    public override bool Check(ISession sessionInstance = null)
    {
      var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
      var user = session.User;
      if (user.Status == UserStatus.System ||
          user.Status == UserStatus.Administrator) return true;
      return base.Check(sessionInstance);
    }
  }


}
