using System;
using System.Collections.Generic;
using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Wave;

using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Forms
{
  /// <summary>
  /// Form for create or edit Issue
  /// </summary>
  public class IssueForm : ProjectFormBase
  {
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

        RoundtripBag[ITEM_ID_BAG_PARAM] = counter.Value;
      }
      else
      {
        FormMode = FormMode.Insert;
      }
    }

    [Field]
    public string Name { get; set; }

    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      IssueRow row = null;

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
      }

      CopyFields(row);

      var verror = row.ValidateAndPrepareForStore();
      if (verror != null) return verror;

      saveResult = row;

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

      return null;
    }
  }
}

