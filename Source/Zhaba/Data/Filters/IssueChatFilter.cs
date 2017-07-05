using System;
using NFX.DataAccess.CRUD;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Filters
{
  public class IssueChatFilter : IssueFilterBase
  {
    #region Nested

    private class IssueChatFilterRow : TypedRow
    {
      [Field] public ulong    Counter    { get; set; }
      [Field] public ulong    C_Issue    { get; set; }
      [Field] public ulong    C_User     { get; set; }
      [Field] public DateTime Note_Date  { get; set; }
      [Field] public string   Note       { get; set; }
    }

    #endregion
    
    [Field(valueList: "2 ASC:Name Ascending,2 DESC:Name Descending",
      metadata: "Description='Sort By' Hint='Sort issue list by'")]
    public string OrderBy { get; set; }

    [Field(metadata: "Description='Note Date' Placeholder='Note Date' Hint='Note Date'")]
    public DateTime? Note_Date { get; set; }
    
    [Field(metadata: "Description='User' Placeholder='User' Hint='User'")]
    public ulong? C_User { get; set; }
    
    [Field(metadata: "Description='Note' Placeholder='Note' Hint='Note'")]
    public string Note { get; set; }

    [Field(metadata: "Description='limit' Placeholder='limit' Hint='limit'")]
    public int? Limit { get; set; }

    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      Query<IssueChatFilterRow> query = QIssueChat.findIssueChatByFilter<IssueChatFilterRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(query);
      return null;
    }
  }
}