using System;

using NFX.DataAccess;
using NFX.ApplicationModel.Pile;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Rows;
using Zhaba.Data.Forms;

namespace Zhaba.Data
{
  public interface IZhabaDataStore : IDataStore
  {
    ICRUDDataStore CRUD { get; }
    IUniqueSequenceProvider SequenceProvider { get; }
    ICache Cache { get; }

    IUserLogic Users { get; }
    IIssueLogic Issue { get; }
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
    void WriteLogEvent(IssueLogEvent evt);
    Exception WriteIssueForm(IssueForm from, out object saveResult);
    Exception WriteIssueAssignForm(IssueAssignForm from, out object saveResult);
    void CloseIssue(ulong C_Project, ulong C_Issue, ulong C_User);
    void ReOpenIssue(ulong C_Project, ulong C_Issue, ulong C_User);
    void DeferIssue(ulong C_Project, ulong C_Issue, ulong C_User);
    void ChangeProgess(ulong C_User, ulong issueCounter, int value, string description = null);
  }

  

}
