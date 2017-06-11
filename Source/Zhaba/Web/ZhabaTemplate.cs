using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.ApplicationModel;
using NFX.Serialization.JSON;
using NFX.Wave;
using NFX.Wave.Client;
using NFX.Wave.Templatization;

using Zhaba.Security;
using Zhaba.Data.Forms;

namespace Zhaba.Web
{
  /// <summary>
  /// All templates in Zhaba derive from this one
  /// </summary>
  public abstract class ZhabaTemplate : WaveTemplate
  {
    public const string URI_STATIC_ROOT_PATH = "/static/";


    /// <summary>
    /// Returns local template work context or current request one or throws
    /// </summary>
    public WorkContext WorkContext
    {
      get
      {
        var result = this.Context;
        if (result != null) return result;

        result = ExecutionContext.Request as WorkContext;
        if (result == null)
          throw new ZhabaWebException("WorkContext is not injected");

        return result;
      }
    }


    /// <summary>
    /// Returns ZhabaWebSession or throws if not injected
    /// </summary>
    public ZhabaWebSession ZhabaSession
    {
      get
      {
        var result = Session as ZhabaWebSession;
        if (result == null)
          throw new ZhabaWebException("Zhaba web session is null");

        return result;
      }
    }


    /// <summary>
    /// Tries to get an ZhabaUser from existing session, if not available then returns ZhabaUser.Invalid
    /// </summary>
    public ZhabaUser ZhabaUser
    {
      get
      {
        ZhabaUser result;
        if (Context != null)
        {
          Context.NeedsSession(onlyExisting: true); //ONLY EXISTING!!!
          result = (Context.Session != null) ? (Session.User as ZhabaUser) : null;
        }
        else
        {
          var session = ExecutionContext.Session as ZhabaWebSession;
          result = (session != null) ? (ZhabaUser)session.User : null;
        }

        return result ?? ZhabaUser.Invalid;
      }
    }

    /// <summary>
    /// Returns metadata for WAVE.RecordModel.Record object for a form encoded as JSON string
    /// </summary>
    public string FormJSON(ZhabaForm form,
                           Exception validationError = null,
                           ModelFieldValueListLookupFunc lookupFunc = null,
                           string recID = null,
                           string target = null)
    {
      var generator = new RecordModelGenerator();
      return generator.RowToRecordInitJSON(form, validationError, recID, target, CoreConsts.ISO_COUNTRY_USA, lookupFunc).ToJSON();
    }

    public string StockContentPath(string path)
    {
      return URIUtils.JoinPathSegs(URI_STATIC_ROOT_PATH, path);
    }
  }
}
