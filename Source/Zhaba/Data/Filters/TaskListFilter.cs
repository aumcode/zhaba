using System;
using System.Globalization;
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

    [Field(metadata: "Description='AsOf' Placeholder='AsOf' Hint='AsOf'")]
    public string AsOf { get; set; } = App.TimeSource.UTCNow.Date.ToString(CultureInfo.InvariantCulture);
    
    [Field(metadata: "Description='Search' Placeholder='Type n=*, a=*, c=*, cat=* for search. Example: n=Issue1 a=UI' Hint='Search'")]
    public string Search { get; set; }

    [Field(metadata: "Description='Assign' Placeholder='Assign' Hint='Assign'")]
    public ulong? C_USER { get; set; }

    [Field(metadata: "Description='Project name' Placeholder='Project name' Hint='Project name'")]
    public string ProjectName { get; set; }

    [Field(valueList: "1:Day, 2:2 Days, 7:7 Days, 14: 14 Days, 30:Month, 60:Quarter, 365:Year", metadata: "Description='Due' Hint='Due'")]
    public string Due { get; set; }

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
      return result;
    }

    protected override Exception DoSave(out object saveResult)
    {
      var qry = QTask.TasksByFilter<TaskListFilterRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);
      return null;
    }
  }
}
