using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using NFX;
using NFX.DataAccess.MySQL;

using Zhaba.Data.Filters;
using Zhaba.DBAccess.Handlers;

namespace Zhaba.DBAccess.SQL.Filters
{
  public class ProjectList : ZhabaFilteredMySQLQueryHandler<ProjectListFilter>
  {
    public ProjectList(MySQLDataStore store) : base(store)
    {
    }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, ProjectListFilter filter)
    {
      string where = string.Empty;

      var name = filter.Name.ParseName();
      if (name != null)
      {
        where += "AND (TP.NAME LIKE ?pName)";
        cmd.Parameters.AddWithValue("pName", name);
      }

      var description = filter.Description.ParseName();
      if (description != null)
      {
        where += "AND (TP.DESCRIPTION LIKE ?pDescription)";
        cmd.Parameters.AddWithValue("pDescription", description);
      }

      string order = "TP.Counter";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        var desc = filter.OrderBy.StartsWith("-");
        if (desc)
          order = "TP." + filter.OrderBy.Substring(1) + " DESC";
        else
          order = "TP." + filter.OrderBy + " ASC";
      }

      cmd.CommandText =
@"SELECT *
FROM tbl_project TP
WHERE (1 = 1) {0}
ORDER BY {1}".Args(where, order);
    }
  }
}
