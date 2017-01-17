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
    public IssueList(MySQLDataStore store) : base(store)
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

      var description = filter.Description.ParseName();
      if (description != null)
      {
        where += "AND (TI.DESCRIPTION LIKE ?pDescription)";
        cmd.Parameters.AddWithValue("pDescription", description);
      }

      var milestone = filter.Milestone.ParseName();
      if (milestone != null)
      {
        where += "AND (TM.NAME LIKE ?pMilestoneName)";
        cmd.Parameters.AddWithValue("pMilestoneName", milestone);
      }

      var area = filter.Area.ParseName();
      if (area != null)
      {
        where += "AND (TA.NAME LIKE ?pAreaName)";
        cmd.Parameters.AddWithValue("pAreaName", area);
      }

      var component = filter.Component.ParseName();
      if (component != null)
      {
        where += "AND (TC.NAME LIKE ?pComponentName)";
        cmd.Parameters.AddWithValue("pComponentName", component);
      }

      List<Tuple<DateTime?, DateTime?>> dates;
      if (filter.CreationDateSpan.ParseDateSpan(out dates) && dates != null)
      {
        var sb = BuildDatesFilter("TI.CREATION_DATE", dates, cmd);
        where += "AND ({0}) \r\n".Args(sb);
      }

      if (filter.ChangeDateSpan.ParseDateSpan(out dates) && dates != null)
      {
        var sb = BuildDatesFilter("TI.CHANGE_DATE", dates, cmd);
        where += "AND ({0}) \r\n".Args(sb);
      }

      string order = "TI.Counter";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        var desc = filter.OrderBy.StartsWith("-");
        if (desc)
          order = "TI." + filter.OrderBy.Substring(1) + " DESC";
        else
          order = "TI." + filter.OrderBy + " ASC";
      }

      cmd.Parameters.AddWithValue("pProj_ID", filter.ProjectID);
      cmd.CommandText =
@"SELECT
  TI.COUNTER,
	TI.NAME,
	TI.DESCRIPTION,
	TI.C_MILESTONE,
	TI.C_AREA,
	TI.C_COMPONENT,
	TI.STATUS,
	TI.CREATOR,
	TI.OWNER,
	TI.CREATION_DATE,
	TI.CHANGE_DATE,
  TM.NAME AS MILESTONE_NAME,
  TA.NAME AS AREA_NAME,
  TC.NAME AS COMPONENT_NAME
FROM
  tbl_issue     TI LEFT OUTER JOIN
  tbl_milestone TM ON (TI.C_MILESTONE = TM.COUNTER) LEFT OUTER JOIN
  tbl_area      TA ON (TI.C_AREA = TA.COUNTER) LEFT OUTER JOIN
  tbl_component TC ON (TI.C_COMPONENT = TC.COUNTER)
WHERE
  (TI.C_PROJECT = ?pProj_ID)
  {0}
ORDER BY {1}".Args(where, order);
    }
  }
}
