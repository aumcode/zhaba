using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX.DataAccess.CRUD;

using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
  public class IssueAreaListFilter : ProjectIssueFilterBase
  {
    #region Nested
      private class IssueAreaListRow : TypedRow
      {
        [Field(visible: false)] public ulong  C_Project { get; set; }
        [Field(visible: false)] public ulong  C_Issue { get; set; }
        [Field] public ulong  Counter { get; set; }
        [Field] public string Name { get; set; }
        [Field] public bool   Linked { get; set; }
      }
    #endregion

    [Field(metadata: "Description='Name' Placeholder='Area Name' Hint='Area Name'")]
    public string Name { get; set; }

    [Field(valueList: "Name:Name Ascending,-Name:Name Descending",
           metadata: "Description='Sort By' Hint='Sort area list by'")]
    public string OrderBy { get; set; }

    protected override Exception DoSave(out object saveResult)
    {
      var qry = QProject.IssueAreaListByFilter<IssueAreaListRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);

      return null;
    }
  }
}
