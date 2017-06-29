using System;
using NFX;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using Zhaba.Data.Domains;
using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;

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

    }

    #endregion

    [Field(valueList: "Name:Name Ascending,-Name:Name Descending",
           metadata: "Description='Sort By' Hint='Sort component list by'")]
    public string OrderBy { get; set; }

    [Field(metadata: "Description='Name' Placeholder='Name' Hint='Area Name'")]
    public string Name { get; set; }

    [Field(metadata: "Description='Description' Placeholder='Description' Hint='Area Description'")]
    public string Description { get; set; }

    [Field(metadata: "Description='Category name' Placeholder='CategoryName' Hint='Category name'")]
    public string C_Category { get; set; }
    
    [Field(metadata: "Description='Filter' Placeholder='Filter' Hint='Filter'")]
    public string Filter { get; set; }

    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      JSONDataMap result = new JSONDataMap();

      return null;
    }

    protected override Exception DoSave(out object saveResult)
    {
      var qry = QTask.TasksByFilter<TaskListFilterRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);
      return null;
    }
  }
}
