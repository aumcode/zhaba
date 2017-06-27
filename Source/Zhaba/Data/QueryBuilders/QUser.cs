using NFX.DataAccess.CRUD;
using System;

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

    public static Query<TRow> FindAllActiveUser<TRow>() where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.User.FindAllActiveUser");
    }

    public static Query<TRow> FindAllActiveUserAndNotAssignedOnDate<TRow>(ulong C_Issue, DateTime DateUTC) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.User.FindAllActiveUserAndNotAssignedOnDate") 
      {
        new Query.Param("C_Issue", C_Issue),
        new Query.Param("DateUTC", DateUTC)
      };
    }
  }
}
