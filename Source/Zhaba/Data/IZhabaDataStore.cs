using System;

using NFX.DataAccess;
using NFX.ApplicationModel.Pile;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Rows;
using Zhaba.Data.Forms;
using Zhaba.Data.Reports;

namespace Zhaba.Data
{
  public interface IZhabaDataStore : IDataStore
  {
    ICRUDDataStore CRUD { get; }
    IUniqueSequenceProvider SequenceProvider { get; }
    ICache Cache { get; }

    IUserLogic Users { get; }
    IIssueLogic Issue { get; }
    IReportLogic ReportLogic { get; }
  }


  public interface IStoreLogic
  {
    IZhabaDataStore Store { get; }
  }


  public interface IUserLogic : IStoreLogic
  {
    /// <summary>
    /// Returns User by given login
    /// </summary>
    UserRow GetUser(string login);

    UserRow GetUserByToken(NFX.Security.AuthenticationToken token);

    NFX.Security.AuthenticationToken CreateToken(UserRow row);
  }


  /// <summary>
  /// Work with Issue Log
  /// </summary>
  public interface IIssueLogic : IStoreLogic
  {
    void WriteLogEvent(IssueLogEvent evt, ICRUDOperations operations = null);
    Exception WriteIssueForm(IssueForm form, out object saveResult);
    Exception WriteIssueAssignForm(IssueAssignForm form, out object saveResult);
    void CloseIssue(ulong C_Project, ulong C_Issue, ulong C_User);
    void ReOpenIssue(ulong C_Project, ulong C_Issue, ulong C_User);
    void DeferIssue(ulong C_Project, ulong C_Issue, ulong C_User);
    void ChangeProgess(ulong C_User, ulong issueCounter, int value, string description = null);
    void ChangeStatus(ulong c_User, ulong c_Project, ulong c_Issue, string status, string description, ulong? c_AssignedUser = null);
  }

  public interface IReportLogic : IStoreLogic
  {
    Exception DueItemReport(DueItemsReport form, out object saveResult);
  }
  

}
