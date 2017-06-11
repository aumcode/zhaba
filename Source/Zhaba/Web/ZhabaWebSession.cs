using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX.Security;
using NFX.Wave;

using Zhaba.Data.Rows;
using Zhaba.Security;

namespace Zhaba.Web
{
  [Serializable]
  public sealed class ZhabaWebSession : WaveSession, IZhabaSession
  {
    #region .ctor

      public ZhabaWebSession() : base() { }
      public ZhabaWebSession(Guid id, User user) : base(id)
      {
        User = user;
      }

    #endregion

    #region Properties

      public ZhabaUser ZhabaUser
      {
        get { return User as ZhabaUser ?? ZhabaUser.Invalid; }
      }

      public ProjectRow SelectedProject { get; set; }

    #endregion
  }
}
