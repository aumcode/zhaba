using System;

using NFX.DataAccess;
using NFX.ApplicationModel.Pile;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Rows;

namespace Zhaba.Data
{
  public interface IZhabaDataStore : IDataStore
  {
    ICRUDDataStore CRUD { get; }
    IUniqueSequenceProvider SequenceProvider { get; }
    ICache Cache { get; }

    IUserLogic Users { get; }
    IIssueLogLogic IssueLog { get; }
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
  public interface IIssueLogLogic : IStoreLogic
  {
    /// <summary>
    /// Work with Issue Log
    /// </summary>
    void AddCreateIssueEvent(UserRow user, IssueRow issue, MilestoneRow milestone);
    void AssigneeToIssue(UserRow oper, DateTime date, IssueRow issue, UserRow assignee, MilestoneRow milestone);
    void UnAssigneeToIssue(UserRow oper, DateTime date, IssueRow issue, UserRow assignee, MilestoneRow milestone);
    void SetCompletness(UserRow user, int percent, string note);
  }

}
