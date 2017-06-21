using System;
using System.Security.Cryptography;
using System.Text;

using NFX;
using NFX.Wave;
using NFX.Wave.MVC;

using Zhaba.Data.Forms;

namespace Zhaba.Web.Controllers
{
  public class Home : ZhabaController
  {
    [Action]
    public object Index(UserForm form)
    {
      return new Redirect(URIS.USER_LOGIN);
    }
  }
}
