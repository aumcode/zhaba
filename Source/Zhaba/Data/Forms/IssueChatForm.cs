using System;
using NFX;
using Zhaba.Data.Rows;
using NFX.DataAccess.CRUD;
using NFX.Wave;
using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Forms
{
  public class IssueChatForm : IssueFormBase
  {
    #region .ctor
    public IssueChatForm()
    {
      
    }
    public IssueChatForm(ProjectRow project, IssueRow issue, ulong? counter) : base(project, issue)
    {
      if (counter.HasValue) 
      {
        FormMode = FormMode.Edit;
        var row = ZApp.Data.CRUD.LoadRow(QIssueChat.findIssueChatByIdAndIssueAndProject<IssueChatRow>(ProjectRow.Counter, Issue.Counter, counter.Value));
        if (row != null)
        {
          if (row.C_User == ZhabaUser.DataRow.Counter)
          {
            row.CopyFields(this);  
          }
          else
            throw HTTPStatusException.NotFound_404("Wrong user for edit ({0} != {1})".Args(row.C_User, ZhabaUser.DataRow.Counter));
        }
        else
          throw HTTPStatusException.NotFound_404("project or issue");
        
        this.RoundtripBag[ITEM_ID_BAG_PARAM] = counter.Value;
      }
      else
        FormMode = FormMode.Insert;
    }
    
    #endregion

    #region Properties

    [Field(typeof(IssueChatRow))]
    public string Note { get; set; }


    #endregion

    #region public

    #endregion

    #region protected

    protected override Exception DoSave(out object saveResult)
    {
      Exception result = null;
      saveResult = null;
      if (Note.IsNullOrEmpty()) return null;
      try
      {
        var id = RoundtripBag[ITEM_ID_BAG_PARAM].AsNullableULong();
        IssueChatRow row;
        if (FormMode == FormMode.Edit && id.HasValue)
        {
          row = ZApp.Data.CRUD.LoadRow(
            QIssueChat.findIssueChatByIdAndIssueAndProject<IssueChatRow>(ProjectRow.Counter, Issue.Counter, id.Value));
        }
        else
        {
          row = new IssueChatRow(RowPKAction.CtorGenerateNewID)
          {
            C_User = ZhabaUser.DataRow.Counter, 
            C_Issue = IssueID, 
            Note_Date = App.TimeSource.UTCNow
          }; 
        }
        
        if (row != null)
        {
          if (row.C_User == ZhabaUser.DataRow.Counter)
          {
            CopyFields(row, fieldFilter:(n,f) => f.Name != "Counter");  
          }
          else
            throw new ZhabaException("Wrong user for edit ({0} != {1})".Args(row.C_User, ZhabaUser.DataRow.Counter));
        }
        else
          throw new ZhabaException("Wrong project or issue");
        
        result = row.ValidateAndPrepareForStore();
        if (result == null)
        {
          ZApp.Data.CRUD.Upsert(row);
          saveResult = row;
        }
      }
      catch (Exception ex)
      {
        result = ex;
      }
      
      return result;
    }

    #endregion
    
    
  }
}