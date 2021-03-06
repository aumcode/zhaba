﻿using NFX;

using Zhaba.Data;
using Zhaba.Security;
using Zhaba.Web;

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
