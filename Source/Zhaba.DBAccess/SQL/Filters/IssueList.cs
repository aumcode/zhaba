using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.MySQL;

using Zhaba.Data.Filters;
using Zhaba.DBAccess.Handlers;

namespace Zhaba.DBAccess.SQL.Filters
{
  public class IssueList : ZhabaFilteredMySQLQueryHandler<IssueListFilter>
  {
    public IssueList(MySQLDataStore store, string name) : base(store, name)
    {
    }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, IssueListFilter filter)
    {
      string where = string.Empty;

      var name = filter.Name.ParseName();
      if (name != null)
      {
        where += "AND (TI.NAME LIKE ?pName)";
        cmd.Parameters.AddWithValue("pName", name);
      }
      
      // first - number of column, second - OrderBy direction
      var orderBy = "3 ASC";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        orderBy = filter.OrderBy;
      }

      cmd.Parameters.AddWithValue("pProj_ID", filter.ProjectCounter);
      cmd.CommandText =
@"SELECT
  TI.COUNTER,
  TI.NAME,
  TI.IN_USE
FROM
  tbl_issue TI
WHERE
  (TI.C_PROJECT = ?pProj_ID)
  {0}
ORDER BY {1}".Args(where, orderBy);
    }
  }
}
