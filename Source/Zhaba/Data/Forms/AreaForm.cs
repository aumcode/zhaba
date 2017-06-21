using System;
using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Wave;

using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Forms
{
  public class AreaForm : ProjectFormBase
  {
    public AreaForm() { }
    public AreaForm(ProjectRow project, ulong? id) 
      : base(project)
    {
      if (id.HasValue)
      {
        FormMode = FormMode.Edit;

        var qry = QProject.AreaByID<AreaRow>(ProjectID, id.Value);
        var row = ZApp.Data.CRUD.LoadRow(qry);
        if (row != null)
          row.CopyFields(this);
        else
          throw HTTPStatusException.NotFound_404("Area");

        this.RoundtripBag[ITEM_ID_BAG_PARAM] = id.Value;
      }
      else
      {
        FormMode = FormMode.Insert;
      }
    }

    [Field(typeof(AreaRow))]
    public string Name { get; set; }

    [Field(typeof(AreaRow))]
    public string Description { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      AreaRow row = null;

      if (FormMode == FormMode.Insert)
      {
        row = new AreaRow(RowPKAction.Default);
      }
      else
      {
        var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        if (!id.HasValue)
          throw HTTPStatusException.BadRequest_400("No Area ID");

        var qry = QProject.AreaByID<AreaRow>(ProjectID, id.Value);
        row = ZApp.Data.CRUD.LoadRow(qry);
        if (row == null)
          throw HTTPStatusException.NotFound_404("Area");
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
            throw HTTPStatusException.NotFound_404("Area");
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
