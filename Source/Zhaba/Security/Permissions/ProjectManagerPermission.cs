using NFX.ApplicationModel;
using NFX.Security;
using System;

namespace Zhaba.Security.Permissions
{
  /// <summary>
  /// Project Manager
  /// </summary>
  public class ProjectManagerPermission : ZhabaPermission
  {
    #region CONST
    public const string PATH = "ProjectManager";
    #endregion
    #region .ctor
    public ProjectManagerPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion

    public override string Description
    {
      get { return "Project Manager"; }
    }

    public override bool Check(ISession sessionInstance = null)
    {
      var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
      var user = session.User as ZhabaUser;
      return user.IsProjectManager;
    }
  }
}
