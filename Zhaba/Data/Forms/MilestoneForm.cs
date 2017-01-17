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
  public class MilestoneForm : ProjectFormBase
  {
    public MilestoneForm() { }
    public MilestoneForm(ProjectRow project, ulong? id) : base(project)
    {
      if (id.HasValue)
      {
        FormMode = FormMode.Edit;

        var qry = QProject.MilestoneByID<MilestoneRow>(ProjectID, id.Value);
        var row = ZApp.Data.CRUD.LoadRow(qry);
        if (row != null)
          row.CopyFields(this);
        else
          throw HTTPStatusException.NotFound_404("Milestone");

        this.RoundtripBag[ITEM_ID_BAG_PARAM] = id.Value;
      }
      else
      {
        FormMode = FormMode.Insert;
      }
    }

    [Field(required: true)]
    public string Name { get; set; }

    [Field(required: false)]
    public string Description { get; set; }

    [Field(required: false)]
    public DateTime? Start_Date { get; set; }

    [Field(required: false)]
    public DateTime? Plan_Date { get; set; }

    [Field(required: false)]
    public DateTime? Complete_Date { get; set; }


    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      MilestoneRow row = null;

      if (FormMode == FormMode.Insert)
      {
        row = new MilestoneRow(RowULongPKAction.Default);
        row.C_Project = ProjectID;
      }
      else
      {
        var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        if (!id.HasValue)
          throw HTTPStatusException.BadRequest_400("No Milestone ID");

        var qry = QProject.MilestoneByID<MilestoneRow>(ProjectID, id.Value);
        row = ZApp.Data.CRUD.LoadRow(qry);
        if (row == null)
          throw HTTPStatusException.NotFound_404("Milestone");
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
            throw HTTPStatusException.NotFound_404("Milestone");
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
