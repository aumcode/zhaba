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
        [Field] public ulong  Counter        { get; set; }
        [Field] public string Name           { get; set; }
	      [Field] public string Description    { get; set; }
	      [Field] public string Milestone_Name { get; set; }
	      [Field] public string Area_Name      { get; set; }
	      [Field] public string Component_Name { get; set; }
	      [Field] public string Status         { get; set; }
	      [Field] public string Creator        { get; set; }
	      [Field] public string Owner          { get; set; }
	      [Field] public string Creation_Date  { get; set; }
	      [Field] public string Change_Date    { get; set; }
      }

    #endregion


    [Field(valueList: "Name:Name Ascending,-Name:Name Descending,"+
                      "Status:Status Ascending,-Status:Status Descending,"+
                      "Creator:Creator Ascending,-Creator:Creator Descending,"+
                      "Owner:Owner Ascending,-Owner:Owner Descending,"+
                      "Creation_Date:Creation Date Ascending,-Creation_Date:Creation Date Descending,"+
                      "Change_Date:Change Date Ascending,-Change_Date:Change_Date Descending",
           metadata: "Description='Sort By' Hint='Sort component list by'")]
    public string OrderBy { get; set; }

    [Field(metadata: "Description='Name' Placeholder='Name' Hint='Component Name'")]
    public string Name { get; set; }

    [Field(metadata: "Description='Description' Placeholder='Description' Hint='Component Description'")]
    public string Description { get; set; }

    [Field(metadata: "Description='Milestone' Placeholder='Milestone name' Hint='Milestone name'")]
    public string Milestone { get; set; }

    [Field(metadata: "Description='Area' Placeholder='Area name' Hint='Area name'")]
    public string Area { get; set; }

    [Field(metadata: "Description='Component' Placeholder='Component name' Hint='Component name'")]
    public string Component { get; set; }

    [Field(metadata: "Description='Status' Placeholder='Status name' Hint='Status name'")]
    public string Status { get; set; }

    [Field(metadata: "Description='Creator' Placeholder='Creator name' Hint='Creator name'")]
    public string Creator { get; set; }

    [Field(metadata: "Description='Owner' Placeholder='Owner name' Hint='Owner name'")]
    public string Owner { get; set; }

    [Field(metadata: "Description='Creation Date Span' Placeholder='Creation date span' Hint='Creation date span [from]-[to], both optional'")]
    public string CreationDateSpan { get; set; }

    [Field(metadata: "Description='Change Date Span' Placeholder='Change date span' Hint='Change date span [from]-[to], both optional'")]
    public string ChangeDateSpan { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      var qry = QProject.IssuesByFilter<IssueFilterListRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(qry);

      return null;
    }
  }
}
