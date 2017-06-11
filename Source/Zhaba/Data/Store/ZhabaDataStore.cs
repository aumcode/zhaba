using System;
using System.Collections.Generic;

using NFX;
using NFX.ApplicationModel.Pile;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.MySQL;
using NFX.Environment;
using NFX.Wave.Client;
using NFX.ServiceModel;
using NFX.Serialization.JSON;

using Zhaba.Data.Rows;

namespace Zhaba.Data.Store
{
  public sealed partial class ZhabaDataStore : ServiceWithInstrumentationBase<object>, IZhabaDataStore, IDataStoreImplementation
  {
    public const string CONFIG_MYSQL_SECTION = "my-sql";

    #region Nested

      private abstract class LogicBase : DisposableObject, IStoreLogic
      {
        public LogicBase(ZhabaDataStore store)
        {
          m_DataStore = store;
        }

        private readonly ZhabaDataStore m_DataStore;

        public ZhabaDataStore Store       { get { return m_DataStore; } }
        IZhabaDataStore IStoreLogic.DStore { get { return m_DataStore; } }
      }

    #endregion

    #region .ctor

      public ZhabaDataStore() : this(null) { }
      public ZhabaDataStore(object director) : base(director)
      {
        m_DataStore = new MySQLDataStore();
        m_UniqueSequenceProvider = new ZhabaSequenceProvider();
        m_UserLogic = new UserLogic(this);
      }

      protected override void Destructor()
      {
        DisposeAndNull(ref m_DataStore);
        base.Destructor();
      }

    #endregion

    private ICRUDDataStoreImplementation m_DataStore;
    private ZhabaSequenceProvider m_UniqueSequenceProvider;
    private UserLogic m_UserLogic;

    #region Properties

    public ICache Cache { get{ throw new NotImplementedException(); }}
    public ICRUDDataStore CRUD { get { return m_DataStore; }}
    public IUniqueSequenceProvider SequenceProvider { get { return m_UniqueSequenceProvider; }}
    public override bool InstrumentationEnabled
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }
    public string TargetName { get { return m_DataStore.TargetName; }}
    public StoreLogLevel LogLevel
    {
      get { return m_DataStore.LogLevel; }
      set { m_DataStore.LogLevel = value; }
    }

    public IUserLogic Users { get { return m_UserLogic; }}

    #endregion

    #region Public

    //public IEnumerable<T> RecordSet<T>(string queryName, string id = "") where T : TypedRow
    //{
    //  Query<T> q;
    //  if (id == "") q = new Query<T>("{0}".Args(queryName));
    //  else q = new Query<T>("{0}".Args(queryName))
    //     {
    //        new Query.Param("ID", id)
    //     };
    //  return CRUD.LoadOneRowset(q).AsEnumerableOf<T>();
    //}

    //public RowsetBase RecordSet(string queryName, object filter)
    //{
    //  Query q;
    //  if (filter == null) q = new Query<DynamicRow>("{0}".Args(queryName));
    //  else q = new Query<DynamicRow>("{0}".Args(queryName))
    //    {
    //    new Query.Param("filter", filter)
    //    };
    //  return CRUD.LoadOneRowset(q);
    //}

    //public RowsetBase RecordSet(string queryName, string id, object filter)
    //{
    //  Query q;
    //  if (filter == null) q = new Query<DynamicRow>("{0}".Args(queryName))
    //    {
    //    new Query.Param("filter", filter)
    //    };
    //  else q = new Query<DynamicRow>("{0}".Args(queryName))
    //    {
    //    new Query.Param("filter", filter),
    //    new Query.Param("id", id)
    //    };
    //  return CRUD.LoadOneRowset(q);
    //}

    //public T Record<T>(string queryName, string id, bool createNew = false) where T : TypedRow, new()
    //{
    //  if ((id == "" || id == "0" || id == null) && createNew) return new T();
    //  var q = new Query<T>("{0}".Args(queryName))
    //     {
    //        new Query.Param("ID", id)
    //     };
    //  return CRUD.LoadRow<T>(q);
    //}

    //public string JSONRecordSet<T>(string queryName, string id = "") where T : TypedRow
    //{
    //  var records = RecordSet<T>(queryName, id);
    //  if (records != null)
    //  {
    //    var gen = new RecordModelGenerator();
    //    Exception validationError = null;

    //    var result = new JSONDataMap();
    //    result.Add("data", records);

    //    foreach (T row in records)
    //    {
    //      result.AddRange(gen.RowToRecordInitJSON(row, validationError));
    //      break;
    //    }
    //    return result.ToString();
    //  }
    //  return null;
    //}

    //public string JSONFilter<T>() where T : TypedRow, new()
    //{
    //  var gen = new RecordModelGenerator();
    //  return gen.RowToRecordInitJSON(new T(), null).ToString();
    //}


    //public JSONDataMap Lookup(string queryName, string id = "")
    //{
    //  Query q;
    //  if (id == "") q = new Query("{0}".Args(queryName));
    //  else q = new Query("{0}".Args(queryName))
    //     {
    //        new Query.Param("ID", id)
    //     };
    //  var records = CRUD.LoadOneRowset(q);
    //  if (records != null)
    //  {
    //    var result = new JSONDataMap();
    //    foreach (var row in records)
    //      result.Add(row[0].ToString(), row[1].ToString());
    //    return result;
    //  }
    //  return null;
    //}

    public void TestConnection()
    {
      m_DataStore.TestConnection();
    }

    //public object SaveRow(TypedRow row)
    //{
    //  var tx = CRUD.BeginTransaction();
    //  ulong counter = row["Counter"].AsULong();
    //  if (counter == 0)
    //  {
    //    var sequenceName = row.GetType().Name.Replace("Row", "");
    //    counter = SequenceProvider.GenerateOneSequenceID("Zhaba", sequenceName);
    //    row["Counter"] = counter;
    //  }
    //  tx.Upsert(row);
    //  tx.Commit();
    //  return counter;
    //}

    //public object SaveIssueRow(IssueRow row, string comment) {
    //  var tx = CRUD.BeginTransaction();
    //  ulong? counter = row.Counter;
    //  if (counter == null)
    //  {
    //    counter = SequenceProvider.GenerateOneSequenceID("Zhaba", "Issue");
    //    row.Counter = counter;
    //  }
    //  var logCounter = SequenceProvider.GenerateOneSequenceID("Zhaba", "IssueLog");
    //  IssueLogRow logRow = new IssueLogRow()
    //  {
    //    Counter = logCounter,
    //    C_Issue = row.Counter,
    //    Description = comment,
    //    Status = row.Status,
    //    //Creator = ZApp.User.Name,
    //    Creation_Date = DateTime.Now
    //  };

    //  tx.Upsert(row);
    //  tx.Insert(logRow);
    //  tx.Commit();
    //  return counter;
    //}

    #endregion

    protected override void DoConfigure(IConfigSectionNode node)
    {
      if (node != null)
      {
        (node as ConfigSectionNode).ProcessIncludePragmas(true);
        m_DataStore.Configure(node[CONFIG_MYSQL_SECTION]);
      }
      base.DoConfigure(node);
    }

    protected override void DoStart()
    {
      base.DoStart();

    }

    protected override void DoWaitForCompleteStop()
    {
      base.DoWaitForCompleteStop();
    }
  }
}
