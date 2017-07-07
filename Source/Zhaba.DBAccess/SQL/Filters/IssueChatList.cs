using MySql.Data.MySqlClient;
using NFX;
using NFX.DataAccess.MySQL;
using Zhaba.Data.Filters;
using Zhaba.DBAccess.Handlers;

namespace Zhaba.DBAccess.SQL.Filters
{
  public class IssueChatList : ZhabaFilteredMySQLQueryHandler<IssueChatFilter>
  {

    public static readonly int CHAT_VIEW_LIMIT = 5;

    private readonly string m_Script = @"
SELECT t1.COUNTER,
       t1.C_ISSUE,
       t1.C_USER,
       t1.NOTE_DATE,
       t1.NOTE,
       t2.FIRST_NAME,
       t2.LAST_NAME,
       t2.LOGIN
FROM tbl_issuechat t1
JOIN tbl_user t2 ON t2.COUNTER = T1.C_USER
WHERE
{0}
ORDER BY
{1}
LIMIT ?pLimit
";
    
    public IssueChatList(MySQLDataStore store, string name) : base(store, name)
    {
    }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, IssueChatFilter filter)
    {
      string where = "(C_ISSUE = ?pIssue)";
      string order = "4 DESC";

      if (filter.C_User != null)
      {
        where += "AND (C_USER = ?pUser)";
        cmd.Parameters.AddWithValue("pUser", filter.C_User);
      }
      
      if (filter.Note != null)
      {
        where += "AND (Note = ?pNote)";
        cmd.Parameters.AddWithValue("pNote", "%"+filter.Note+"%");
      }
      
      cmd.Parameters.AddWithValue("pIssue", filter.IssueRow.Counter);
      cmd.Parameters.AddWithValue("pLimit", filter.Limit ?? CHAT_VIEW_LIMIT);
      cmd.CommandText = m_Script.Args(where, order);
    }
  }
}