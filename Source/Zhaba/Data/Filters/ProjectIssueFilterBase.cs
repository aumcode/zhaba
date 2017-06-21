using System;

using Zhaba.Data.Rows;

namespace Zhaba.Data.Filters
{
  public abstract class ProjectIssueFilterBase : ProjectFilterBase
  {
    [NonSerialized]
    private IssueRow m_IssueRow;

    public IssueRow IssueRow { get{ return m_IssueRow;} }
    public ulong    IssueCounter  { get{ return m_IssueRow.Counter;} }

    public void ____SetIssue(IssueRow row)
    {
      m_IssueRow = row;
    }
  }
}
