using System;
using NFX;
using Zhaba.Data;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Data.Domains;
using System.Reflection;
using Zhaba.Data.Forms;
using NFX.DataAccess.CRUD;
using NFX.DataAccess;
using NFX.Wave;

namespace Zhaba.DataLogic
{
  internal class ZhabaIssueLogLogic : LogicBase, IIssueLogic
  {
    #region .ctor
    public ZhabaIssueLogLogic(ZhabaDataStore store) : base(store)
    {
    }
    #endregion

    #region Public

    public void ReOpenIssue(ulong C_Project, ulong C_Issue, ulong C_User)
    {
      var query = QProject.ReopenIssue(C_Project, C_Issue);
      ZApp.Data.CRUD.ExecuteWithoutFetch(query);
      var evt = new ReopenIssueEvent()
      {
        C_Issue = C_Issue,
        C_User = C_User,
        DateUTC = DateTime.UtcNow
      };
      write(evt);
    }

    public void CloseIssue(ulong C_Project, ulong C_Issue, ulong C_User) 
    {
      try { 
        var query = QProject.DeleteIssueByProjectAndID(C_Project, C_Issue);
        ZApp.Data.CRUD.ExecuteWithoutFetch(query);
        var evt = new CloseIssueEvent()
        {
          C_Issue = C_Issue,
          C_User = C_User,
          DateUTC = DateTime.UtcNow
        };
        write(evt);
      }
      catch(Exception ex)
      {
        App.Log.Write(new NFX.Log.Message(ex)); 
      }
    }

    public Exception WriteIssueForm(IssueForm form, out object saveResult)
    {
      saveResult = null;

      IssueRow row = null;


      if (form.FormMode == FormMode.Insert)
      {
        row = new IssueRow(RowPKAction.Default);
      }
      else
      {
        var counter = form.RoundtripBag[ZhabaForm.ITEM_ID_BAG_PARAM].AsNullableULong();
        if (!counter.HasValue)
          throw HTTPStatusException.BadRequest_400("No Issue ID");

        var qry = QProject.IssueByID<IssueRow>(form.ProjectID, counter.Value);
        row = ZApp.Data.CRUD.LoadRow(qry);
        if (row == null)
          throw HTTPStatusException.NotFound_404("Issue");

      }

      form.CopyFields(row);

      var verror = row.ValidateAndPrepareForStore();
      if (verror != null) return verror;

      saveResult = row;

      try
      {
        if (form.FormMode == FormMode.Insert)
        {
          row.C_Project = form.ProjectID;
          ZApp.Data.CRUD.Insert(row);
        }
        else
        {
          var affected = ZApp.Data.CRUD.Update(row);
          if (affected < 1)
            throw HTTPStatusException.NotFound_404("Issue");
        }
      }
      catch (Exception error)
      {
        var eda = error as DataAccessException;
        if (eda != null && eda.KeyViolation != null)
          return new CRUDFieldValidationException(form, "Name", "This value is already used");

        throw;
      }

      try
      {
        if (form.FormMode == FormMode.Insert)
          write(new CreateIssueEvent()
          {
            C_Issue = row.Counter,
            C_Milestone = Convert.ToUInt64(form.C_Milestone),
            C_User = form.ZhabaUser.DataRow.Counter,
            DateUTC = DateTime.UtcNow,
            C_Category = Convert.ToUInt64(form.C_Category),
            Priority = form.Priority
          });
        else
          write(new EditIssueEvent()
          {
            C_Issue = row.Counter,
            C_Milestone = Convert.ToUInt64(form.C_Milestone),
            C_User = form.ZhabaUser.DataRow.Counter,
            DateUTC = DateTime.UtcNow,
            C_Category = Convert.ToUInt64(form.C_Category),
            Priority = form.Priority
          });
      }
      catch (Exception ex)
      {
        return ex;
      }

      return null;
    }


