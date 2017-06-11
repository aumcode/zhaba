using System;
using System.Collections.Generic;

using NFX;

using Zhaba.Data;
using Zhaba.Security;
using Zhaba.Web;
using Zhaba.Data.Store;

namespace Zhaba
{
  /// <summary>
  /// Global Zhaba Application Context
  /// </summary>
  public static class ZApp
  {
    public static IZhabaDataStore Data
    {
      get
      {
        var result = App.DataStore as IZhabaDataStore;
        if (result == null) throw new ZhabaException("Zhaba Data is not injected");
        return result;
      }
    }
  }
}
