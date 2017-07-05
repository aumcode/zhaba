using System;
using Zhaba.Data.Rows;

namespace Zhaba.Data.Forms
{
  public abstract class IssueFormBase : ProjectFormBase
  {
    #region .ctor

    protected IssueFormBase()
    {
      
    }

    protected IssueFormBase(ProjectRow project, IssueRow issue) : base(project)
    {
      ____SetIssue(issue);
    }

    #endregion

    #region Fields
    [NonSerialized]
    private IssueRow m_Issue;
    #endregion

    #region Properties
    public IssueRow Issue { get { return m_Issue; } }
    public ulong IssueID { get { return m_Issue.Counter; } }
    #endregion

    #region public
    public void ____SetIssue(IssueRow issue)
    {
      m_Issue = issue;
    }
    #endregion
  }
}