using System;
using NFX.DataAccess.CRUD;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
  public class IssueComponentListFilter : ProjectIssueFilterBase
  {
    #region Nested
    private class IssueComponentListRow : TypedRow
    {
      [Field(visible: false)]
      public ulong C_Project { get; set; }
      [Field(visible: false)]
      public ulong C_Issue { get; set; }
      [Field]
      public ulong Counter { get; set; }
      [Field]
      public string Name { get; set; }
      [Field]
      public bool Linked { get; set; }
    }
    #endregion

    [Field(metadata: "Description='Name' Placeholder='Component Name' Hint='Component Name'")]
    public string Name { get; set; }

    [Field(valueList: "4 ASC:Name Ascending,4 DESC:Name Descending",
           metadata: "Description='Sort By' Hint='Sort component list by'")]
    public string OrderBy { get; set; }

    protected override Exception DoSave(out object saveResult)
    {
      var qry = QProject.IssueComponentListByFilter<IssueComponentListRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);

      return null;
    }
  }
}
