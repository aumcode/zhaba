using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX.ApplicationModel;

namespace Zhaba.Web
{
  public interface IZhabaSession: ISession
  {
    Security.ZhabaUser ZhabaUser { get; }

    string CSRFToken { get; }
  }
}
