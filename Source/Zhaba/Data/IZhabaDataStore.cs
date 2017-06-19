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
}
