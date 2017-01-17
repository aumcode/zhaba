using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;

namespace Zhaba.Web
{
  /// <summary>
  /// All controls in Zhaba derive from this one
  /// </summary>
  public abstract class ZhabaControl : ZhabaTemplate, ISiteControl
  {
  }
}
