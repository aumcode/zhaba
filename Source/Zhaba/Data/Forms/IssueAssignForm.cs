using System;
using NFX.DataAccess.CRUD;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using NFX;
using NFX.Wave;
using NFX.Serialization.JSON;
using Zhaba.Data.Domains;

namespace Zhaba.Data.Forms
{
  public class IssueAssignForm :IssueFormBase
  {
    #region .ctor
    public IssueAssignForm() { }
    public IssueAssignForm(ProjectRow project, IssueRow issue, ulong? counter) : base(project, issue)
    {
      if (counter.HasValue) 
      {
        FormMode = FormMode.Edit;
        var row = ZApp.Data.CRUD.LoadRow(QIssueAssign.findIssueAssignByCounter<IssueAssignRow>(counter.Value));
        if (row != null)
        {
          row.CopyFields(this);
          this.Close_TS = App.TimeSource.UTCNow.Date;
        }
        else
          throw HTTPStatusException.NotFound_404("Project");
        this.RoundtripBag[ITEM_ID_BAG_PARAM] = counter.Value;
      }
    }
    #endregion

    #region Fields
    [Field(typeof(IssueAssignRow))]
    public ulong C_Issue { get; set; }
    [Field(typeof(IssueAssignRow))]
    public string C_User { get; set; }
    [Field(typeof(IssueAssignRow))]
    public DateTime Open_TS { get; set; } = App.TimeSource.UTCNow.Date;
    [Field(typeof(IssueAssignRow))]
    public DateTime? Close_TS { get; set; }
    [Field(typeof(IssueAssignRow))]
    public ulong? C_Close_Meeting { get; set; }
    [Field(typeof(IssueAssignRow))]
    public ulong? C_Open_Meeting { get; set; }
    [Field(typeof(IssueAssignRow))]
    public string Note { get; set; }
    [Field(required: false,
      kind: DataKind.Text,
      minLength: ZhabaMnemonic.MIN_LEN,
      maxLength: ZhabaMnemonic.MAX_LEN,
      description: "Description",
      metadata: @"Placeholder='Description'")]
    public string Description { get; set; }

    #endregion

    #region Public

    public override JSONDataMap GetClientFieldValueList(object callerContext, Schema.FieldDef fdef, string targetName, string isoLang)
    {
      var user = fdef.Name.EqualsIgnoreCase("C_User");
      JSONDataMap result = null;
      if (user)
      {
        if (FormMode == FormMode.Edit)
        {
          int counter = RoundtripBag[ITEM_ID_BAG_PARAM].AsInt();
          result = new JSONDataMap();
          UserForPM item = (UserForPM) ZApp.Data.CRUD.LoadOneRow(QUser.FindUserPmById<UserForPM>(counter));
          result.Add(item.Counter.ToString(), item.FullName);
        }
        else
        {
          var users = ZApp.Data.CRUD.LoadEnumerable<UserForPM>(QUser.FindAllActiveUserAndNotAssignedOnDate<UserForPM>(Issue.Counter, App.TimeSource.UTCNow));
          result = new JSONDataMap();
          foreach (UserForPM item in users)
          {
            result.Add(item.Counter.ToString(), item.FullName);
          }
        }
      }
      return result;
    }

    #endregion

    #region Protected
    protected override Exception DoSave(out object saveResult)
    {
      return ZApp.Data.Issue.WriteIssueAssignForm(this, out saveResult);
    }
    #endregion
  }
}
