using System;

using NFX;
using NFX.Security;
using Zhaba.Security.Permissions;

namespace Zhaba.Security
{
  [Serializable]
  public sealed class ZhabaUser : User
  {
    #region .ctor
    public ZhabaUser(Credentials credentials,
                     AuthenticationToken token,
                     UserStatus status,
                     string name,
                     string descr,
                     Rights rights) : base(credentials, token, status, name, descr, rights)
    {
    }

    public ZhabaUser(Credentials credentials,
                     AuthenticationToken token,
                     string name,
                     Rights rights) : this(credentials, token, UserStatus.User, name, null, rights)
    {
    }
    #endregion

    public static readonly ZhabaUser Invalid =
      new ZhabaUser(new IDPasswordCredentials("", ""),
                    new AuthenticationToken("", ""),
                    UserStatus.Invalid, "Invalid", "Invalid", Rights.None);

    /// <summary>
    /// References user data. Be carefull modifying this data directly as it contains a row instance wich is not thread-safe
    /// </summary>
    public Data.Rows.UserRow DataRow { get; set; }

    public bool IsAdmin
    {
      get
      {
        return Status > UserStatus.User;
      }
    }

    public bool IsSystem
    {
      get
      {
        return Status == UserStatus.System;
      }
    }

    public bool IsProjectManager
    {
      get
      {
        var node = Rights.Root.NavigateSection('/'+ProjectManagerPermission.PATH);
        return (node.RootPath == '/' + ProjectManagerPermission.PATH) || IsAdmin;
      }
    }

    public bool IsCategoryEdit 
    { 
      get
      {
        var node = Rights.Root.NavigateSection('/' + CategoryEditPermission.PATH);
        return (node.RootPath == '/' + CategoryEditPermission.PATH) || IsAdmin;
      }
    }

    public bool IsAreaManager
    {
      get
      {
        var node = Rights.Root.NavigateSection('/' + AreaManagerPermission.PATH);
        return (node.RootPath == '/' + AreaManagerPermission.PATH) || IsAdmin || IsProjectManager;
      }
    }

    public bool IsComponentManager
    {
      get
      {
        var node = Rights.Root.NavigateSection('/' + ComponentManagerPermission.PATH);
        return (node.RootPath == '/' + ComponentManagerPermission.PATH) || IsAdmin || IsProjectManager;
      }
    }

    public bool IsMilestoneManager
    {
      get
      {
        var node = Rights.Root.NavigateSection('/' + MilestoneManagerPermission.PATH);
        return (node.RootPath == '/' + MilestoneManagerPermission.PATH) || IsAdmin || IsProjectManager;
      }
    }

    public bool IsIssueManager
    { 
      get 
      {
        var node = Rights.Root.NavigateSection('/' + IssueManagerPermission.PATH);
        return (node.RootPath == '/' + IssueManagerPermission.PATH) || IsAdmin || IsProjectManager;
      }
    }
  }
}
