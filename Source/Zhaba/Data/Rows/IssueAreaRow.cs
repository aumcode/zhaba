using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;

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
  }
}
