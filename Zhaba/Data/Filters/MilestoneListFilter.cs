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


    [Field(valueList: "Name:Name Ascending,-Name:Name Descending,"+
                      "Start_Date:Start Date Ascending,-Start_Date:Start Date Descending,"+
                      "Plan_Date:Plan Date Ascending,-Plan_Date:Plan Date Descending,"+
                      "Complete_Date:Complete Date Ascending,-Complete_Date:Complete Date Descending",
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
