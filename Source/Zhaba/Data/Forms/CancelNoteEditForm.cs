using NFX.DataAccess.CRUD;
using Zhaba.Data.Rows;

namespace Zhaba.Data.Forms
{
  public class CancelNoteEditForm : IssueFormBase
  {
    public CancelNoteEditForm() { }

    public CancelNoteEditForm(ProjectRow project, IssueRow issue, string status) 
      : base(project, issue)
    {
      FormMode = FormMode.Insert;
      Status = status;
    }

    [Field(required: false,
          description: "Status",
          metadata: @"Placeholder='Status'")]
    public string Status { get; set; }

    [Field(required: false,
           description: "Description",
           metadata: @"Placeholder='Description'",
           minLength: 8)]
    public string Description { get; set; }
  }
}
