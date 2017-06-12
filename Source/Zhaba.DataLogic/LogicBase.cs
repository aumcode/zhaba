using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;

using Zhaba.Data;

namespace Zhaba.DataLogic
{
  internal abstract class LogicBase : DisposableObject, IStoreLogic
  {
    public LogicBase(ZhabaDataStore store)
    {
      m_Store = store;
    }

    private readonly ZhabaDataStore m_Store;

    public ZhabaDataStore Store
    {
      get { return m_Store; }
    }

    IZhabaDataStore IStoreLogic.Store
    {
      get { return m_Store; }
    }
  }
}
