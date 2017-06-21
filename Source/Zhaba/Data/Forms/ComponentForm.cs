using System;
using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Wave;

using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Forms
{
  public class ComponentForm : ProjectFormBase
  {
    public ComponentForm() { }

    public ComponentForm(ProjectRow project, ulong? counter) 
      : base(project)
    {
      if (counter.HasValue)
      {
        FormMode = FormMode.Edit;

        var qry = QProject.ComponentByID<ComponentRow>(ProjectID, counter.Value);
        var row = ZApp.Data.CRUD.LoadRow(qry);
        if (row != null)
          row.CopyFields(this);
        else
          throw HTTPStatusException.NotFound_404("Component");

        this.RoundtripBag[ITEM_ID_BAG_PARAM] = counter.Value;
      }
      else
      {
        FormMode = FormMode.Insert;
      }
    }

    [Field(typeof(ComponentRow))]
    public string Name { get; set; }

    [Field(typeof(ComponentRow))]
    public string Description { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      ComponentRow row = null;

      if (FormMode == FormMode.Insert)
      {
        row = new ComponentRow(RowPKAction.Default);
      }
      else
      {
        var counter = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        if (!counter.HasValue)
          throw HTTPStatusException.BadRequest_400("No Component ID");

        var qry = QProject.ComponentByID<ComponentRow>(ProjectID, counter.Value);
        row = ZApp.Data.CRUD.LoadRow(qry);
        if (row == null)
          throw HTTPStatusException.NotFound_404("Component");
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
            throw HTTPStatusException.NotFound_404("Component");
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