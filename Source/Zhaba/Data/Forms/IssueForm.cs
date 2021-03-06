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
    public const int DEFAULT_PRIORITY = 10;
    private const string DEFAULT_ISSUE_INSERT_DESCRIPTION = "Created";

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

        var issueLogQry = QIssueLog.FindLastIssueLogByIssue<IssueLogRow>(counter.Value);
        var issueLog = ZApp.Data.CRUD.LoadRow(issueLogQry);
        if (issueLog != null)
        {
          C_Category = issueLog.C_Category.ToString();
          C_Milestone = issueLog.C_Milestone.ToString();
          Priority = issueLog.Priority;
          Start_Date = issueLog.Start_Date;
          Due_Date = issueLog.Due_Date;
          Description = issueLog.Description;
        }
        else
        {
          Start_Date = App.TimeSource.UTCNow.Date;
          Priority = DEFAULT_PRIORITY;
        }

        RoundtripBag[ITEM_ID_BAG_PARAM] = counter.Value;
      }
      else
      {
        FormMode = FormMode.Insert;
        Start_Date = App.TimeSource.UTCNow.Date;
        Priority = DEFAULT_PRIORITY;
        Description = DEFAULT_ISSUE_INSERT_DESCRIPTION;
      }
    }
    #endregion

    #region Field
    [Field(typeof(IssueRow))]
    public string Name { get; set; }

    [Field(required: true,
           description: "Milestone",
           metadata: @"Placeholder='Milestone'")]
    public String C_Milestone { get; set; }

    [Field(required: true, 
           description: "Category",
           metadata: @"Placeholder='Category'")]
    public string C_Category { get; set; }
    
    [Field(required: true, 
      kind: DataKind.DateTime, 
      description: "Start date",
      metadata: @"Placeholder='Issue start date'")]
    public DateTime? Start_Date { get; set; }

    [Field(required: true, 
      kind: DataKind.DateTime, 
      description: "Due date",
      metadata: @"Placeholder='Issue due date'")]
    public DateTime? Due_Date { get; set; }

    [Field(required: true,
      description: "Priority",
      metadata: @"Placeholder='Priority'")]
    public ulong Priority { get; set; }

    [Field(required: false,
           description: "Description",
           metadata: @"Placeholder='Description'")]
    public string Description { get; set; }
    #endregion

    #region Public

    public override Exception Validate()
    {
      var result = base.Validate();
      if (result == null)
      {
        if (Start_Date.HasValue && Due_Date.HasValue && Start_Date.Value.CompareTo(Due_Date.Value) > 0 )
        {
          result = new ZhabaException("Start Date {0} > Due Date {1}".Args(Start_Date, Due_Date));
        }
      }
      return result;
    }

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
      else if (fdef.Name.EqualsIgnoreCase("C_Milestone")) 
      {
        var milestoneFilter = new Filters.MilestoneListFilter();
        milestoneFilter.____SetProject(ProjectRow);
        var milestones = ZApp.Data.CRUD.LoadEnumerable<MilestoneRow>(QProject.MilestonesByFilter<MilestoneRow>(milestoneFilter));
        result = new JSONDataMap();
        foreach(MilestoneRow item in milestones) 
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
      var result = this.Validate();
      if (result != null) return result;
      return ZApp.Data.Issue.WriteIssueForm(this, out saveResult);
    }
    #endregion
  }
}

