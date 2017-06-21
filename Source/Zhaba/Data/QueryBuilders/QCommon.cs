using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX.DataAccess.CRUD;

using Zhaba.Data.Filters;

namespace Zhaba.Data.QueryBuilders
{
  public static class QCommon
  {
    public static Query<TRow> ProjectsByFilter<TRow>(ProjectListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.ProjectList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    public static Query<TRow> ProjectByID<TRow>(ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Project.ByID")
      {
        new Query.Param("pID", id)
      };
    }
    
    public static Query<TRow> UsersByFilter<TRow>(UserListFilter filter) where TRow : Row
    {
        return new Query<TRow>("SQL.Filters.UserList")
        {
            new Query.Param("pFilter", filter)
        };
    }

    public static Query<TRow> AllAreas<TRow>() where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Area.All");
    }

    public static Query<TRow> AgendaByID<TRow>(ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Agenda.ByID")
      {
        new Query.Param("pID", id)
      };
    }

    public static Query<TRow> MeetingByID<TRow>(ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Meeting.ByID")
      {
        new Query.Param("pID", id)
      };
    }
  }
}
