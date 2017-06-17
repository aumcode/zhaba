using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;

using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Rows
{
  public class AgendaIssue : TypedRow
  {
    public AgendaIssue() : base() {}

    [Field(required: true, nonUI: true, storeFlag: StoreFlag.OnlyLoad)]
    public ulong C_Project { get; set; }
    
    [Field(required: true, key: true, description: "Agenda")]
    public ulong C_Agenda { get; set; }

    [Field(required: true, key: true, description: "Issue" )]
    public ulong C_Issue { get; set; }

    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var iQry = QProject.IssueByID<IssueRow>(C_Project, C_Issue);
      var issue = ZApp.Data.CRUD.LoadRow(iQry);
      if (issue == null)
        return new CRUDFieldValidationException(this.Schema.Name, "C_Issue", "Non existing issue");
      
      // TODO
      var aQry = QProject.AreaByID<AreaRow>(C_Project, C_Agenda);
      var area = ZApp.Data.CRUD.LoadRow(aQry);
      if (area == null)
        return new CRUDFieldValidationException(this.Schema.Name, "C_Area", "Non existing area");

      return null;
    }
  }
}
