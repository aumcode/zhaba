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
  public class ComponentForm : ZhabaForm
  {
    public ComponentForm() { }
    public ComponentForm(ulong? id)
    {
      if (id.HasValue)
      {
        FormMode = FormMode.Edit;

        var qry = QCommon.ComponentByID<ComponentRow>(id.Value);
        var row = ZApp.Data.CRUD.LoadRow(qry);
        if (row != null)
          row.CopyFields(this);
        else
          throw HTTPStatusException.NotFound_404("Component");

        this.RoundtripBag[ITEM_ID_BAG_PARAM] = id.Value;
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
        row = new ComponentRow(RowULongPKAction.Default);
      }
      else
      {
        var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        if (!id.HasValue)
          throw HTTPStatusException.BadRequest_400("No Component ID");

        var qry = QCommon.ComponentByID<ComponentRow>(id.Value);
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
          ZApp.Data.CRUD.Insert(row);
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