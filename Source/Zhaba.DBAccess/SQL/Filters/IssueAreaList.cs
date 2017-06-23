using MySql.Data.MySqlClient;

using NFX;
using NFX.DataAccess.MySQL;

using Zhaba.Data.Filters;
using Zhaba.DBAccess.Handlers;

namespace Zhaba.DBAccess.SQL.Filters
{
  public class IssueAreaList : ZhabaFilteredMySQLQueryHandler<IssueAreaListFilter>
  {
    public IssueAreaList(MySQLDataStore store, string name) : base(store, name)
    {
    }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, IssueAreaListFilter filter)
    {
      string where = string.Empty;

      var name = filter.Name.ParseName();
      if (name != null)
      {
        where += "AND (TA.NAME LIKE ?pName)";
        cmd.Parameters.AddWithValue("pName", name);
      }

      string order = "TA.NAME";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        var desc = filter.OrderBy.StartsWith("-");
        if (desc)
          order = "TA." + filter.OrderBy.Substring(1) + " DESC";
        else
          order = "TA." + filter.OrderBy + " ASC";
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
FROM tbl_area TA
LEFT JOIN tbl_issuearea TIA ON TIA.C_AREA = ta.COUNTER AND TIA.C_ISSUE = ?pIssue
WHERE TA.C_PROJECT = ?pProject
  {0}
ORDER BY {1}".Args(where, order);
    }
  }
}
