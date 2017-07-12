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

    public static Query<TRow> FindIssueAssignByIssue<TRow>(ulong C_Issue, DateTime dateUTC) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Task.IssueAssign")
      {
        new Query.Param("C_Issue", C_Issue),
        new Query.Param("dateUTC", dateUTC)
      };
    }

    public static Query<TRow> FindAllAreaByIssue<TRow>(ulong c_Issue) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Task.Areas")
      {
        new Query.Param("pIssue", c_Issue)
      };
    }

    public static Query<TRow> FindAllComponentByIssue<TRow>(ulong c_Issue) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Task.Components")
      {
        new Query.Param("pIssue", c_Issue)
      };
    }

    public static Query<TRow> FindAllAssignee<TRow>(ulong C_Issue, DateTime dateUTC) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Task.AssigneeList")
      {
        new Query.Param("pIssue", C_Issue),
        new Query.Param("dateUTC", dateUTC)
      };
    }
  }
}
