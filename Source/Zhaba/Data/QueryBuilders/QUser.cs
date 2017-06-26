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

    public static Query<TRow> findAllActiveUser<TRow>() where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.User.FindAllActiveUser");
    }
  }
}
