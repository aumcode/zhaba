using System;
using System.Globalization;
using MySql.Data.MySqlClient;
using NFX;
using NFX.DataAccess.MySQL;
using Zhaba.Data.Filters;
using Zhaba.DBAccess.Handlers;

namespace Zhaba.DBAccess.SQL.Filters
{
  public class TaskList : ZhabaFilteredMySQLQueryHandler<TaskListFilter>
  {
    public TaskList(MySQLDataStore store, string name)
      : base(store, name)
    {
    }

    private readonly string m_Script = @"
select 
  TI.COUNTER, 
  TI.NAME,
  T1.STATUS,
  T1.STATUS_DATE,
  T1.DESCRIPTION,
  TM.COMPLETE_DATE, 
  TM.START_DATE,
  T1.COMPLETENESS, 
  TM.PLAN_DATE,
  TC.NAME as CATEGORY_NAME,
  TP.COUNTER as C_PROJECT,
  TP.NAME as PROJECTNAME,
  T1.NOTE
from tbl_issuelog as T1
  join tbl_issue as TI on T1.C_ISSUE = TI.COUNTER
  join tbl_category as TC on T1.C_CATEGORY = TC.COUNTER
  join tbl_milestone as TM on T1.C_MILESTONE = TM.COUNTER
  join tbl_project as TP on TI.C_PROJECT = TP.COUNTER
where {0}";

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd,
      TaskListFilter filter)
    {
      var where = "(T1.STATUS_DATE = (Select MAX(STATUS_DATE) from tbl_issuelog as T2 where (T1.C_ISSUE = T2.C_ISSUE) and (STATUS_DATE <= ?pDateUTC))) ";
      DateTime asOf;
      cmd.Parameters.AddWithValue("pDateUTC", DateTime.TryParse(filter.AsOf, out asOf) ? asOf.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : App.TimeSource.UTCNow.Date);

      if (filter.Due.IsNotNullOrWhiteSpace())
      {
        var days = int.Parse(filter.Due);
        
        var end = asOf.Date;
        var start = end.AddDays(-days);

        where += "AND ((TM.PLAN_DATE between ?pSTART and ?pEND) OR (TM.PLAN_DATE < ?pSTART and T1.STATUS != 'C'))";
        cmd.Parameters.AddWithValue("pSTART", start);
        cmd.Parameters.AddWithValue("pEND", end);
      }

      if (filter.ProjectName.IsNotNullOrWhiteSpace())
      {
        where += "AND (TP.NAME like ?pPName)";
        cmd.Parameters.AddWithValue("pPName", filter.ProjectName);
      }

      if (filter.C_USER.HasValue)
      {
        where += @"
          AND 
          (T1.C_ISSUE IN 
            (SELECT C_ISSUE 
             FROM tbl_issueassign 
             WHERE 
               C_USER = ?pC_User AND 
               ( OPEN_TS <= ?pDateUTC AND
                (?pDateUTC < CLOSE_TS OR CLOSE_TS IS NULL)
               )
            )
          ) ";
        cmd.Parameters.AddWithValue("pC_User", filter.C_USER);
      }

      try
      {
        var searchStr = filter.Search;
        if (searchStr.IsNotNullOrWhiteSpace())
        {
          var scfg = searchStr.AsLaconicConfig(handling: ConvertErrorHandling.Throw);
          var filterName = scfg.Navigate("$n|$name").Value;
          if (filterName != null)
          {
            where += "AND (TI.NAME LIKE ?pName)";
            cmd.Parameters.AddWithValue("pName", filterName);
          }

          var filterArea = scfg.Navigate("$a|$area").Value;
          if (filterArea.IsNotNullOrWhiteSpace())
          {
            where +=
              "AND Exists(select C_ISSUE from tbl_issuearea as _TIA join tbl_area as _TA on _TIA.C_AREA = _TA.COUNTER where (_TIA.C_ISSUE = T1.C_ISSUE) and (_TA.Name like ?pAREA))";
            cmd.Parameters.AddWithValue("pAREA", filterArea);
          }

          var componentFilter = scfg.Navigate("$c|$component").Value;
          if (componentFilter.IsNotNullOrWhiteSpace())
          {
            where +=
              "AND Exists(select C_ISSUE from tbl_issuecomponent as _TIC join tbl_component as _TC on _TIC.C_COMPONENT = _TC.COUNTER where (_TIC.C_ISSUE = T1.C_ISSUE) and(_TC.Name like ?pCOMPONENT))";
            cmd.Parameters.AddWithValue("pCOMPONENT", componentFilter);
          }

          var categoryFilter = scfg.Navigate("$cat|$category").Value;
          if (categoryFilter.IsNotNullOrWhiteSpace())
          {
            where += "AND (TC.NAME like pCATEGORY)";
            cmd.Parameters.AddWithValue("pCATEGORY", categoryFilter);
          }
        }
        cmd.CommandText = m_Script.Args(where);
      }
      catch (Exception)
      {
        where += "AND ((TI.NAME LIKE ?pSearch) OR";
        where += "(T1.STATUS LIKE ?pSearch) OR";
        where += "(TP.NAME LIKE ?pSearch) OR";
        where += "(T1.DESCRIPTION LIKE ?pSearch)) OR";
        where += "(T1.NOTE LIKE ?pSearch))";
        cmd.Parameters.AddWithValue("pSearch", "%{0}%".Args(filter.Search));
        cmd.CommandText = m_Script.Args(where);
      }
    }
  }
}
