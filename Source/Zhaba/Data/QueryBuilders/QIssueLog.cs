using NFX.DataAccess.CRUD;

namespace Zhaba.Data.QueryBuilders
{
  public static class QIssueLog
  {
    public static Query<TRow> findLastIssueLogByIssue<TRow>(ulong C_Issue) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueLog.FindLastByIssue")
      {
        new Query.Param("C_Issue", C_Issue)
      };
    }
  }
}
