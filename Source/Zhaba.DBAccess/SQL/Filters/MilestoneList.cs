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
  public class MilestoneList : ZhabaFilteredMySQLQueryHandler<MilestoneListFilter>
  {
    public MilestoneList(MySQLDataStore store) : base(store)
    {
    }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, MilestoneListFilter filter)
    {
      string where = string.Empty;

      var name = filter.Name.ParseName();
      if (name != null)
      {
        where += "AND (TM.NAME LIKE ?pName)";
        cmd.Parameters.AddWithValue("pName", name);
      }

      var description = filter.Description.ParseName();
      if (description != null)
      {
        where += "AND (TM.DESCRIPTION LIKE ?pDescription)";
        cmd.Parameters.AddWithValue("pDescription", description);
      }

      List<Tuple<DateTime?, DateTime?>> dates;
      if (filter.StartDateSpan.ParseDateSpan(out dates) && dates != null)
      {
        var sb = BuildDatesFilter("TM.START_DATE", dates, cmd);
        where += "AND ({0}) \r\n".Args(sb);
      }

      if (filter.PlanDateSpan.ParseDateSpan(out dates) && dates != null)
      {
        var sb = BuildDatesFilter("TM.PLAN_DATE", dates, cmd);
        where += "AND ({0}) \r\n".Args(sb);
      }

      if (filter.CompleteDateSpan.ParseDateSpan(out dates) && dates != null)
      {
        var sb = BuildDatesFilter("TM.COMPLETE_DATE", dates, cmd);
        where += "AND ({0}) \r\n".Args(sb);
      }

      string order = "TM.Counter";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        var desc = filter.OrderBy.StartsWith("-");
        if (desc)
          order = "TM." + filter.OrderBy.Substring(1) + " DESC";
        else
          order = "TM." + filter.OrderBy + " ASC";
      }

      cmd.Parameters.AddWithValue("pProj_ID", filter.ProjectID);
      cmd.CommandText =
@"SELECT *
FROM tbl_milestone TM
WHERE
  (C_PROJECT = ?pProj_ID)
  {0}
ORDER BY {1}".Args(where, order);
    }
  }
}
