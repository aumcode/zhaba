using NFX.DataAccess;
using NFX.DataAccess.CRUD;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_issuecomponent")]
  public class IssueComponentRow : TypedRow
  {
    public IssueComponentRow() : base() {}

    [Field(required: true, nonUI: true, storeFlag: StoreFlag.OnlyLoad)]
    public ulong C_Project { get; set; }

    [Field(required: true, key: true, description: "Issue" )]
    public ulong C_Issue { get; set; }
    
    [Field(required: true, key: true, description: "Component")]
    public ulong C_Component { get; set; }
  }
}
