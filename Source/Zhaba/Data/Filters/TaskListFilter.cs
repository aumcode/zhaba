using System;
using NFX;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using Zhaba.Data.Domains;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using System.Collections.Generic;

namespace Zhaba.Data.Filters
{
  public class TaskListFilter : ZhabaFilterForm
  {
    #region Nested

    private class TaskListFilterRow : TypedRow
    {
      private string _status;

      [Field]
      public ulong Counter { get; set; }

      [Field]
      public string Name { get; set; }

      [Field]
      public string Category_Name { get; set; }

      [Field]
      public string Description { get; set; }

      [Field]
      public string Status
      {
        get { return ZhabaIssueStatus.MapDescription(_status); }
        set { _status = value; }
      }

      [Field]
      public DateTime Complete_Date { get; set; }

      [Field]
      public DateTime Start_Date { get; set; }

      [Field]
      public DateTime Plan_Date { get; set; }

      [Field]
      public int Completeness { get; set; }

      [Field]
      public int C_Project { get; set; }

      [Field]
      public string ProjectName { get; set; }

      [Field]
      public string Note { get; set; }
      
      [Field]
      public List<TaskListFilterRow> Details { get; set; }

    }

    #endregion

    [Field(metadata: "Description='AsOf' Placeholder='AsOf' Hint='AsOf'")]
    public string AsOf { get; set; } = App.TimeSource.UTCNow.ToString();
    
    [Field(metadata: "Description='Search' Placeholder='Search' Hint='Search'")]
    public string Search { get; set; }

    [Field(metadata: "Description='Assign' Placeholder='Assign' Hint='Assign'")]
    public ulong? C_USER { get; set; }

    [Field(metadata: "Description='Project' Placeholder='Project' Hint='Project'")]
    public string C_PROJECT { get; set; }

    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      var result = new JSONDataMap();
      if (fdef.Name.EqualsIgnoreCase("C_USER"))
      {
        var users = ZApp.Data.CRUD.LoadEnumerable(QUser.FindAllActiveUser<UserRow>());
        foreach (var user in users)
          result.Add(user.Counter.ToString(), user.First_Name);
      }
      return result;
    }

    protected override Exception DoSave(out object saveResult)
    {
      var qry = QTask.TasksByFilter<TaskListFilterRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);
      var data = saveResult as RowsetBase;
      if (data != null)
      {
        DateTime asOf = DateTime.TryParse(this.AsOf, out asOf) ? asOf : App.TimeSource.UTCNow;
        foreach (var item in data)
          try
          {
            var itemLog = item as TaskListFilterRow;
            App.Log.Write(new NFX.Log.Message(item.ToJSON()));
            var query = QTask.FindFirst5IssueLogByIssue<TaskListFilterRow>(itemLog.Counter, asOf);
            var list = ZApp.Data.CRUD.LoadOneRowset(query);
            itemLog.Details = new List<TaskListFilterRow>();
            foreach (var item1 in list)
            {
              var item1Log = item1 as TaskListFilterRow;
              itemLog.Details.Add(item1Log);
            }
          }
          catch (Exception ex)
          {
            App.Log.Write(new NFX.Log.Message(ex));
          }
      }
      
      return null;
    }
  }
}
