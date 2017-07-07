using NFX.DataAccess.CRUD;

namespace Zhaba.Data.QueryBuilders
{
  public static class QIssueLog
  {
    public static Query<TRow> FindLastIssueLogByIssue<TRow>(ulong C_Issue) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueLog.FindLastByIssue")
      {
        new Query.Param("C_Issue", C_Issue)
      };
    }

    public static Query<TRow> FindMilestoneByIssue<TRow>(ulong C_Issue) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueLog.FindMilestoneByIssue")
      {
        new Query.Param("C_Issue", C_Issue)
      };
    }

    public static Query<TRow> findAllByIssue<TRow>(ulong c_Issue) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueLog.findAllByIssue")
      {
        new Query.Param("pIssue", c_Issue)
      };
    }
  }
}
