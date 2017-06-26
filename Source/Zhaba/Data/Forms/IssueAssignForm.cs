using System;
using NFX.DataAccess.CRUD;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using NFX;
using NFX.Wave;

namespace Zhaba.Data.Forms
{
  public class IssueAssignForm :ProjectFormBase
  {
    #region .ctor
    public IssueAssignForm() { }
    public IssueAssignForm(ProjectRow project, IssueRow issue, ulong? counter) : base(project)
    {
      m_Issue = issue;
      if (counter.HasValue) 
      {
        FormMode = FormMode.Edit;
        var row = ZApp.Data.CRUD.LoadRow(QIssueAssign.findIssueAssignByCounter<IssueAssignRow>(counter.Value));
        if (row != null)
          row.CopyFields(this);
        else
          throw HTTPStatusException.NotFound_404("Project");
        this.RoundtripBag[ITEM_ID_BAG_PARAM] = counter.Value;
      }
    }
    #endregion

    [NonSerialized]
    private IssueRow m_Issue;

    #region Fields
    [Field(typeof(IssueAssignRow))]
    public ulong C_Issue { get; set; }
    [Field(typeof(IssueAssignRow))]
    public ulong C_User { get; set; }
    [Field(typeof(IssueAssignRow))]
    public DateTime Open_TS { get; set; }
    [Field(typeof(IssueAssignRow))]
    public DateTime? Close_TS { get; set; }
    [Field(typeof(IssueAssignRow))]
    public ulong? C_CloseMeeting { get; set; }
    [Field(typeof(IssueAssignRow))]
    public ulong? C_OpenMeeting { get; set; }
    [Field(typeof(IssueAssignRow))]
    public string Note { get; set; }

    public IssueRow Issue { get { return m_Issue; } }
    public ulong IssueID { get { return m_Issue.Counter; } } 

    #endregion

    #region Public

    #endregion

    #region Protected
    protected override Exception DoSave(out object saveResult)
    {
      Exception result = null;
      saveResult = null;
      try 
      {
        var counter = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        IssueAssignRow row = FormMode == FormMode.Edit && counter.HasValue  
        ? ZApp.Data.CRUD.LoadRow(QIssueAssign.findIssueAssignByCounter<IssueAssignRow>(counter.Value)) 
        : new IssueAssignRow(RowPKAction.Default) { C_OpenOper = ZhabaUser.DataRow.Counter };
        
        CopyFields(row, fieldFilter: (n, f) => f.Name != "C_OpenOper" && f.Name != "C_CloseOper");

        result = row.ValidateAndPrepareForStore();
        if (result != null) 
        {
          ZApp.Data.CRUD.Upsert(row);
          saveResult = row;
        }

      } 
      catch(Exception ex) 
      {
        result = ex;
      }

      return result;
    }

    public void ____SetIssue(IssueRow issue)
    {
      m_Issue = issue;
    }
    #endregion
  }
}
