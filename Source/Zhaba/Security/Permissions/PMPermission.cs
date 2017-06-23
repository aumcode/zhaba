using NFX.ApplicationModel;
using NFX.Security;
using System;

namespace Zhaba.Security.Permissions
{
  /// <summary>
  /// Project Manager
  /// </summary>
  public class PMPermission : ZhabaPermission
  {
    #region .ctor
    public PMPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion

    public override string Description
    {
      get { return "Project Manager"; }
    }

  }
}
