using System;
using NFX.DataAccess.CRUD;

namespace Zhaba.Data.QueryBuilders
{
  public static class QReports
  {
    
    public static class DueItems
    {
      public static Query<TRow> CountIssueByProject<TRow>(ulong? c_Project, DateTime asOf) where TRow : Row
      {
        return new Query<TRow>("SQL.Reports.CountIssueByProject")
        {
          new Query.Param("pProject", c_Project),
          new Query.Param("pAsOf", asOf)
        };
      }

      public static Query<TRow> CountStatusIssueByProject<TRow>(ulong? c_Project, DateTime asOf) where TRow : Row
      {
        return new Query<TRow>("SQL.Reports.CountStatusIssueByProject")
        {
          new Query.Param("pProject", c_Project),
          new Query.Param("pAsOf", asOf)
        };
      }

      public static Query<TRow> IssueDetails<TRow>(ulong c_Project, DateTime asOf) where TRow : Row
      {
        return new Query<TRow>("SQL.Reports.IssueDetails")
        {
          new Query.Param("pProject", c_Project),
          new Query.Param("pAsOf", asOf)
        };
      }
      
    }
    
  }
}