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
    { }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, TaskListFilter filter)
    {
      string where = "(T1.STATUS_DATE = (Select MAX(STATUS_DATE) from tbl_issuelog as T2 where T1.C_ISSUE = T2.C_ISSUE)) ";

      var name = filter.Name.ParseName();
      if (name != null)
      {
        where += "AND (TI.NAME LIKE ?pName)";
        cmd.Parameters.AddWithValue("pName", name);
      }

      var description = filter.Description.ParseName();
      if (description != null)
      {
        where += "AND (T1.DESCRIPTION LIKE ?pDescription)";
        cmd.Parameters.AddWithValue("pDescription", description);
      }

      var filterStr = filter.Filter.ParseName();
      if (filterStr.IsNotNullOrWhiteSpace())
      {
        //where += "AND (t1.C_CATEGORY = ?pCategory)";
        //cmd.Parameters.AddWithValue("pfilterStr", filterStr);
      }

      if(filter.C_User != null) 
      {
        where += @"
        AND 
        (T1.C_ISSUE IN 
          (SELECT C_ISSUE 
           FROM tbl_issueassign 
           WHERE 
             C_USER = ?C_User AND 
             ( OPEN_TS <= ?DateUTC AND
              (?DateUTC < CLOSE_TS OR CLOSE_TS IS NULL)
             )
          )
        ) ";
        cmd.Parameters.AddWithValue("C_User", filter.C_User);
        cmd.Parameters.AddWithValue("DateUTC", App.TimeSource.UTCNow);// todo Здесь нужно поставить Date AsOf, если он задан! 
      }

      cmd.CommandText = @"
select 
  TI.COUNTER, 
  TI.NAME,
  T1.STATUS,
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
where {0}".Args(where);
      //and
      //  Exists(
      //	select C_ISSUE from tbl_issuearea as _TIA
      //		join tbl_area as _TA on _TIA.C_AREA = _TA.COUNTER
      //    where (_TIA.C_ISSUE = T1.C_ISSUE) and (_TA.Name like '%'))
      //and
      //  Exists(
      //	select C_ISSUE from tbl_issuecomponent as _TIC
      //		join tbl_component as _TC on _TIC.C_COMPONENT = _TC.COUNTER
      //    where (_TIC.C_ISSUE = T1.C_ISSUE) and (_TC.Name like '%'))

    }
  }
}
