using System;
using System.Collections.Generic;
using System.Dynamic;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using NFX.Wave;
using NFX.Wave.MVC;
using Zhaba.Data.Forms;
using Zhaba.Data.Reports;
using Zhaba.Security.Permissions;
using Zhaba.Web.Pages;
using Zhaba.Web.Pages.Reports;

namespace Zhaba.Web.Controllers
{
  [SiteUserPermission]
  public class Reports : ZhabaController
  {
    [Action]
    public object DueItems(DueItemsReport report)
    {
      return DateSetup<DueItemsReport, DueItemsReport.Statistic, DueItemsReportPage, DueItemsReportFormPage>(report);
    }
    
    [Action("dueitemsview", 0, "match { methods=GET}")]
    public object DueItemsView(DateTime? asOf, ulong? C_Project)
    {
      var report = new DueItemsReport() {AsOf = asOf, C_Project = C_Project};
      return DateSetupView<DueItemsReport, DueItemsReport.Statistic, DueItemsReportPage>(report);
    }

    #region .pvt

    private object DateSetup<TReport, TData, TReportPage, TFormPage>(TReport report)
    where TReport : ZhabaReportForm
    where TReportPage : ZhabaPage
    where TFormPage : ZhabaPage
    where TData : Row
    {
      Exception error = null;
      if (WorkContext.IsPOST)
      {
        IEnumerable<TData> rows;
        error = report.Save(out rows);
        if (error == null)
        {
          if (WorkContext.RequestedJSON) return new JSONResult(rows, new JSONWritingOptions { RowsAsMap = true, RowsetMetadata = true });;
          return MakePage<TReportPage>(report, rows);
        }
      }
      else
        report = (TReport) Activator.CreateInstance(typeof(TReport), new object[]{});
      if (WorkContext.RequestedJSON) return new ClientRecord(report, error);
      return MakePage<TFormPage>(report, error);
    }

    private object DateSetupView<TReport, TData, TReportPage>(TReport report)
      where TReport : ZhabaReportForm
      where TReportPage : ZhabaPage
      where TData : Row
    {
      Exception error = null;
      if (WorkContext.IsGET)
      {
        IEnumerable<TData> rows;
        error = report.Save(out rows);
        if (error == null)
        {
          if (WorkContext.RequestedJSON) return new JSONResult(rows, new JSONWritingOptions { RowsAsMap = true, RowsetMetadata = true });;
          return MakePage<TReportPage>(report, rows);
        }        
      }
      if (WorkContext.RequestedJSON) return new ClientRecord(report, error);
      return MakePage<TReportPage>(report, error);
    }
    

    #endregion
  }
}