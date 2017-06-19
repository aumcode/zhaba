using NFX.ApplicationModel;

namespace Zhaba.Security.Permissions
{
  /// <summary>
  /// Denotes system user that can manage other users
  /// </summary>
  public sealed class UserManagerPermission : ZhabaPermission
  {
    public UserManagerPermission() : base(NFX.Security.AccessLevel.VIEW) { }

    public override string Description
    {
      get { return "Denotes system users that can manage other users"; }
    }

    public override bool Check(ISession sessionInstance = null)
    {
      var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
      var user = session.User;

      return user.Status >= NFX.Security.UserStatus.System;
    }
  }
}
