using System;

using NFX.DataAccess.CRUD;

using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
  public class AreaListFilter : ProjectFilterBase
  {
    #region Nested

      private class AreaFilterListRow : TypedRow
      {
        [Field] public ulong  Counter     { get; set; }
        [Field] public string Name        { get; set; }
        [Field] public string Description { get; set; }
      }

    #endregion


    [Field(valueList: "Name:Name Ascending,-Name:Name Descending",
           metadata: "Description='Sort By' Hint='Sort area list by'")]
    public string OrderBy { get; set; }

    [Field(metadata: "Description='Name' Placeholder='Name' Hint='Area Name'")]
    public string Name { get; set; }

    [Field(metadata: "Description='Description' Placeholder='Description' Hint='Area Description'")]
    public string Description { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      var qry = QProject.AreasByFilter<AreaFilterListRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);

      return null;
    }
  }
}
