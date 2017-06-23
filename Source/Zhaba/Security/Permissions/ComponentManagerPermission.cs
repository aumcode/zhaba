using NFX.ApplicationModel;
using NFX.Security;

namespace Zhaba.Security.Permissions
{
  public class ComponentManagerPermission :ZhabaPermission
  {
    #region CONST
    public const string PATH = "ComponentManager";
    #endregion
    #region .ctor
    public ComponentManagerPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion

    public override bool Check(ISession sessionInstance = null)
    {
      var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
      var user = session.User as ZhabaUser;
      return user.IsComponentManager;
    }
  }
}
