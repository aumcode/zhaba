using System;
using NFX.ApplicationModel;
using NFX.Security;

namespace Zhaba.Security.Permissions
{
  /// <summary>
  /// Acces for edit category
  /// </summary>
  public sealed class CategoryEditPermission : ZhabaPermission
  {
    #region CONST
    public const string PATH = "CategoryEdit";
    #endregion
    #region .ctor
    public CategoryEditPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion

    public override bool Check(ISession sessionInstance = null)
    {
      var session = sessionInstance ?? ExecutionContext.Session ?? NOPSession.Instance;
      var user = session.User as ZhabaUser;
      return user.IsCategoryEdit;
    }

  }


}
