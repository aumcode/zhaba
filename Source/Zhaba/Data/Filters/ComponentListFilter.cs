using System;

using NFX.DataAccess.CRUD;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
  public class ComponentListFilter : ProjectFilterBase
  {
    #region Nested

      private class ComponentFilterListRow : TypedRow
      {
        [Field] public ulong  Counter     { get; set; }
        [Field] public string Name        { get; set; }
        [Field] public string Description { get; set; }
      }

    #endregion


    [Field(valueList: "3 ASC:Name Ascending,3 DESC:Name Descending",
           metadata: "Description='Sort By' Hint='Sort component list by'")]
    public string OrderBy { get; set; }

    [Field(metadata: "Description='Name' Placeholder='Name' Hint='Component Name'")]
    public string Name { get; set; }

    [Field(metadata: "Description='Description' Placeholder='Description' Hint='Component Description'")]
    public string Description { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      var qry = QProject.ComponentsByFilter<ComponentFilterListRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);

      return null;
    }
  }
}
