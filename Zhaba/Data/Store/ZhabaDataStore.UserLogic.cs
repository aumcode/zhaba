using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.ServiceModel;

using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Store
{
  public sealed partial class ZhabaDataStore : ServiceWithInstrumentationBase<object>, IZhabaDataStore, IDataStoreImplementation
  {
    private class UserLogic : LogicBase, IUserLogic
    {
      public UserLogic(ZhabaDataStore dataStore) : base(dataStore) { }

      public UserRow GetUser(string login)
      {
        if (login.IsNullOrWhiteSpace())
          throw new ZhabaDataException("UserLogic.GetUser(login=null|empty)");

        var qry = QUser.GetUserByLogin<UserRow>(login);
        return Store.CRUD.LoadRow(qry);
      }
    }
  }
}
