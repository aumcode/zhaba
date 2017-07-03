using System;

using NFX.DataAccess.CRUD;

using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
  public class MilestoneListFilter : ProjectFilterBase
  {
    #region Nested

      private class MilestoneFilterListRow : TypedRow
      {
        [Field] public ulong    Counter       { get; set; }
        [Field] public string   Name          { get; set; }
        [Field] public string   Description   { get; set; }
        [Field] public DateTime Start_Date    { get; set; }
        [Field] public DateTime Plan_Date     { get; set; }
        [Field] public DateTime Complete_Date { get; set; }
      }

    #endregion


    [Field(valueList: "3 ASC:Name Ascending,3 DESC:Name Descending," +
                      "6 ASC:Start Date Ascending,6 DESC:Start Date Descending," +
                      "7 ASC:Plan Date Ascending,7 DESC:Plan Date Descending," +
                      "8 ASC:Complete Date Ascending,8 DESC:Complete Date Descending",
           metadata: "Description='Sort By' Hint='Sort component list by'")]
    public string OrderBy { get; set; }

    [Field(metadata: "Description='Name' Placeholder='Name' Hint='Component Name'")]
    public string Name { get; set; }

    [Field(metadata: "Description='Description' Placeholder='Description' Hint='Component Description'")]
    public string Description { get; set; }

    [Field(metadata: "Description='Start Date Span' Placeholder='Start date span' Hint='Start date span [from]-[to], both optional'")]
    public string StartDateSpan { get; set; }

    [Field(metadata: "Description='Plan Date Span' Placeholder='Plan date span' Hint='Plan date span [from]-[to], both optional'")]
    public string PlanDateSpan { get; set; }

    [Field(metadata: "Description='Complete Date Span' Placeholder='Complete date span' Hint='Complete date span [from]-[to], both optional'")]
    public string CompleteDateSpan { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      var qry = QProject.MilestonesByFilter<MilestoneFilterListRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);

      return null;
    }
  }
}
