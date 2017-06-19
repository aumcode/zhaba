using System;
using System.Collections.Generic;

using NFX;

using Zhaba.Data;
using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.DataLogic
{
  internal class ZhabaUserLogic : LogicBase, IUserLogic
  {
    public ZhabaUserLogic(ZhabaDataStore dataStore) : base(dataStore) { }

    public UserRow GetUser(string login)
    {
      if (login.IsNullOrWhiteSpace())
        throw new ZhabaDataException("UserLogic.GetUser(login=null|empty)");

      var qry = QUser.GetUserByLogin<UserRow>(login);
      return Store.CRUD.LoadRow(qry);
    }

    public UserRow GetUserByToken(NFX.Security.AuthenticationToken token)
    {
      var counter = token.Data.AsNullableULong();
      if (!counter.HasValue)
        throw new ZhabaDataException("UserLogic.GetUserByToken(invalid token)");

      var qry = QUser.GetUserById<UserRow>(counter.Value);
      var userRow = ZApp.Data.CRUD.LoadRow(qry);
      return userRow;
    }

    public NFX.Security.AuthenticationToken CreateToken(UserRow user)
    {
      return new NFX.Security.AuthenticationToken(Consts.ZHABA_SECURITY_REALM, user.Counter);
    }
  }
}
