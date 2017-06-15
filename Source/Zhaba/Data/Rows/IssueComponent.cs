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
  public class IssueComponent : TypedRow
  {
    public IssueComponent() : base() {}


    [Field(required: true, nonUI: true, storeFlag: StoreFlag.OnlyLoad)]
    public ulong C_Project { get; set; }

    [Field(required: true, key: true, description: "Issue" )]
    public ulong C_Issue { get; set; }
    
    [Field(required: true, key: true, description: "Component")]
    public ulong C_Component { get; set; }


    public override Exception Validate(string targetName)
    {
      var error = base.Validate(targetName);
      if (error != null) return error;

      var iQry = QProject.IssueByID<IssueRow>(C_Project, C_Issue);
      var issue = ZApp.Data.CRUD.LoadRow(iQry);
      if (issue == null)
        return new CRUDFieldValidationException(this.Schema.Name, "C_Issue", "Non existing issue");
      
      var cQry = QProject.ComponentByID<AreaRow>(C_Project, C_Component);
      var component = ZApp.Data.CRUD.LoadRow(cQry);
      if (component == null)
        return new CRUDFieldValidationException(this.Schema.Name, "C_Component", "Non existing component");

      return null;
    }
  }
}