    public Exception WriteIssueAssignForm(IssueAssignForm from, out object saveResult)
    {
      Exception result = null;
      saveResult = null;
      try
      {
        using (var trn = ZApp.Data.CRUD.BeginTransaction())
        {
          var counter = from.RoundtripBag[ZhabaForm.ITEM_ID_BAG_PARAM].AsNullableULong();
          IssueAssignRow row = from.FormMode == FormMode.Edit && counter.HasValue
          ? trn.LoadRow(QIssueAssign.findIssueAssignByCounter<IssueAssignRow>(counter.Value))
          : new IssueAssignRow(RowPKAction.Default) { C_Issue = from.Issue.Counter, C_Open_Oper = from.ZhabaUser.DataRow.Counter };

          from.CopyFields(row, fieldFilter: (n, f) => f.Name != "C_Open_Oper" && f.Name != "C_Close_Oper" && f.Name != "C_Issue");

          result = row.ValidateAndPrepareForStore();
          if (result == null)
          {
            trn.Upsert(row);
            saveResult = row;

            var note = "";
            var query = QUser.FindAllActiveUserAndAssignedOnDate<UserRow>(from.Issue.Counter, DateTime.UtcNow);
            var usrs = trn.LoadEnumerable<UserRow>(query);
            foreach (UserRow item in usrs)
            {
              note += item.Login + "; ";
            }

            AssignIssueEvent evt = new AssignIssueEvent()
            {
              C_Issue = from.Issue.Counter,
              C_User = from.ZhabaUser.DataRow.Counter,
              DateUTC = DateTime.UtcNow,
              Note = note
            };
           write(evt);
          }
        }
      }
      catch (Exception ex)
      {
        result = ex;
      }

      return result;
    }

    public void WriteLogEvent(IssueLogEvent evt)
    {
      if (evt == null) throw new ZhabaDataException(StringConsts.ARGUMENT_ERROR+"WriteEvent(evt == null)");

      var error = evt.Validate();
      if(error != null ) throw new ZhabaDataException(StringConsts.ARGUMENT_ERROR + "WriteEvent(!evt.Validate())", error);
      
      var etp = evt.GetType();
      var mi = this.GetType().GetMethod("write", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { etp }, null);
      if (mi == null) throw new ZhabaDataException(StringConsts.ARGUMENT_ERROR + "WriteEvent(evt'{0}' not matched)".Args(etp.Name), error);

      try
      {
        mi.Invoke(this, new object[] { evt });
      }
      catch (Exception e)
      {
        var t = e is TargetInvocationException ?  e.InnerException : e ;
        throw new ZhabaDataException(StringConsts.ISSUELOG_EVENT_INVOCATION_ERROR.Args(t.ToMessageWithType()), t);
      }
    }

    #endregion

    #region Private

    private void write(CreateIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.NEW);
      newRow.C_Category = evt.C_Category;
      newRow.C_Milestone = evt.C_Milestone;
      newRow.Completeness = 0;
      newRow.Priority = evt.Priority;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(EditIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt);
      newRow.C_Category = evt.C_Category;
      newRow.C_Milestone = evt.C_Milestone;
      newRow.Completeness = 0;
      newRow.Priority = evt.Priority;
      newRow.Status = newRow.Status ?? ZhabaIssueStatus.NEW;
      ZApp.Data.CRUD.Insert(newRow);
    }
    private void write(ProceedIssueEvent evt) 
    {
      IssueLogRow newRow = NewIssueLog(evt);
      newRow.Completeness = evt.Completeness;
      newRow.Description = evt.Description;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(AssignIssueEvent evt)
    {
        IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.ASSIGNED);
        newRow.Note = evt.Note;
        ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(CloseIssueEvent evt) 
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.CLOSED);
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(ReopenIssueEvent evt) 
    {
      IssueLogRow newRow = NewIssueLog(evt,ZhabaIssueStatus.REOPEN);
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(DeferIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.DEFER);
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(DoneIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.DONE);
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(ChangePriorityIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt);
      newRow.Priority = evt.Priority;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(ChangeCategoryIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt);
      newRow.C_Category = evt.C_Category;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private IssueLogRow NewIssueLog(IssueLogEvent evt, string status = null)
    {
      IssueLogRow result = new IssueLogRow(RowPKAction.CtorGenerateNewID) 
      {
        C_Issue = evt.C_Issue,
        C_Operator = evt.C_User,
        Status_Date = evt.DateUTC
      };
      IssueLogRow oldRow = ZApp.Data.CRUD.LoadRow<IssueLogRow>(QIssueLog.FindLastIssueLogByIssue<IssueLogRow>(evt.C_Issue));
      if (oldRow != null)
      {
        oldRow.CopyFields(result,
          fieldFilter: (n, f) =>
              f.Name != "Counter" &&
              f.Name != "C_Issue" &&
              f.Name != "C_Operator" &&
              f.Name != "Status_Date"
        );
      }
      if (status != null) result.Status = status;
      return result;
    }

    #endregion
  }
}
