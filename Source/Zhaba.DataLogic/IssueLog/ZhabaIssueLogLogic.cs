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
using System.Collections.Generic;

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

    public void ChangeStatus(ulong c_User, ulong c_Project, ulong c_Issue, string status, string description = null, ulong? c_AssignUser=null) 
    {
      IssueLogEvent evt = null;
      switch (status)
      {
        case ZhabaIssueStatus.REOPEN:
          evt = new ReopenIssueEvent()
          {
            C_User = c_User,
            C_Issue = c_Issue,
            DateUTC = App.TimeSource.UTCNow,
            Description = description
          };
          break;
        case ZhabaIssueStatus.DONE:
          evt = new DoneIssueEvent()
          {
            C_User = c_User,
            C_Issue = c_Issue,
            DateUTC = App.TimeSource.UTCNow,
            Description = description
          };
          break;
        case ZhabaIssueStatus.DEFER:
          IssueAssignClose(c_User, c_Issue);
          evt = new DeferIssueEvent()
          {
            C_User = c_User,
            C_Issue = c_Issue,
            DateUTC = App.TimeSource.UTCNow,
            Description = description
          };
          break;
        case ZhabaIssueStatus.CLOSED:
          IssueAssignClose(c_User, c_Issue);
          evt = new CloseIssueEvent()
          {
            C_User = c_User,
            C_Issue = c_Issue,
            DateUTC = App.TimeSource.UTCNow,
            Description = description
          };
          break;
        case ZhabaIssueStatus.CANCELED:
          IssueAssignClose(c_User, c_Issue);
          evt = new CancelIssueEvent()
          {
            C_User = c_User,
            C_Issue = c_Issue,
            DateUTC = App.TimeSource.UTCNow,
            Description = description
          };
          break;
      }
      if (evt != null) WriteLogEvent(evt);
    }

    public void ChangeProgess(ulong C_User, ulong issueCounter, int value, string description = null) 
    {
      var evt = new ProceedIssueEvent()
      {
        C_Issue = issueCounter,
        C_User = C_User,
        DateUTC = App.TimeSource.UTCNow, 
        Completeness = value,
        Description = description
      };
      try 
      {
        write(evt);
      }
      catch(Exception ex)
      {
        App.Log.Write(new NFX.Log.Message(ex));
      }
      
    }

    public void DeferIssue(ulong C_Project, ulong C_Issue, ulong C_User)
    {
      var evt = new DeferIssueEvent()
      {
        C_Issue = C_Issue,
        C_User = C_User,
        DateUTC = App.TimeSource.UTCNow
      };
      write(evt);
    }

    public void ReOpenIssue(ulong C_Project, ulong C_Issue, ulong C_User)
    {
      var query = QProject.ReopenIssue(C_Project, C_Issue);
      ZApp.Data.CRUD.ExecuteWithoutFetch(query);
      var evt = new ReopenIssueEvent()
      {
        C_Issue = C_Issue,
        C_User = C_User,
        DateUTC = App.TimeSource.UTCNow
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
          DateUTC = App.TimeSource.UTCNow
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
        //using (var trn = ZApp.Data.CRUD.BeginTransaction())
        {
          if (form.FormMode == FormMode.Insert)
          {
            row.C_Project = form.ProjectID;
            ZApp.Data.CRUD.Insert(row);
          }
          else
          {
            var affected =  ZApp.Data.CRUD.Update(row);
            if (affected < 1)
              throw HTTPStatusException.NotFound_404("Issue");
          }

          if (form.FormMode == FormMode.Insert)
            write(new CreateIssueEvent()
            {
              C_Issue = row.Counter,
              C_Milestone = Convert.ToUInt64(form.C_Milestone),
              C_User = form.ZhabaUser.DataRow.Counter,
              DateUTC = App.TimeSource.UTCNow,
              C_Category = Convert.ToUInt64(form.C_Category),
              Priority = form.Priority,
              Start_Date = form.Start_Date.Date,
              Due_Date = form.Due_Date.Date
            });
          else
            write(new EditIssueEvent()
            {
              C_Issue = row.Counter,
              C_Milestone = Convert.ToUInt64(form.C_Milestone),
              C_User = form.ZhabaUser.DataRow.Counter,
              DateUTC = App.TimeSource.UTCNow,
              C_Category = Convert.ToUInt64(form.C_Category),
              Priority = form.Priority,
              Start_Date = form.Start_Date.Date,
              Due_Date = form.Due_Date.Date
            });
          // trn.Commit();
        }
      }
      catch (Exception error)
      {
        var eda = error as DataAccessException;
        if (eda != null && eda.KeyViolation != null)
          return new CRUDFieldValidationException(form, "Name", "This value is already used");
      }
      return null;
    }


    public Exception WriteIssueAssignForm(IssueAssignForm form, out object saveResult)
    {
      Exception result = null;
      saveResult = null;
      try
      {
        using (var trn = ZApp.Data.CRUD.BeginTransaction())
        {
          var counter = form.RoundtripBag[ZhabaForm.ITEM_ID_BAG_PARAM].AsNullableULong();
          IssueAssignRow row = form.FormMode == FormMode.Edit && counter.HasValue
          ? trn.LoadRow(QIssueAssign.findIssueAssignByCounter<IssueAssignRow>(counter.Value))
          : new IssueAssignRow(RowPKAction.Default) { C_Issue = form.Issue.Counter, C_Open_Oper = form.ZhabaUser.DataRow.Counter };

          form.CopyFields(row, fieldFilter: (n, f) => f.Name != "C_Open_Oper" && f.Name != "C_Close_Oper" && f.Name != "C_Issue");

          result = row.ValidateAndPrepareForStore();
          if (result == null)
          {
            
            trn.Upsert(row);
            saveResult = row;

            AssignIssueEvent evt = new AssignIssueEvent()
            {
              C_Issue = form.Issue.Counter,
              C_User = form.ZhabaUser.DataRow.Counter,
              DateUTC = App.TimeSource.UTCNow,
              Description = form.Description
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

    public void IssueAssignClose(ulong c_User, ulong c_Issue)
    {
      var query = new Query("SQL.CRUD.IssueAssign.RemoveAllUsers")
      {
        new Query.Param("pClose_TS", App.TimeSource.UTCNow.Date),
        new Query.Param("pC_Issue", c_Issue),
        new Query.Param("pC_User", c_User)
      };
      ZApp.Data.CRUD.ExecuteWithoutFetch(query);
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
      newRow.Start_Date = evt.Start_Date;
      newRow.Due_Date = evt.Due_Date;
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
      newRow.Start_Date = evt.Start_Date;
      newRow.Due_Date = evt.Due_Date;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(ProceedIssueEvent evt) 
    {
      IssueLogRow newRow = NewIssueLog(evt);
      if (newRow.Status == ZhabaIssueStatus.CLOSED) return;
      newRow.Completeness = evt.Completeness;
      newRow.Description = evt.Description;
//      if (evt.Completeness == 100) newRow.Status = ZhabaIssueStatus.DONE;
      ZApp.Data.CRUD.Insert(newRow);
    }

    private void write(AssignIssueEvent evt)
    {
        IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.ASSIGNED);
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

    private void write(CancelIssueEvent evt)
    {
      IssueLogRow newRow = NewIssueLog(evt, ZhabaIssueStatus.CANCELED);
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
        if (status != null && !new List<string>(ZhabaIssueStatus.NextState(oldRow.Status)).Contains(status))
          throw new ZhabaException("Wrong state {0} -> {1} for Issue ID = {2}".Args(oldRow.Status, status, evt.C_Issue));

        oldRow.CopyFields(result,
          fieldFilter: (n, f) =>
              f.Name != "Counter" &&
              f.Name != "C_Issue" &&
              f.Name != "C_Operator" &&
              f.Name != "Status_Date" 
        );
      }
      else if (status != ZhabaIssueStatus.NEW && !(evt is EditIssueEvent)) throw new ZhabaException("Issue(ID={0}) Log not present for status ({1})".Args(evt.C_Issue, status));

      if (status != null) result.Status = status;
      if (evt.Description.IsNotNullOrWhiteSpace()) result.Description = evt.Description;

      return result;
    }

    #endregion
  }
}
