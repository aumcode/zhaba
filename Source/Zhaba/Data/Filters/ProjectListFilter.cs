using System;

using NFX.DataAccess.CRUD;

using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
  public class ProjectListFilter : ZhabaFilterForm
  {
    #region Nested

      private class ProjectFilterListRow : TypedRow
      {
        [Field] public ulong  Counter     { get; set; }
        [Field] public string Name        { get; set; }
        [Field] public string Description { get; set; }
      }

    #endregion


    [Field(valueList: "2 ASC:Name Ascending,2 DESC:Name Descending",
           metadata: "Description='Sort By' Hint='Sort Project list by'")]
    public string OrderBy { get; set; }

    [Field(metadata: "Description='Name' Placeholder='Name' Hint='Project Name'")]
    public string Name { get; set; }

    [Field(metadata: "Description='Description' Placeholder='Description' Hint='Project Description'")]
    public string Description { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      var qry = QCommon.ProjectsByFilter<ProjectFilterListRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);

      return null;
    }
  }
}
