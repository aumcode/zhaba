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
  NULL AS COMPLETE_DATE, 
  T1.START_DATE,
  T1.COMPLETENESS, 
  T1.DUE_DATE,
  TC.NAME as CATEGORY_NAME,
  TP.COUNTER as C_PROJECT,
  TP.NAME as PROJECTNAME,
  (SELECT GROUP_CONCAT(_tt2.LOGIN SEPARATOR '; ')
     FROM tbl_issueassign _tt1
       JOIN tbl_user _tt2 ON _tt1.C_USER = _tt2.COUNTER
     WHERE (_tt1.C_ISSUE = T1.C_ISSUE) AND ( ?pDateUTC < _tt1.CLOSE_TS OR _tt1.CLOSE_TS IS NULL )
     GROUP BY C_ISSUE) AS ASSIGNEE,
  T1.PRIORITY
    
from tbl_issuelog as T1
  join tbl_issue as TI on T1.C_ISSUE = TI.COUNTER
  join tbl_category as TC on T1.C_CATEGORY = TC.COUNTER
  join tbl_milestone as TM on T1.C_MILESTONE = TM.COUNTER
  join tbl_project as TP on TI.C_PROJECT = TP.COUNTER
where {0}
ORDER BY T1.DUE_DATE ASC
";

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd,
      TaskListFilter filter)
    {
      var where = "(T1.STATUS_DATE = (Select MAX(STATUS_DATE) from tbl_issuelog as T2 where (T1.C_ISSUE = T2.C_ISSUE) and (STATUS_DATE <= ?pDateUTC))) ";
      DateTime asOf;
      cmd.Parameters.AddWithValue("pDateUTC", DateTime.TryParse(filter.AsOf, out asOf) ? asOf.Date.AddHours(23).AddMinutes(59).AddSeconds(59) : App.TimeSource.UTCNow.Date);

      if (filter.Due.IsNotNullOrWhiteSpace())
      {
        var days = int.Parse(filter.Due);
        var start = asOf.Date;
        var end = start.AddDays(days) ;

        where += "AND ((T1.DUE_DATE between ?pSTART and ?pEND) OR (T1.DUE_DATE < ?pSTART and (T1.STATUS NOT IN ('C', 'X', 'F', 'D'))))";
        cmd.Parameters.AddWithValue("pSTART", start);
        cmd.Parameters.AddWithValue("pEND", end);
      }

      if (filter.ProjectName.IsNotNullOrWhiteSpace())
      {
        where += "AND (TP.NAME like ?pPName)";
        cmd.Parameters.AddWithValue("pPName", filter.ProjectName);
      }

      if (filter.CategoryName.IsNotNullOrWhiteSpace())
      {
        where += "AND (TC.NAME like ?pCName)";
        cmd.Parameters.AddWithValue("pCName", filter.CategoryName);
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

      if (filter.Status.IsNotNullOrEmpty())
      {
        where += " AND (T1.STATUS = ?pStatus) ";
        cmd.Parameters.AddWithValue("pStatus", filter.Status);
      }
      else
      {
        where += " AND (T1.STATUS NOT IN ('X')) ";  
      }

      try
      {
        var searchStr = filter.Search;
        if (searchStr.IsNotNullOrWhiteSpace())
        {
          int value;
          if (int.TryParse(searchStr, out value))
          {
            where += "AND (T1.C_ISSUE = ?pIssueId) ";
            cmd.Parameters.AddWithValue("pIssueId", value);
          }
          else if ((searchStr.StartsWith("<") || searchStr.StartsWith(">") || searchStr.StartsWith("=")) && (int.TryParse(searchStr.Substring(1).Trim() , out value)) )
          {
            where += "AND (T1.COMPLETENESS  {0} ?pProceed) ".Args(searchStr.Substring(0,1));
            cmd.Parameters.AddWithValue("pProceed", value);
          }
          else
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
          }
        }
        cmd.CommandText = m_Script.Args(where);
      }
      catch (Exception)
      {
        where += @" AND 
(
      (TI.NAME LIKE ?pSearch)
  OR  (T1.STATUS LIKE ?pSearch)
  OR  (TP.NAME LIKE ?pSearch)
  OR  (
        EXISTS
        (
          SELECT _tis.COUNTER 
          FROM tbl_issuelog _tis 
          WHERE (_tis.C_ISSUE = T1.C_ISSUE) 
            AND (UPPER(_tis.DESCRIPTION) LIKE UPPER(?pSearch))
        )
      )
  OR  (
        EXISTS
        (
          SELECT CONCAT(_tu.FIRST_NAME,' ',_tu.LAST_NAME,' (',_tu.LOGIN ,')') AS NAME
          FROM tbl_issueassign _tia
          JOIN tbl_user _tu ON _tu.COUNTER = _tia.C_USER  
          WHERE (_tia.C_ISSUE = T1.C_ISSUE)
            AND (UPPER(CONCAT(_tu.FIRST_NAME,' ',_tu.LAST_NAME,' (',_tu.LOGIN ,')')) LIKE UPPER(?pSearch))
        )
      )
)
";
        //where += "OR (EXISTS(SELECT _tia.Count FROM tbl_issueassign _tia WHERE (?pDateUTC <= _tia.CLOSE_TS OR _tia.CLOSE_TS IS NULL) AND (_tia.C_ISSUE = T1.C_ISSUE) AND  (_tia.LOGIN LIKE ?pSearch OR _tia.FULL_NAME LIKE ?pSearch _tia.LAST_NAME LIKE ?pSearch)))";
        cmd.Parameters.AddWithValue("pSearch", "%{0}%".Args(filter.Search));
        cmd.CommandText = m_Script.Args(where);
      }
    }
  }
}
