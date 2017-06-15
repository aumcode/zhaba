using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Wave;

using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;

namespace Zhaba.Data.Forms
{
  public class ProjectForm : ZhabaForm
  {
    public ProjectForm() { }
    public ProjectForm(ulong? id)
    {
      if (id.HasValue)
      {
        FormMode = FormMode.Edit;

        var qry = QCommon.ProjectByID<ProjectRow>(id.Value);
        var row = ZApp.Data.CRUD.LoadRow(qry);
        if (row != null)
          row.CopyFields(this);
        else
          throw HTTPStatusException.NotFound_404("Project");

        this.RoundtripBag[ITEM_ID_BAG_PARAM] = id.Value;
      }
      else
      {
        FormMode = FormMode.Insert;
      }
    }

    [Field(typeof(ProjectRow))]
    public string Name { get; set; }

    [Field(typeof(ProjectRow))]
    public string Description { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      ProjectRow row = null;

      if (FormMode == FormMode.Insert)
      {
        row = new ProjectRow(RowPKAction.Default);
      }
      else
      {
        var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        if (!id.HasValue)
          throw HTTPStatusException.BadRequest_400("No Project ID");

        var qry = QCommon.ProjectByID<ProjectRow>(id.Value);
        row = ZApp.Data.CRUD.LoadRow(qry);
        if (row == null)
          throw HTTPStatusException.NotFound_404("Project");
      }

      CopyFields(row);

      var verror = row.ValidateAndPrepareForStore();
      if (verror != null) return verror;

      saveResult = row;

      try
      {
        if (FormMode == FormMode.Insert)
          ZApp.Data.CRUD.Insert(row);
        else
        {
          var affected = ZApp.Data.CRUD.Update(row);
          if (affected < 1)
            throw HTTPStatusException.NotFound_404("Project");
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

