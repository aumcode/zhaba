using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.Wave.MVC;

using Zhaba.Security;
using Zhaba.Data.Rows;

namespace Zhaba.Web
{
  public abstract class ZhabaController : Controller
  {
    /// <summary>
    /// Returns true to indicate that the session is present and injected with a valid NFX user
    /// </summary>
    public bool IsValidUser
    {
      get
      {
        var session = WorkContext.Session;
        if (session == null) return false;
        var user = session.User;
        if (user == null) return false;
        return user.IsAuthenticated;
      }
    }

    /// <summary>
    /// Returns current ZhabaWebSession or throws if it is not available
    /// </summary>
    public ZhabaWebSession ZhabaWebSession
    {
      get
      {
        var result = WorkContext.Session as ZhabaWebSession;
        if (result == null)
          throw new ZhabaWebException("Zhaba web session is null");

        return result;
      }
    }

    /// <summary>
    /// Returns ZhabaUser or invalid
    /// </summary>
    public ZhabaUser ZhabaUser
    {
      get { return ZhabaWebSession.ZhabaUser; }
    }

    public static object JSON_OK_ROW_ID(Row row)
    {
      var gr = row as ZhabaRowWithULongPK;
      if (gr==null) return NFX.Wave.SysConsts.JSON_RESULT_OK;

      return new {OK = true, GDID = gr.Counter};
    }

    public TTemplate MakePage<TTemplate>(params object[] ctorArgs) where TTemplate : ZhabaTemplate
    {
      return (TTemplate)Activator.CreateInstance(typeof(TTemplate), ctorArgs);
    }

    public TControl MakeControl<TControl>(params object[] ctorArgs) where TControl : ZhabaControl
    {
      return (TControl)Activator.CreateInstance(typeof(TControl), ctorArgs);
    }
  }
}
