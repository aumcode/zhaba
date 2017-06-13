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
  }
}
