using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zhaba.Data.Forms;
using Zhaba.Data.Rows;

namespace Zhaba.Data.Filters
{
  public abstract class ProjectFilterBase : ZhabaFilterForm
  {
    [NonSerialized]
    private ProjectRow m_ProjectRow;

    public ProjectRow ProjectRow { get{ return m_ProjectRow;} }
    public ulong      ProjectID  { get{ return m_ProjectRow.Counter;} }

    public void ____SetProject(ProjectRow row)
    {
      m_ProjectRow = row;
    }
  }
}
