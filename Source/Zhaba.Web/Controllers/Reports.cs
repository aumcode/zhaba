using System;
using System.Collections.Generic;
using System.Dynamic;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
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
    public object dueitems(DueItemsReport report)
    {
      return DateSetup<DueItemsReport, DueItemsReport.Statistic, DueItemsReportPage, DueItemsReportFormPage>(new object[] {report}, report);
    }



    #region .pvt

    private object DateSetup<TReport, TData, TReportPage, TFormPage>(object[] args, TReport report)
    where TReport : ZhabaReportForm
    where TReportPage : ZhabaPage
    where TFormPage : ZhabaPage
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
    

    #endregion
  }
}