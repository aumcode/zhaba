using NFX.DataAccess.CRUD;
using Zhaba.Data.Filters;

namespace Zhaba.Data.QueryBuilders
{
  public class QIssueChat
  {
    public static Query<TRow> findIssueChatByIdAndIssueAndProject<TRow>(ulong c_Project, ulong c_Issue, ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueChat.findIssueChatByIdAndIssueAndProject")
      {
        new Query.Param("pPID", c_Project),
        new Query.Param("pIID", c_Issue),
        new Query.Param("pID", id)
      };
    }
    
    public static Query<TRow> findIssueChatById<TRow>(ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueChat.findIssueChatById")
      {
        new Query.Param("pID", id)
      };
    }

    public static Query<TRow> findIssueChatByFilter<TRow>(IssueChatFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.IssueChatList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    public static Query<TRow> findByIssue<TRow>(ulong c_Issue) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.IssueChat.findByIssue")
      {
        new Query.Param("pIssue", c_Issue)
      };
    }
  }
}