using NFX.DataAccess.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFX;
using NFX.Wave;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Rows;

namespace Zhaba.Data.Forms
{
  public class NoteEditForm : ProjectFormBase
  {
    public NoteEditForm() { }

    public NoteEditForm(ProjectRow project,  IssueRow issue, string status) 
      : base(project)
    {
      m_Issue = issue;
      FormMode = FormMode.Insert;
      Status = status;
    }
    
    
    [NonSerialized]
    private IssueRow m_Issue;
    
    [Field(required: false,
      description: "Status",
      metadata: @"Placeholder='Status'")]
    public string Status { get; set; }
    [Field(required: false,
           description: "Description",
           metadata: @"Placeholder='Description'")]
    public string Description { get; set; }
    
    public IssueRow Issue { get { return m_Issue; } }
    public ulong IssueID { get { return m_Issue.Counter; } }
  }
}
