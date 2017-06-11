using System;

using NFX.DataAccess;
using NFX.ApplicationModel.Pile;
using NFX.DataAccess.CRUD;

using Zhaba.Data.Rows;

namespace Zhaba.Data.Store
{
  public interface IZhabaDataStore : IDataStore
  {
    ICRUDDataStore CRUD { get; }
    IUniqueSequenceProvider SequenceProvider { get; }
    ICache Cache { get; }

    IUserLogic Users { get; }

    ///// <summary>
    ///// Returns Typed Rows by given query name
    ///// </summary>
    //IEnumerable<T> RecordSet<T>(string queryName, string id = "") where T : TypedRow;

    ///// <summary>
    ///// Returns Dynamic Rows by given query name
    ///// </summary>
    //RowsetBase RecordSet(string queryName, object filter);
    //RowsetBase RecordSet(string queryName, string id, object filter);

    ///// <summary>
    ///// Returns one row by given query name and id
    ///// </summary>
    //T Record<T>(string queryName, string id, bool createNew = false) where T : TypedRow, new();

    ///// <summary>
    ///// Returns Rows as JSON string by given query name
    ///// </summary>
    ////string JSONRecordSet<T>(string queryName, string id = "") where T : TypedRow;

    //string JSONFilter<T>() where T : TypedRow, new();

    ///// <summary>
    ///// Returns Rows as JSON Map {field1 : field2} by given query name
    ///// </summary>
    //JSONDataMap Lookup(string queryName, string id = "");

    ///// <summary>
    ///// Stores Row into database
    ///// </summary>
    //object SaveRow(TypedRow row);

    ///// <summary>
    ///// Stores Issue Row into database
    ///// </summary>
    //object SaveIssueRow(IssueRow row, string comment);
  }

  public interface IStoreLogic
  {
    IZhabaDataStore DStore { get; }
  }

  public interface IUserLogic : IStoreLogic
  {
    /// <summary>
    /// Returns User by given login
    /// </summary>
    UserRow GetUser(string login);
  }
}
