using System;
using System.Collections.Generic;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;
using NFX.Serialization.JSON;
using NFX.Wave;

using Zhaba.Data.Rows;
using Zhaba.Data.QueryBuilders;
using Zhaba.Data.Domains;

namespace Zhaba.Data.Forms
{
  public class IssueForm : ProjectFormBase
  {
    public IssueForm() { }
    public IssueForm(ProjectRow project, ulong? id) : base(project)
    {
    }

    protected override Exception DoSave(out object saveResult)
    {
      throw new NotImplementedException();
    }
  }
}

