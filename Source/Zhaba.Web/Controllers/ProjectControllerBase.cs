using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.Wave;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;
using Zhaba.Data.Filters;

namespace Zhaba.Web.Controllers
{
  public abstract class ProjectControllerBase : ZhabaDataSetupController
  {
    [ThreadStatic]
    private static ProjectRow ts_ProjectRow;

    /// <summary>
    /// Accesses thread-static fresh Project row record re-read from DB for THIS request
    /// </summary>
    protected ProjectRow ProjectRow { get { return ts_ProjectRow; } }

    protected override bool BeforeActionInvocation(WorkContext work, string action, MethodInfo method, object[] args, ref object result)
    {
      ts_ProjectRow = null;

      //1. Must supply progect ID
      var projID = work.MatchedVars[URIS.PROJECT_ID_PARAM].AsNullableULong();
      if (!projID.HasValue)
        throw HTTPStatusException.BadRequest_400("Missing " + URIS.PROJECT_ID_PARAM);

      //2. Check permissions
      //if (!pass) throw HTTPStatusException.Forbidden_403();

      //3. Fetch Project row data
      var qry = QCommon.ProjectByID<ProjectRow>(projID.Value);
      var project = ZApp.Data.CRUD.LoadRow(qry);
      if (project == null)
        throw HTTPStatusException.NotFound_404("Project missing/deleted");

      //4. Inject thread context
      ts_ProjectRow = project;

      //5. Bind regular params
      var handled = base.BeforeActionInvocation(work, action, method, args, ref result);

      //6. Inject Project Row in forms and filters
      foreach (var arg in args)
      {
        var pForm = arg as ProjectFormBase;
        if (pForm != null)
        {
          pForm.____SetProject(project);
          continue;
        }

        var pFilter = arg as ProjectFilterBase;
        if (pFilter != null)
        {
          pFilter.____SetProject(project);
          continue;
        }
      }

      return handled;
    }


    protected object DataSetup_ProjectItemDetails<TForm, TPage>(ulong? id, TForm form, string postRedirect)
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
      {
        form = (TForm)Activator.CreateInstance(typeof(TForm), new object[] { ProjectRow, id });
      }

      if (WorkContext.RequestedJSON)
        return new ClientRecord(form, error);
      else
        return MakePage<TPage>(form, error);
    }
  }
}
