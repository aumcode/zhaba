using NFX.ApplicationModel;
using NFX.Security;
using System;

namespace Zhaba.Security.Permissions
{
  /// <summary>
  /// Project Manager
  /// </summary>
  public class MeetingHostPermission : ZhabaPermission
  {
    #region .ctor
    public MeetingHostPermission() : base(NFX.Security.AccessLevel.VIEW) { }
    #endregion

    public override string Description
    {
      get { return "Meeting Host"; }
    }

  }
}
