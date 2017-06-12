using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.ApplicationModel.Pile;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Environment;
using NFX.ServiceModel;

using Zhaba.Data;

namespace Zhaba.DataLogic
{
  public class ZhabaDataStore : ServiceWithInstrumentationBase<object>, IZhabaDataStore, IDataStoreImplementation
  {
    public const string CONFIG_ZHABA_DS = "zhaba-data-store";

    #region .ctor

    public ZhabaDataStore() : this(null) {}
    public ZhabaDataStore(object director) : base(director)
    {
      m_Users = new ZhabaUserLogic(this);
      m_SequenceProvider = new ZhabaSequenceProvider();
    }

    protected override void Destructor()
    {
      DisposableObject.DisposeAndNull(ref m_Users);
      base.Destructor();
    }

    #endregion

    #region Fields

    private ICRUDDataStoreImplementation m_CRUD;
    private ZhabaUserLogic m_Users;
    private ZhabaSequenceProvider m_SequenceProvider;

    #endregion

    public ICache Cache { get { throw new NotImplementedException(); } }
    public ICRUDDataStore CRUD { get { return m_CRUD; } }
    public IUniqueSequenceProvider SequenceProvider { get { return m_SequenceProvider; } }
    public IUserLogic Users { get { return m_Users; } }

    public override bool InstrumentationEnabled
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    public StoreLogLevel LogLevel
    {
      get { return m_CRUD.LogLevel; }
      set { m_CRUD.LogLevel = value; }
    }

    public string TargetName { get { return m_CRUD.TargetName; } }

    public void TestConnection()
    {
      m_CRUD.TestConnection();
    }

    protected override void DoConfigure(IConfigSectionNode node)
    {
      var config = node[CONFIG_ZHABA_DS];
      if (config.Exists)
        m_CRUD = FactoryUtils.MakeAndConfigure<ICRUDDataStoreImplementation>(config);
      else m_CRUD = null;
      base.DoConfigure(node);
    }
  }
}
