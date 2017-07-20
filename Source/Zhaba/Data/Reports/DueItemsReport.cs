using System;
using System.Collections.Generic;
using NFX;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using Zhaba.Data.Domains;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;

namespace Zhaba.Data.Reports
{
  public class DueItemsReport : ZhabaReportForm
  {
    
    #region Nested

    public class IssueStatus : TypedRow
    {
      private string m_Status;

      [Field]
      public string Status
      {
        get { return ZhabaIssueStatus.MapDescription(m_Status); }
        set { m_Status = value; } 
        
      }
      [Field] public int Count { get; set; }
    }

    public class IssueDetails : TypedRow
    {
      private string m_Status;
      
      [Field] public ulong Counter { get; set; }
      [Field] public string Name { get; set; }
      [Field]
      public string Status
      {
        get { return ZhabaIssueStatus.MapDescription(m_Status); }
        set { m_Status = value; } 
        
      }
      [Field] public DateTime? Status_Date { get; set; }
      [Field] public string Description { get; set; }
      [Field] public DateTime? Start_Date { get; set; }
      [Field] public int Completness { get; set; }
      [Field] public DateTime? Due_Date { get; set; }
      [Field] public string Operator { get; set; }
      [Field] public string Category_Name { get; set; }
      [Field] public ulong C_Project { get; set; }
      [Field] public string Project_Name { get; set; }
      [Field] public string Assignee { get; set; }
      [Field] public int Priority { get; set; }
      [Field] public DateTime? Complete_Date { get; set; }
      [Field] public int Remaining { get; set; }
    }
    
    public class Statistic : TypedRow
    {
      [Field] public ulong C_Project { get; set; }
      [Field] public string Name { get; set; }
      [Field] public int IssueCount { get; set; }
      [Field] public IEnumerable<IssueStatus> DetailIssueCount { get; set; }
      [Field] public IEnumerable<IssueDetails> IssueDetals { get; set; }
    }
    
    #endregion

    [Field(required:true,
           kind: DataKind.DateTime, 
           description: "As Of",
           metadata: @"Placeholder='As Of'")]
    public DateTime? AsOf { get; set; } = App.TimeSource.UTCNow.Date;
    
    [Field(required:false,
           description: "Project",
           metadata: @"Placeholder='Project'")] 
    public ulong? C_Project { get; set; }


    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      JSONDataMap result = null;
      var project = fdef.Name.EqualsIgnoreCase("C_Project");
      if (project)
      {
        var projects = ZApp.Data.CRUD.LoadEnumerable<ProjectRow>(QProject.AllProjects<ProjectRow>());
        result = new JSONDataMap();
        foreach (var item in projects)
        {
          result.Add(item.Counter.ToString(), item.Name);
        }
      }
      return result;
    }
    
    protected override Exception DoSave(out object saveResult)
    {
      return ZApp.Data.ReportLogic.DueItemReport(this, out saveResult);
    }
    
  }
}