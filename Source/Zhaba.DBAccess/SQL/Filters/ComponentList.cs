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
  public class ComponentList : ZhabaFilteredMySQLQueryHandler<ComponentListFilter>
  {
    public ComponentList(MySQLDataStore store, string name) : base(store, name)
    {
    }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, ComponentListFilter filter)
    {
      string where = string.Empty;

      var name = filter.Name.ParseName();
      if (name != null)
      {
        where += "AND (TC.NAME LIKE ?pName)";
        cmd.Parameters.AddWithValue("pName", name);
      }

      var description = filter.Description.ParseName();
      if (description != null)
      {
        where += "AND (TC.DESCRIPTION LIKE ?pDescription)";
        cmd.Parameters.AddWithValue("pDescription", description);
      }

      string order = "TC.Counter";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        var desc = filter.OrderBy.StartsWith("-");
        if (desc)
          order = "TC." + filter.OrderBy.Substring(1) + " DESC";
        else
          order = "TC." + filter.OrderBy + " ASC";
      }

      cmd.Parameters.AddWithValue("pProj_ID", filter.ProjectID);
      cmd.CommandText =
@"SELECT *
FROM tbl_component TC
WHERE
  (C_PROJECT = ?pProj_ID)
  {0}
ORDER BY {1}".Args(where, order);
    }
  }
}
