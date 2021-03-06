﻿using System;
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
    public MilestoneList(MySQLDataStore store, string name) : base(store, name)
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
      if (filter.StartDateSpan.ParseDate(out dates) && dates != null)
      {
        var sb = BuildDatesFilter("TM.START_DATE", dates, cmd);
        where += "AND ({0}) \r\n".Args(sb);
      }

      if (filter.PlanDateSpan.ParseDate(out dates) && dates != null)
      {
        var sb = BuildDatesFilter("TM.PLAN_DATE", dates, cmd);
        where += "AND ({0}) \r\n".Args(sb);
      }

      if (filter.CompleteDateSpan.ParseDate(out dates) && dates != null)
      {
        var sb = BuildDatesFilter("TM.COMPLETE_DATE", dates, cmd);
        where += "AND ({0}) \r\n".Args(sb);
      }

      // first - number of column, second - OrderBy direction
      var orderBy = "3 ASC";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        orderBy = filter.OrderBy;
      }

      cmd.Parameters.AddWithValue("pProj_ID", filter.ProjectCounter);
      cmd.CommandText =
@"SELECT *
FROM tbl_milestone TM
WHERE
  (C_PROJECT = ?pProj_ID)
  {0}
ORDER BY {1}".Args(where, orderBy);
    }
  }
}
