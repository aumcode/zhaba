using System;
using NFX.DataAccess.CRUD;
using Zhaba.Data.QueryBuilders;
using NFX;

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
      [Field] public string   First_Name { get; set; }
      [Field] public string   Last_Name  { get; set; }
      [Field] public string   Login      { get; set; }
      [Field] public string   Name 
      { 
        get 
        {
          return "{0} {1}".Args(First_Name, Last_Name);
        } 
      }
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