﻿using System;
using System.Collections.Generic;

using NFX;
using NFX.ApplicationModel;
using NFX.DataAccess.CRUD;
using NFX.Log;

using Zhaba.Web;

namespace Zhaba.Data.Forms
{
  public abstract class ZhabaForm : FormModel
  {
    public const string ITEM_ID_BAG_PARAM = "ITEM_ID";

    /// <summary>
    /// A typed shortcut to ExecutionContext.Session
    /// </summary>
    public ZhabaWebSession ZhabaSession
    {
      get
      {
        var result = ExecutionContext.Session as ZhabaWebSession;
        if (result == null)
          throw new ZhabaWebException("Zhaba session unavailable");

        return result;
      }
    }

    public Security.ZhabaUser ZhabaUser { get { return ZhabaSession.ZhabaUser; } }

    public void Delete(out Exception error)
    {
        error = DoDelete();
    }

        protected void Log(MessageType tp, string from, string text, Exception error = null, Guid? related = null)
    {
      App.Log.Write(new Message
      {
        Type = tp,
        Topic = Consts.ZHABA_TOPIC,
        From = "{0}.{1}".Args(GetType().Name, from),
        Text = text,
        Exception = error,
        RelatedTo = related ?? Guid.Empty
      });
    }

    protected override Exception DoSave(out object saveResult)
    {
      throw new NotImplementedException();
    }

    protected virtual Exception DoDelete()
    {
      throw new NotImplementedException();
    }
  }

  public abstract class ZhabaFormWithCSRFCheck : ZhabaForm
  {
    protected ZhabaFormWithCSRFCheck()
    {
      var session = ExecutionContext.Session as IZhabaSession;
      if (session != null) CSRFToken = session.CSRFToken;
    }

    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;
      if (!checkCSRF())
        return new CRUDFieldValidationException(GetType().FullName, "CSRFToken", "CSRF check failure");

      return null;
    }

    private bool checkCSRF()
    {
      var session = ExecutionContext.Session as IZhabaSession;
      if (session==null) return true; // If session does not exist then pass. This does not violate security
                                      // because SCRF is not security but additional check for cross-site forgery which will always apply once security passes
                                      // however, forms are purposed not only for web, therefore if session is not there, then pass CSRF

      return session.LastLoginType == SessionLoginType.Robot ||
             (CSRFToken != null && CSRFToken.EqualsOrdSenseCase(session.CSRFToken));
    }
  }

  public abstract class ZhabaFilterForm : ZhabaForm
  {
    protected ZhabaFilterForm() : base()
    {
    }
  }

  public abstract class ZhabaReportForm : ZhabaForm
  {
    protected ZhabaReportForm() : base()
    {
      
    }
  }
}
