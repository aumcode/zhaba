using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX.DataAccess.CRUD;

using Zhaba.Data.Filters;

namespace Zhaba.Data.QueryBuilders
{
  public static class QProject
  {
    public static Query<TRow> AllProjects<TRow>() where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Project.All");
    }

    public static Query<TRow> MilestoneByUK<TRow>(ulong projID, string msName) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Milestone.ByUK")
      {
        new Query.Param("pProj_ID", projID),
        new Query.Param("pName", msName)
      };
    }

    public static Query<TRow> MilestoneByID<TRow>(ulong projID, ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Milestone.ByID")
      {
        new Query.Param("pProj_ID", projID),
        new Query.Param("pID", id)
      };
    }

    public static Query<TRow> MilestonesByFilter<TRow>(MilestoneListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.MilestoneList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    public static Query<TRow> MilestonesByProjectID<TRow>(ulong projID) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Milestones.ByProjectID")
      {
        new Query.Param("pProj_ID", projID)
      };
    }

    public static Query<TRow> IssueByID<TRow>(ulong projID, ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Issue.ByID")
      {
        new Query.Param("pProj_ID", projID),
        new Query.Param("pID", id)
      };
    }

    public static Query<TRow> IssuesByFilter<TRow>(IssueListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.IssueList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    public static Query<TRow> AreaByID<TRow>(ulong projID, ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Area.ByID")
      {
        new Query.Param("pProj_ID", projID),
        new Query.Param("pID", id)
      };
    }

    public static Query<TRow> ComponentByID<TRow>(ulong projID, ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Component.ByID")
      {
        new Query.Param("pProj_ID", projID),
        new Query.Param("pID", id)
      };
    }
  }
}
