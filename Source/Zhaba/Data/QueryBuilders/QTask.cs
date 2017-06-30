using NFX.DataAccess.CRUD;
using System;
using Zhaba.Data.Filters;

namespace Zhaba.Data.QueryBuilders
{
  public static class QTask
  {
    public static Query<TRow> TasksByFilter<TRow>(TaskListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.TaskList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    public static Query<TRow> FindFirst5IssueLogByIssue<TRow>(ulong C_Issue, DateTime dateUTC) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Task.IssueLog")
      {
        new Query.Param("C_Issue", C_Issue),
        new Query.Param("dateUTC", dateUTC)
      };
    }
  }
}
