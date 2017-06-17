using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;

using Zhaba.Data.QueryBuilders;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issuearea")]
  public class IssueAreaRow : TypedRow
  {
    public IssueAreaRow() : base() {}

    [Field(required: true, nonUI: true, storeFlag: StoreFlag.OnlyLoad)]
    public ulong C_Project { get; set; }

    [Field(required: true, key: true, description: "Issue" )]
    public ulong C_Issue { get; set; }
    
    [Field(required: true, key: true, description: "Area")]
    public ulong C_Area { get; set; }

    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var iQry = QProject.IssueByID<IssueRow>(C_Project, C_Issue);
      var issue = ZApp.Data.CRUD.LoadRow(iQry);
      if (issue == null)
        return new CRUDFieldValidationException(this.Schema.Name, "C_Issue", "Non existing issue");
      
      var aQry = QProject.AreaByID<AreaRow>(C_Project, C_Area);
      var area = ZApp.Data.CRUD.LoadRow(aQry);
      if (area == null)
        return new CRUDFieldValidationException(this.Schema.Name, "C_Area", "Non existing area");

      return null;
    }
  }
}
