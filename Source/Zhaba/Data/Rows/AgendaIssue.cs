using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.DataAccess;
using NFX.DataAccess.CRUD;

namespace Zhaba.Data.Rows
{
  [Table(name: "tbl_agendaissue")]
  public class AgendaIssue : TypedRow
  {
    public AgendaIssue() : base() {}

    [Field(required: true, nonUI: true, storeFlag: StoreFlag.OnlyLoad)]
    public ulong C_Project { get; set; }
    
    [Field(required: true, key: true, description: "Agenda")]
    public ulong C_Agenda { get; set; }

    [Field(required: true, key: true, description: "Issue" )]
    public ulong C_Issue { get; set; }
  }
}
