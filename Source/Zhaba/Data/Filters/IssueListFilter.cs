using System;

using NFX.DataAccess.CRUD;

using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
  public class IssueListFilter : ProjectFilterBase
  {
    #region Nested

      private class IssueFilterListRow : TypedRow
      {
        [Field] public ulong  Counter { get; set; }
        [Field] public bool   In_Use  { get; set; }
        [Field] public string Name    { get; set; }
      }

    #endregion


    [Field(valueList: "2 ASC:Name Ascending,2 DESC:Name Descending",
           metadata: "Description='Sort By' Hint='Sort issue list by'")]
    public string OrderBy { get; set; }

    [Field(metadata: "Description='Name' Placeholder='Name' Hint='Issue Name'")]
    public string Name { get; set; }

    protected override Exception DoSave(out object saveResult)
    {
      var qry = QProject.IssuesByFilter<IssueFilterListRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);

      return null;
    }
  }
}
