using System;
using System.Collections.Generic;
using NFX;
using NFX.DataAccess.CRUD;
using NFX.Log;
using Zhaba.Data;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Reports;

namespace Zhaba.DataLogic
{
  internal class ZhabaReportsLogic : LogicBase, IReportLogic
  {
    #region .ctor
    public ZhabaReportsLogic(ZhabaDataStore store) : base(store)
    {
    }
    #endregion

    #region public
    public Exception DueItemReport(DueItemsReport report, out object saveResult)
    {
      Exception result = null;
      saveResult = null;
      try
      {
        using (var trn = ZApp.Data.CRUD.BeginTransaction())
        {
          var asOf = (report.AsOf ?? App.TimeSource.UTCNow.Date).AddHours(23).AddMinutes(59).AddSeconds(59); 
          
          var query = QReports.DueItems.CountIssueByProject<DueItemsReport.Statistic>(report.C_Project, asOf);
          saveResult = trn.LoadEnumerable<DueItemsReport.Statistic>(query);
          var rows = saveResult as IEnumerable<DueItemsReport.Statistic>;
          foreach (var row in rows)
          {
            var queryCount = QReports.DueItems.CountStatusIssueByProject<DueItemsReport.IssueStatus>(row.C_Project, asOf);
            row.DetailIssueCount = trn.LoadEnumerable<DueItemsReport.IssueStatus>(queryCount);
            var queryDetails = QReports.DueItems.IssueDetails<DueItemsReport.IssueDetails>(row.C_Project, asOf);
            row.IssueDetals = trn.LoadEnumerable<DueItemsReport.IssueDetails>(queryDetails);
          }
        }
      }
      catch (Exception ex)
      {
        App.Log.Write(new Message(ex.Message));
        result = ex;
      }
      
      return result;
    }
    #endregion
    
  }
}