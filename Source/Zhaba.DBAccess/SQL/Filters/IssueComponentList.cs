using MySql.Data.MySqlClient;
using NFX;
using NFX.DataAccess.MySQL;
using Zhaba.Data.Filters;
using Zhaba.DBAccess.Handlers;

namespace Zhaba.DBAccess.SQL.Filters
{
  public class IssueComponentList : ZhabaFilteredMySQLQueryHandler<IssueComponentListFilter>
  {
    public IssueComponentList(MySQLDataStore store, string name) 
      : base(store, name)
    { }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, IssueComponentListFilter filter)
    {
      string where = string.Empty;

      var name = filter.Name.ParseName();
      if (name != null)
      {
        where += "AND (TA.NAME LIKE ?pName)";
        cmd.Parameters.AddWithValue("pName", name);
      }

      // first - number of column, second - OrderBy direction
      var orderBy = "4 ASC";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        orderBy = filter.OrderBy;
      }

      cmd.Parameters.AddWithValue("pProject", filter.ProjectCounter);
      cmd.Parameters.AddWithValue("pIssue", filter.IssueCounter);
      cmd.CommandText =
@"SELECT
 ?pProject AS C_PROJECT,
 ?pIssue AS C_iSSUE,
 TA.COUNTER,
 TA.NAME,
 CASE WHEN TIA.C_ISSUE IS NULL THEN 'F' ELSE 'T' END AS LINKED
FROM tbl_component TA
LEFT JOIN tbl_issuecomponent TIA ON TIA.C_COMPONENT = ta.COUNTER AND TIA.C_ISSUE = ?pIssue
WHERE TA.C_PROJECT = ?pProject AND TA.IN_USE = 'T'
  {0}
ORDER BY {1}".Args(where, orderBy);
    }
  }
}
