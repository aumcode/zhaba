using System;
using System.Security.Cryptography;
using System.Text;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.Security;
using NFX.Serialization.JSON;
using NFX.Wave;
using NFX.Wave.Client;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;
using Zhaba.Data.Rows;
using Zhaba.Security;
using Zhaba.Web.Pages;

namespace Zhaba.Web.Controllers
{
  public class User : ZhabaController
  {
  /*
    [Action("sign-up", 0, "match{methods=GET,POST}")]
    public object SignUp(UserForm form)
    {
      WorkContext.NeedsSession();

      if (IsValidUser)
      {
        if (WorkContext.RequestedJSON)
          throw HTTPStatusException.NotAcceptable_406("Log out first");
        else
          return new Redirect(URIS.DASHBOARD);
      }

      Exception error = null;
      if (WorkContext.IsPOST)
      {
        UserRow newUserRow;
        error = form.Save(out newUserRow);
        if (error == null)
        {
          ZhabaWebSession.User = App.SecurityManager.Authenticate(new IDPasswordCredentials(form.Login, form.Password));
          ZhabaWebSession.HasJustLoggedIn(NFX.ApplicationModel.SessionLoginType.Human);
          if (WorkContext.RequestedJSON)
            return new ClientRecord(form, null);
          else
            return new Redirect(URIS.DASHBOARD);
        }
      }
      else //GET
      {
        form = new UserForm();
      }

      if (WorkContext.RequestedJSON)
        return new ClientRecord(form, error);
      else
        return MakePage<UserRegistrationPage>(form, error);
    }

    */
    [Action]
    public object LogIn(LoginForm form)
    {
      WorkContext.NeedsSession();

      if (IsValidUser)
      {
        if (WorkContext.RequestedJSON)
          throw HTTPStatusException.NotAcceptable_406("Already logged in");
        else
          return new Redirect(URIS.DASHBOARD);
      }

      Exception error = null;
      if (WorkContext.IsPOST)
      {
        ZhabaWebSession.User = App.SecurityManager.Authenticate(new IDPasswordCredentials(form.ID, form.Password));
        if (IsValidUser)
        {
          ZhabaWebSession.HasJustLoggedIn(NFX.ApplicationModel.SessionLoginType.Human);
          if (WorkContext.RequestedJSON)
            return NFX.Wave.SysConsts.JSON_RESULT_OK;
          else
            return new Redirect(URIS.DASHBOARD);
        }
        else
          error = new ZhabaException("Invalid user");
      }
      else //GET
      {
        form = new LoginForm();
        form.CSRFToken = ZhabaWebSession.CSRFToken;
      }

      if (WorkContext.RequestedJSON)
        return NFX.Wave.SysConsts.JSON_RESULT_OK;
      else
        return MakePage<Login>(form, error);
    }

    [Action, NoCache]
    public object LogOut()
    {
      WorkContext.NeedsSession(onlyExisting: true);

      if (IsValidUser)
        ZhabaWebSession.End();

      if (WorkContext.RequestedJSON)
        return NFX.Wave.SysConsts.JSON_RESULT_OK;
      else
        return new Redirect(URIS.HOME);
    }
  }
}
