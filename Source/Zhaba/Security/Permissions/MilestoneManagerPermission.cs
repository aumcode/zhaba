
using NFX.ApplicationModel;

namespace Zhaba.Security.Permissions
{
  public sealed class MilestoneManagerPermission :ZhabaPermission
  {
    #region CONST
    public const string PATH = "MilestoneManager";
    #endregion
    #region .ctor
    public MilestoneManagerPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion

    public override bool Check(ISession sessionInstance = null)
    {
      var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
      var user = session.User as ZhabaUser;
      return user.IsMilestoneManager;
    }
  }
}
