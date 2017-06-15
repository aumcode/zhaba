using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX.DataAccess.CRUD;

namespace Zhaba.Data.QueryBuilders
{
  public static class QUser
  {
    public static Query<TRow> GetUserById<TRow>(ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.User.ByID")
      {
        new Query.Param("pID", id)
      };
    }

    public static Query<TRow> GetUserByLogin<TRow>(string login) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.User.ByLogin")
      {
         new Query.Param("login", login)
      };
    }

    public static Query<TRow> AllUserInfos<TRow>() where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.User.AllInfos");
    }
  }
}
