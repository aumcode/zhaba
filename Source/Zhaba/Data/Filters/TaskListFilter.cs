using System;
using System.Globalization;
using NFX;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using Zhaba.Data.Domains;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using System.Collections.Generic;
using System.Linq;

namespace Zhaba.Data.Filters
{
  public class TaskListFilter : ZhabaFilterForm
  {
    #region Nested

    public class IssueAssignListFilterRow : IssueAssignRow
    {
      [Field]
      public string UserLogin { get; set; }

      [Field]
      public string OperatorOpenLogin { get; set; }

      [Field]
      public string OperatorCloseLogin { get; set; }
      
      [Field]
      public string UserFirstName { get; set; } 

      [Field]
      public string UserLastName { get; set; } 
      
      [Field]
      public string OperatorOpenFirstName { get; set; } 

      [Field]
      public string OperatorOpenLastName { get; set; } 

      [Field]
      public string OperatorCloseFirstName { get; set; } 

      [Field]
      public string OperatorCloseLastName { get; set; } 

    }

    public class TaskListFilterRow : TypedRow
    {
      [Field]
      public string statusId { get; set; }

      [Field]
      public ulong Counter { get; set; }
      
      [Field]
      public int Priority { get; set; }

      [Field]
      public string Name { get; set; }

      [Field]
      public string Category_Name { get; set; }

      [Field]
      public string Description { get; set; }

      [Field]
      public string Status
      {
        get { return ZhabaIssueStatus.MapDescription(statusId); }
        set { statusId = value; }
      }

      [Field]
      public DateTime? Complete_Date { get; set; }

      [Field]
      public DateTime? Start_Date { get; set; }

      [Field]
      public DateTime? Due_Date { get; set; }

      [Field]
      public int Completeness { get; set; }

      [Field]
      public int C_Project { get; set; }

      [Field]
      public string ProjectName { get; set; }

      [Field]
      public string Assignee { get; set; }
      
      [Field]
      public IEnumerable<IssueAssignListFilterRow> AssigneeList { get; set; }
      
      [Field]
      public IEnumerable<TaskListFilterRow> Details { get; set; }

      [Field]
      public IEnumerable<IssueAssignListFilterRow> Assignments { get; set; }
      
      [Field]
      public IEnumerable<AreaRow> Areas { get; set; }
      
      [Field]
      public IEnumerable<ComponentRow> Components { get; set; }
        
      [Field]
      public string[] NextState { get { return ZhabaIssueStatus.NextState(statusId); } }

      [Field]
      public int Remaining { get; set; }

      [Field]
      public bool HasAssignee { get { return AssigneeList != null && AssigneeList.Any(); } }
    }

    #endregion

    [Field(metadata: "Description='As of' Placeholder='As of' Hint='As of'")]
    public string AsOf { get; set; } = App.TimeSource.UTCNow.Date.ToString("d", CultureInfo.InvariantCulture);
    
    [Field(metadata: "Description='Search' Placeholder='Type n=*, a=*, c=* for search. Example: n=Issue1 a=UI' Hint='Search'")]
    public string Search { get; set; }

    [Field(metadata: "Description='Assign' Placeholder='Assign' Hint='Assign'")]
    public ulong? C_USER { get; set; }

    [Field(metadata: "Description='Category' Placeholder='Category' Hint='Category'")]
    public string CategoryName { get; set; }

    [Field(metadata: "Description='Project name' Placeholder='Project name' Hint='Project name'")]
    public string ProjectName { get; set; }

    [Field(valueList: "1:Day, 2:2 Days, 7:7 Days, 14: 14 Days, 30:Month, 60:Quarter, 365:Year", metadata: "Description='Due' Hint='Due'")]
    public string Due { get; set; }
    
    [Field(valueList: "N:NEW, R:REOPEN, A:ASSIGNED, D:DONE, F:DEFER, X:CANCELED, C:CLOSED", metadata: "Description='Status' Hint='Status'")]
    public string Status { get; set; }

    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      var result = new JSONDataMap();
      if (fdef.Name.EqualsIgnoreCase("C_USER"))
      {
        var users = ZApp.Data.CRUD.LoadEnumerable(QUser.FindAllActiveUser<UserRow>());
        foreach (var user in users)
          result.Add(user.Counter.ToString(), user.First_Name);
      }

      if (fdef.Name.EqualsIgnoreCase("ProjectName"))
      {
        var projects = ZApp.Data.CRUD.LoadEnumerable(QProject.AllProjects<ProjectRow>());
        foreach (var project in projects)
          result.Add(project.Name, project.Name);
      }

      if (fdef.Name.EqualsIgnoreCase("CategoryName"))
      {
        var categories = ZApp.Data.CRUD.LoadEnumerable(QCategory.findCategoryByFilter<CategoryRow>(new CategoryListFilter()));
        foreach (var category in categories)
          result.Add(category.Name, category.Name);
      }
      return result;
    }

    protected override Exception DoSave(out object saveResult)
    {
      Exception result = null;
      using (var trn = ZApp.Data.CRUD.BeginTransaction() )
      {
        var qry = QTask.TasksByFilter<TaskListFilterRow>(this);
        saveResult = trn.LoadOneRowset(qry);
        var data = saveResult as RowsetBase;
        if (data != null)
        {
          DateTime asOf = DateTime.TryParse(AsOf, out asOf) ? asOf.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : App.TimeSource.UTCNow;
          foreach (var itemLog in data.OfType<TaskListFilterRow>())
            try
            {
              App.Log.Write(new NFX.Log.Message(itemLog.ToJSON()));
              if(itemLog.Due_Date.HasValue)
                itemLog.Remaining = (itemLog.Due_Date.Value - asOf).Days;
              
              var issueLogQuery = QTask.FindFirst5IssueLogByIssue<TaskListFilterRow>(itemLog.Counter, asOf);
              itemLog.Details = trn.LoadOneRowset(issueLogQuery).AsEnumerableOf<TaskListFilterRow>();

              var issueAssignQuery = QTask.FindIssueAssignByIssue<IssueAssignListFilterRow>(itemLog.Counter, asOf);
              itemLog.Assignments = trn.LoadOneRowset(issueAssignQuery).AsEnumerableOf<IssueAssignListFilterRow>();

              var areaQuery = QTask.FindAllAreaByIssue<AreaRow>(itemLog.Counter);
              itemLog.Areas = trn.LoadOneRowset(areaQuery).AsEnumerableOf<AreaRow>();
              
              var componentQuery = QTask.FindAllComponentByIssue<ComponentRow>(itemLog.Counter);
              itemLog.Components = trn.LoadOneRowset(componentQuery).AsEnumerableOf<ComponentRow>();

              var assigneeListQuery = QTask.FindAllAssignee<IssueAssignListFilterRow>(itemLog.Counter, asOf);
              itemLog.AssigneeList = trn.LoadOneRowset(assigneeListQuery).AsEnumerableOf<IssueAssignListFilterRow>();

            }
            catch (Exception ex)
            {
              App.Log.Write(new NFX.Log.Message(ex));
              result = ex;
            }
        }
      }
      return result;
    }
  }
}
