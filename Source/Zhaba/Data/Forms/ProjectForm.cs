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
        FormMode = FormMode.Insert;
    }

    [Field(typeof(ProjectRow))]
    public string Name { get; set; }

    [Field(typeof(ProjectRow))]
    public string Description { get; set; }
    
    [Field(targetName:"C_CREATOR", visible:false)]
    public ulong? C_Creator { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      try
      {

        var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        ProjectRow row = FormMode == FormMode.Edit && id.HasValue
          ? ZApp.Data.CRUD.LoadRow(QCommon.ProjectByID<ProjectRow>(id.Value))
          : new ProjectRow(RowPKAction.Default){C_Creator = ZhabaUser.DataRow.Counter};

        CopyFields(row, fieldFilter: (n, f) => f.Name != "C_Creator");

        var verror = row.ValidateAndPrepareForStore();

        if (verror != null) return verror;

        ZApp.Data.CRUD.Upsert(row);
        saveResult = row;

      }
      catch (DataAccessException error)
      {
        if (error != null && error.KeyViolation != null)
          return new CRUDFieldValidationException(this, "Name", "This value is already used");
      }
      catch (Exception ex)
      {
        return ex;
      }
      return null;
    }
  }
}

