﻿using System;
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
    public static Query<TRow> AllComponents<TRow>() where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Component.All");
    }

    public static Query<TRow> ComponentsByFilter<TRow>(ComponentListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.ComponentList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    public static Query<TRow> ComponentByID<TRow>(ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Component.ByID")
      {
        new Query.Param("pID", id)
      };
    }

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

    public static Query<TRow> AreasByFilter<TRow>(AreaListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.AreaList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    public static Query<TRow> AreaByID<TRow>(ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Area.ByID")
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

    public static Query<TRow> UserByID<TRow>(ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.User.ByID")
      {
        new Query.Param("pID", id)
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
