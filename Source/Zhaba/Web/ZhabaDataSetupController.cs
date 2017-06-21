using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;

namespace Zhaba.Web
{
  /// <summary>
  /// Provides base for controllers that participate in data setup - have filters/grids and record details
  /// </summary>
  public abstract class ZhabaDataSetupController : ZhabaController
  {
    protected virtual object DataSetup_Index<TFilter, TGrid, TPage>(TFilter filter, [CallerMemberName]string callerMethod = null)
      where TFilter : ZhabaFilterForm
      where TGrid   : ZhabaControl, IGridControl
      where TPage   : ZhabaPage
    {
      if (callerMethod.IsNullOrWhiteSpace())
        throw new ZhabaWebException("DataSetup_Index(callerMethod==null|empty)");

      var FILTER_SESSION_KEY = "filterKey:" + GetType().FullName + "." + callerMethod;

      if (WorkContext.IsPOST)
      {
        object data;
        filter.Save(out data);

        if (WorkContext.HasAnyVarsMatchingFieldNames(filter))
          ZhabaWebSession[FILTER_SESSION_KEY] = filter; //remember new filter in session
        else
          ZhabaWebSession.Items.Remove(FILTER_SESSION_KEY);

        if (WorkContext.RequestedJSON)
          return new JSONResult(data, new JSONWritingOptions { RowsAsMap = false, RowsetMetadata = true });
        else
          return MakeControl<TGrid>(data, "tbl" + callerMethod);
      }
      else
      {
        if (!WorkContext.HasAnyVarsMatchingFieldNames(filter)) //if new filter came blank, then try to take it from session
        {
          var priorFilter = ZhabaWebSession[FILTER_SESSION_KEY] as TFilter;
          if (priorFilter != null) priorFilter.CopyFields(filter);
        }

        if (WorkContext.RequestedJSON)
          return new ClientRecord(filter, null);
        else
          return MakePage<TPage>(filter);
      }
    }

    protected virtual object DataSetup_ItemDetails<TForm, TPage>(object[] args, TForm form, string postRedirect)
      where TForm : ZhabaForm
      where TPage : ZhabaPage
    {
      Exception error = null;

      if (WorkContext.IsPOST)
      {
        Row row;
        error = form.Save(out row);
        if (error == null)
        {
          if (WorkContext.RequestedJSON)
            return JSON_OK_ROW_ID(row);
          else
            return new Redirect(postRedirect);
        }
      }
      else
        form = (TForm)Activator.CreateInstance(typeof(TForm), args);

      if (WorkContext.RequestedJSON)
        return new ClientRecord(form, error);
      else
        return MakePage<TPage>(form, error);
    }
  }
}
