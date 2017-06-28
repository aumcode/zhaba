using NFX.DataAccess.CRUD;
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
  }
}
