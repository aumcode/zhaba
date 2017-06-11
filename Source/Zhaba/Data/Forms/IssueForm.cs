using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using NFX.Wave;

using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Domains;

namespace Zhaba.Data.Forms
{
  public class IssueForm : ProjectFormBase
  {
    public IssueForm() { }
    public IssueForm(ProjectRow project, ulong? id) : base(project)
    {
      if (id.HasValue)
      {
        FormMode = FormMode.Edit;

        var qry = QProject.IssueByID<IssueRow>(ProjectID, id.Value);
        var row = ZApp.Data.CRUD.LoadRow(qry);
        if (row != null)
        {
          row.CopyFields(this);
          this.Milestone = row.C_Milestone.AsString();
          this.Area = row.C_Area.AsString();
          this.Component = row.C_Component.AsString();
        }
        else
          throw HTTPStatusException.NotFound_404("Issue");

        this.RoundtripBag[ITEM_ID_BAG_PARAM] = id.Value;
      }
      else
      {
        FormMode = FormMode.Insert;
      }
    }

    [Field(typeof(IssueRow))]
    public string Name { get; set; }

    [Field(typeof(IssueRow))]
    public string Description { get; set; }

    [Field]
    public string Milestone { get; set; }

    [Field]
    public string Area { get; set; }

    [Field]
    public string Component { get; set; }

    [Field(typeof(IssueRow))]
    public string Status { get; set; }

    [Field(typeof(IssueRow))]
    public string Owner { get; set; }

    [Field(maxLength: ZhabaDescription.MAX_LEN,
           kind: DataKind.Text,
           description: "Comment",
           metadata: @"Placeholder='Comment' ControlType='textarea'")]
    public string Comment { get; set; }


    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      if (fdef.Name.EqualsIgnoreCase("Milestone"))
      {
        var qry = QProject.MilestonesByProjectID<MilestoneRow>(ProjectID);
        var rowset = ZApp.Data.CRUD.LoadEnumerable(qry);
        if (rowset != null)
        {
          var result = new JSONDataMap();
          foreach (var row in rowset)
            result.Add(row.Counter.ToString(), row.Name);

          return result;
        }
      }
      if (fdef.Name.EqualsIgnoreCase("Area"))
      {
        var qry = QCommon.AllAreas<AreaRow>();
        var rowset = ZApp.Data.CRUD.LoadEnumerable(qry);
        if (rowset != null)
        {
          var result = new JSONDataMap();
          foreach (var row in rowset)
            result.Add(row.Counter.ToString(), row.Name);

          return result;
        }
      }
      if (fdef.Name.EqualsIgnoreCase("Component"))
      {
        var qry = QCommon.AllComponents<ComponentRow>();
        var rowset = ZApp.Data.CRUD.LoadEnumerable(qry);
        if (rowset != null)
        {
          var result = new JSONDataMap();
          foreach (var row in rowset)
            result.Add(row.Counter.ToString(), row.Name);

          return result;
        }
      }
      if (fdef.Name.EqualsIgnoreCase("Owner"))
      {
        var qry = QUser.AllUserInfos<UserInfo>();
        var rowset = ZApp.Data.CRUD.LoadEnumerable(qry);
        if (rowset != null)
        {
          var result = new JSONDataMap();
          foreach (var row in rowset)
            result.Add(row.Login, row.Login);

          return result;
        }
      }

      return null;
    }

    protected override Exception DoSave(out object saveResult)
    {
      saveResult = null;
      IssueRow row = null;

      if (FormMode == FormMode.Insert)
      {
        row = new IssueRow(RowULongPKAction.Default);
        row.C_Project = ProjectID;
        row.Creation_Date = App.TimeSource.UTCNow;
        row.Creator = this.ZhabaUser.DataRow.Login;
      }
      else
      {
        var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        if (!id.HasValue)
          throw HTTPStatusException.BadRequest_400("No Issue ID");

        var qry = QProject.IssueByID<IssueRow>(ProjectID, id.Value);
        row = ZApp.Data.CRUD.LoadRow(qry);
        if (row == null)
          throw HTTPStatusException.NotFound_404("Issue");
      }

      CopyFields(row);
      row.C_Milestone = this.Milestone.AsNullableULong(null);
      row.C_Area = this.Area.AsNullableULong(null);
      row.C_Component = this.Component.AsNullableULong(null);

      var verror = row.ValidateAndPrepareForStore();
      if (verror != null) return verror;

      saveResult = row;

      try
      {
        var log = new IssueLogRow(RowULongPKAction.Default)
        {
          C_Project = ProjectID,
          Creation_Date = App.TimeSource.UTCNow,
          Creator = this.ZhabaUser.DataRow.Login,
          C_Issue = row.Counter,
          Status = row.Status
        };

        if (FormMode == FormMode.Insert)
        {
          ZApp.Data.CRUD.Insert(row);
          log.Description = row.Description;
        }
        else
        {
          var affected = ZApp.Data.CRUD.Update(row);
          if (affected < 1)
            throw HTTPStatusException.NotFound_404("Issue");

          log.Description = this.Comment;
        }

        log.ValidateAndPrepareForStore();
        ZApp.Data.CRUD.Insert(log);
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

