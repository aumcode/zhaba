using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX.ApplicationModel;
using NFX.Environment;
using NFX.Security;
using NFX;

using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;

namespace Zhaba.Security
{
  public sealed class ZhabaSecurityManager : ApplicationComponent, ISecurityManagerImplementation
  {
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

      return authenticateByID(id.Value);
    }

    public User Authenticate(Credentials credentials)
    {
      if (credentials==null) return ZhabaUser.Invalid;

      if (credentials is IDPasswordCredentials)
      {
        var idPwdCredentials = credentials as IDPasswordCredentials;
        return authenticateByIdPassword(idPwdCredentials.ID, idPwdCredentials.Password);
      }

      if (credentials is ULongIDCredentials)
      {
        var idCredentials = credentials as ULongIDCredentials;
        return authenticateByID(idCredentials.ID);
      }

      return ZhabaUser.Invalid;
    }

    public AccessLevel Authorize(User user, Permission permission)
    {
      IConfigSectionNode ACCESS_CONF_USER =  "p{level=1}".AsLaconicConfig();
      IConfigSectionNode ACCESS_CONF_ADMIN = "p{level=100}".AsLaconicConfig();

      if (user == null || user.Status == UserStatus.Invalid)
        throw new AuthorizationException();

      IDPasswordCredentials credentials = user.Credentials as IDPasswordCredentials;
      if (credentials == null)
        throw new AuthorizationException();

      IConfigSectionNode confNode = (user.Status == UserStatus.Admin ?
        ACCESS_CONF_ADMIN : ACCESS_CONF_USER);

      return new AccessLevel(user, permission, confNode);
    }

    public void Configure(IConfigSectionNode node)
    {
      ConfigAttribute.Apply(this, node);
    }


    #region .pvt

    private User authenticateByID(ulong userId)
    {
      var qry = QUser.SecurityLoadByID<UserRow>(userId);
      var userRow = ZApp.Data.CRUD.LoadRow(qry);
      if (userRow == null) return ZhabaUser.Invalid;

      return new ZhabaUser(
          new ULongIDCredentials(userId),
          new AuthenticationToken(Consts.ZHABA_SECURITY_REALM, userRow.Counter),
          userRow.Role.EqualsIgnoreCase("ADMIN") ? UserStatus.Admin : UserStatus.User,
          userRow.Login,
          "{0} {1}".Args(userRow.First_Name, userRow.Last_Name),
          Rights.None) { DataRow = userRow };
    }

    private User authenticateByIdPassword(string login, string password)
    {
      var userRow = ZApp.Data.Users.GetUser(login.ToUpperInvariant());
      if (userRow == null ||
          userRow.Login.IsNullOrWhiteSpace() ||
          !userRow.Login.EqualsIgnoreCase(login))
        return ZhabaUser.Invalid;

      var passwordHash = SecurityUtils.HashUserPassword(password, userRow.Password_Salt);
      if (passwordHash != userRow.Password_Hash)
        return ZhabaUser.Invalid;

      return new ZhabaUser(
        new IDPasswordCredentials(userRow.Login, password),
        new AuthenticationToken(Consts.ZHABA_SECURITY_REALM, userRow.Counter),
        userRow.Role.EqualsIgnoreCase("ADMIN") ? UserStatus.Admin : UserStatus.User,
        userRow.Login,
        "{0} {1}".Args(userRow.First_Name, userRow.Last_Name),
        Rights.None) { DataRow = userRow };
    }

    #endregion
  }
}
