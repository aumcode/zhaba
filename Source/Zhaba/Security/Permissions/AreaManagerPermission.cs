using NFX.ApplicationModel;
using NFX.Security;
using NFX.Wave.Templatization;

namespace Zhaba.Security.Permissions
{
  public sealed class AreaManagerPermission :ZhabaPermission
  {
    #region CONST
    public const string PATH = "AreaManager";
    #endregion
    #region .ctor
    public AreaManagerPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion

    public override bool Check(ISession sessionInstance = null)
    {
      var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
      var user = session.User as ZhabaUser;
      return user.IsAreaManager;
    }
  }
}
