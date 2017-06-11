using System;
using System.Collections.Generic;

using NFX;
using NFX.ApplicationModel.Pile;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
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

    public void TestConnection()
    {
      m_DataStore.TestConnection();
    }

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
