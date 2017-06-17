using System;

using Zhaba.Data.Rows;

namespace Zhaba.Data.Forms
{
  public abstract class ProjectFormBase : ZhabaForm
  {
    protected ProjectFormBase() { }
    protected ProjectFormBase(ProjectRow project)
    {
      ____SetProject(project);
    }

    [NonSerialized]
    private ProjectRow m_ProjectRow;

    public ProjectRow ProjectRow { get { return m_ProjectRow; } }
    public ulong      ProjectID  { get { return m_ProjectRow.Counter; } }

    public void ____SetProject(ProjectRow row)
    {
      m_ProjectRow = row;
    }
  }
}
