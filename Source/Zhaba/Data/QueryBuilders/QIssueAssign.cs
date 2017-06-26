using NFX.DataAccess.CRUD;
using System;

namespace Zhaba.Data.QueryBuilders
{
  public class QIssueAssign
  {
    public static Query<TRow> findIssueAssigneeByIssue<TRow>(ulong C_Issue) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueAssign.FindByIssue")
      {
        new Query.Param("C_Issue", C_Issue)
      };
    }

    public static Query<TRow> findIssueAssignByUser<TRow>(ulong C_User) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueAssign.FindByUser")
      {
        new Query.Param("C_User", C_User)
      };
    }

    public static Query<TRow> findIssueAssignByIssueAndUser<TRow>(ulong C_Issue, ulong C_User) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueAssign.FindByIssueAndUser")
      {
        new Query.Param("C_Issue", C_Issue),
        new Query.Param("C_User", C_User)
      };
    }

    public static Query<TRow> findIssueAssignByIssueAndUserAndDate<TRow>(ulong C_Issue, ulong C_User, DateTime date) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueAssign.FindByIssueAndUserAndDate")
      {
        new Query.Param("C_Issue", C_Issue),
        new Query.Param("C_User", C_User),
        new Query.Param("date", date)
      };
    }

    public static Query<TRow> findIssueAssignByCounter<TRow>(ulong Counter) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueAssign.FindByCounter")
      {
        new Query.Param("Counter", Counter)
      };
    }
  }
}
