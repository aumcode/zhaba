using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.Wave;
using NFX.Wave.Templatization;

namespace Zhaba.Web
{
  /// <summary>
  /// All pages in Zhaba derive from this one
  /// </summary>
  public abstract class ZhabaPage : ZhabaTemplate
  {
    public string Title { get; set; }
  }
}
