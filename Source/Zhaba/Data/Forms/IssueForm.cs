﻿using System;
using System.Collections.Generic;
using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Wave;

using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;
using NFX.Serialization.JSON;

namespace Zhaba.Data.Forms
{
  /// <summary>
  /// Form for create or edit Issue
  /// </summary>
  public class IssueForm : ProjectFormBase
  {
    #region .ctor
    public IssueForm() { }

    public IssueForm(ProjectRow project, ulong? counter) 
      : base(project)
    {
      if (counter.HasValue)
      {
        FormMode = FormMode.Edit;

        var issueQry = QProject.IssueByID<IssueRow>(ProjectID, counter.Value);
        var row = ZApp.Data.CRUD.LoadRow(issueQry);
        if (row != null)
          row.CopyFields(this);
        else
          throw HTTPStatusException.NotFound_404("Issue");

        var milestoneQry = QIssueLog.FindMilestoneByIssue<MilestoneRow>(counter.Value);
        var milestone = ZApp.Data.CRUD.LoadRow(milestoneQry);
        if (milestone != null)
        {
          milestone.CopyFields(this, fieldFilter: (n, f) => f.Name != "Name");
        }

        var issueLogQry = QIssueLog.FindLastIssueLogByIssue<IssueLogRow>(counter.Value);
        var issueLog = ZApp.Data.CRUD.LoadRow(issueLogQry);
        if (issueLog != null)
        {
          C_Category = issueLog.C_Category.ToString();
          Priority = issueLog.Priority;
        }

        RoundtripBag[ITEM_ID_BAG_PARAM] = counter.Value;
      }
      else
      {
        FormMode = FormMode.Insert;
        Start_Date = DateTime.UtcNow;
      }
    }
    #endregion

    private MilestoneRow m_Milestone;

    #region Field
    [Field]
    public string Name { get; set; }

    [Field(typeof(MilestoneRow))]
    public string Description { get; set; }

    [Field(typeof(MilestoneRow))]
    public DateTime? Start_Date { get; set; }

    [Field(typeof(MilestoneRow))]
    public DateTime? Plan_Date { get; set; }

    [Field(typeof(MilestoneRow))]
    public DateTime? Complete_Date { get; set; }
    
    [Field(required: true)]
    public string C_Category { get; set; }

    [Field(required: true)]
    public ulong Priority { get; set; }
    #endregion

    #region Public

    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      var category = fdef.Name.EqualsIgnoreCase("C_Category");
      JSONDataMap result = null;
      if (category)
      {
        var categories = ZApp.Data.CRUD.LoadEnumerable<CategoryRow>(QCategory.findCategoryByFilter<CategoryRow>(new Filters.CategoryListFilter()));
        result = new JSONDataMap();
        foreach (CategoryRow item in categories)
        {
          result.Add(item.Counter.ToString(), item.Name);
        }
      }
      return result;
    }

    #endregion

    #region Protected
    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;

      IssueRow row = null;
      MilestoneRow milestoneRow = null;

      if (FormMode == FormMode.Insert)
      {
        row = new IssueRow(RowPKAction.Default);
      }
      else
      {
        var counter = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        if (!counter.HasValue)
          throw HTTPStatusException.BadRequest_400("No Issue ID");

        var qry = QProject.IssueByID<IssueRow>(ProjectID, counter.Value);
        row = ZApp.Data.CRUD.LoadRow(qry);
        if (row == null)
          throw HTTPStatusException.NotFound_404("Issue");

        var milestoneQry = QIssueLog.FindMilestoneByIssue<MilestoneRow>(counter.Value);
        milestoneRow = ZApp.Data.CRUD.LoadRow(milestoneQry);

      }

      CopyFields(row);

      var verror = row.ValidateAndPrepareForStore();
      if (verror != null) return verror;

      saveResult = row;

      try
      {
        if (milestoneRow == null)
          milestoneRow = new MilestoneRow(RowPKAction.CtorGenerateNewID) { C_Project = this.ProjectID };
      
        CopyFields(milestoneRow, fieldFilter: (n, f) => f.Name != "Counter" && f.Name != "C_Project");
        
        ZApp.Data.CRUD.Upsert(milestoneRow);
      }
      catch (Exception ex)
      {
        return ex;
      }

      try
      {
        if (FormMode == FormMode.Insert)
        {
          row.C_Project = ProjectID;
          ZApp.Data.CRUD.Insert(row);
        }
        else
        {
          var affected = ZApp.Data.CRUD.Update(row);
          if (affected < 1)
            throw HTTPStatusException.NotFound_404("Issue");
        }
      }
      catch (Exception error)
      {
        var eda = error as DataAccessException;
        if (eda != null && eda.KeyViolation != null)
          return new CRUDFieldValidationException(this, "Name", "This value is already used");

        throw;
      }


      try
      {
        CreateIssueEvent evt = new CreateIssueEvent()
        {
          C_Issue = row.Counter,
          C_Milestone = milestoneRow.Counter,
          C_User = ZhabaUser.DataRow.Counter,
          DateUTC = DateTime.UtcNow,
          C_Category = Convert.ToUInt64(this.C_Category),
          Priority = this.Priority
        };
        ZApp.Data.IssueLog.WriteEvent(evt);
      }
      catch (Exception ex)
      {
        return ex;
      }

      return null;
    }
    #endregion
  }
}

