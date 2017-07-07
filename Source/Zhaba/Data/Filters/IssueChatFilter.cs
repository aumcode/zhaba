using System;
using NFX.DataAccess.CRUD;
using Zhaba.Data.QueryBuilders;
using NFX;
using Zhaba.Data.Rows;
using NFX.Serialization.JSON;

namespace Zhaba.Data.Filters
{
  public class IssueChatFilter : IssueFilterBase
  {
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

    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      var result = new JSONDataMap();
      if (fdef.Name.EqualsIgnoreCase("C_USER"))
      {
        var users = ZApp.Data.CRUD.LoadEnumerable(QUser.FindAllActiveUser<UserRow>());
        foreach (var user in users)
          result.Add(user.Counter.ToString(), "{0} {1} ({2})".Args(user.First_Name, user.Last_Name, user.Last_Name));
      }

      return result;
    }

    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      Query<IssueChatFilterRow> query = QIssueChat.findIssueChatByFilter<IssueChatFilterRow>(this);
      saveResult = ZApp.Data.CRUD.LoadOneRowset(query);
      return null;
    }
  }
}