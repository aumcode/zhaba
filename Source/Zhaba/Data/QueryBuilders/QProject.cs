using NFX.DataAccess.CRUD;
using Zhaba.Data.Filters;

namespace Zhaba.Data.QueryBuilders
{
  /// <summary>
  /// Shortcuts for project data query
  /// </summary>
  public static class QProject
  {
    #region Projects

    /// <summary>
    /// Get all projects
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> AllProjects<TRow>() where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Project.All");
    }

    #endregion Projects

    #region Milestones

    /// <summary>
    /// Get all milestone by name
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> MilestoneByUK<TRow>(ulong projID, string msName) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Milestone.ByUK")
      {
        new Query.Param("pProj_ID", projID),
        new Query.Param("pName", msName)
      };
    }

    /// <summary>
    /// Get milestone by ID
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> MilestoneByID<TRow>(ulong projID, ulong id) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Milestone.ByID")
      {
        new Query.Param("pProj_ID", projID),
        new Query.Param("pID", id)
      };
    }

    /// <summary>
    /// Get all milestones by filter
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> MilestonesByFilter<TRow>(MilestoneListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.MilestoneList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    /// <summary>
    /// Get all milestones by project ID
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> MilestonesByProjectID<TRow>(ulong projID) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Milestones.ByProjectID")
      {
        new Query.Param("pProj_ID", projID)
      };
    }

    #endregion Milestones

    #region Issues

    /// <summary>
    /// Get issue by ID
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> IssueByID<TRow>(ulong projCounter, ulong counter) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Issue.ByID")
      {
        new Query.Param("pProj_ID", projCounter),
        new Query.Param("pID", counter)
      };
    }

    /// <summary>
    /// Get all issues by filter
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> IssuesByFilter<TRow>(IssueListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.IssueList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    #endregion Issues

    #region Components

    /// <summary>
    /// Get component by ID
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> ComponentByID<TRow>(ulong projCounter, ulong counter) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Component.ByID")
      {
        new Query.Param("pProj_Counter", projCounter),
        new Query.Param("pCounter", counter)
      };
    }

    /// <summary>
    /// Get all components by filter
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> ComponentsByFilter<TRow>(ComponentListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.ComponentList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    #endregion Components

    #region Areas

    /// <summary>
    /// Get all areas by filter
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> AreasByFilter<TRow>(AreaListFilter filter) where TRow : Row
    {
      return new Query<TRow>("SQL.Filters.AreaList")
      {
        new Query.Param("pFilter", filter)
      };
    }

    /// <summary>
    /// Get area by ID
    /// </summary>
    /// <typeparam name="TRow">Type of row</typeparam>
    /// <returns>Query</returns>
    public static Query<TRow> AreaByID<TRow>(ulong projCounter, ulong counter) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Area.ByID")
      {
        new Query.Param("pProj_Counter", projCounter),
        new Query.Param("pCounter", counter)
      };
    }

    #endregion Areas
  }
}
