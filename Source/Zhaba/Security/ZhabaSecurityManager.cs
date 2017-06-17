using System;

using NFX;
using NFX.Environment;
using NFX.Security;
using NFX.ServiceModel;

using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;

namespace Zhaba.Security
{
  public sealed class ZhabaSecurityManager : ServiceWithInstrumentationBase<object>, ISecurityManagerImplementation
  {
    public const string CONFIG_PASSWORD_MANAGER_SECTION = "password-manager";

    private IPasswordManagerImplementation m_PasswordManager;

    public IPasswordManager PasswordManager { get { return m_PasswordManager; } }

    public override bool InstrumentationEnabled
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    #region Public
    public void Authenticate(User user)
    {
      var credentials = user.Credentials as IDPasswordCredentials;
      if (credentials != null)
      {
        var u = Authenticate(credentials);
        user.___update_status(u.Status, u.Name, u.Description, u.Rights);
      }
    }

    public User Authenticate(AuthenticationToken token)
    {
      var id = token.Data.AsNullableULong();
      if (!id.HasValue) return ZhabaUser.Invalid;

      return authenticateByCounter(id.Value);
    }

    public User Authenticate(Credentials credentials)
    {
      if (credentials==null) return ZhabaUser.Invalid;

      var idpc = credentials as IDPasswordCredentials;
      if (idpc != null)
        return authenticateByIdPassword(idpc);

      return ZhabaUser.Invalid;
    }

    public AccessLevel Authorize(User user, Permission permission)
    {
      if (user == null)
        throw new SecurityException(StringConsts.ARGUMENT_ERROR + GetType().Name + ".Authorize(user==null)");

      if (user.Status == UserStatus.Invalid)
        return AccessLevel.DeniedFor(user, permission);

      var node = user.Rights.Root.NavigateSection(permission.FullPath);
      return new AccessLevel(user, permission, node);
    }
    #endregion

    #region Protected
    protected override void DoConfigure(IConfigSectionNode node)
    {
      base.DoConfigure(node);
      m_PasswordManager = FactoryUtils.MakeAndConfigure<IPasswordManagerImplementation>(node[CONFIG_PASSWORD_MANAGER_SECTION], typeof(DefaultPasswordManager), new object[] { this });
    }

    protected override void DoStart()
    {
      if (m_PasswordManager != null)
        m_PasswordManager.Start();
    }

    protected override void DoSignalStop()
    {
      if (m_PasswordManager != null)
        m_PasswordManager.SignalStop();
    }

    protected override void DoWaitForCompleteStop()
    {
      if (m_PasswordManager != null)
        m_PasswordManager.WaitForCompleteStop();
    }
    #endregion

    #region .pvt
    private User authenticateByCounter(ulong userId)
    {
      var qry = QUser.GetUserById<UserRow>(userId);
      var userRow = ZApp.Data.CRUD.LoadRow(qry);
      if (userRow == null || !userRow.In_Use) return ZhabaUser.Invalid;

      return new ZhabaUser(
          new ZhabaCounterCredentials(userId),
          new AuthenticationToken(Consts.ZHABA_SECURITY_REALM, userRow.Counter),
          Data.Domains.ZhabaUserStatus.MapStatus(userRow.Status),
          userRow.Login,
          "{0} {1}".Args(userRow.First_Name, userRow.Last_Name),
          Rights.None) { DataRow = userRow };
    }

    private User authenticateByIdPassword(IDPasswordCredentials idpc)
    {
      var userRow = ZApp.Data.Users.GetUser(idpc.ID.ToUpperInvariant());
      if (userRow == null ||
          !userRow.In_Use ||
          !userRow.Login.EqualsOrdIgnoreCase(idpc.ID))
        return ZhabaUser.Invalid;

      var needRehash = true;
      if (!App.SecurityManager.PasswordManager.Verify(idpc.SecurePassword, HashedPassword.FromString(userRow.Password), out needRehash))
        return ZhabaUser.Invalid;

      idpc.Forget();

      return new ZhabaUser(
        idpc,
        new AuthenticationToken(Consts.ZHABA_SECURITY_REALM, userRow.Counter),
        Data.Domains.ZhabaUserStatus.MapStatus(userRow.Status),
        userRow.Login,
        "{0} {1}".Args(userRow.First_Name, userRow.Last_Name),
        Rights.None) { DataRow = userRow };
    }
    #endregion
  }
}
