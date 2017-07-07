using NFX;
using NFX.DataAccess.CRUD;
using System;

namespace Zhaba.Data.Rows
{
  public class IssueChatFilterRow :ZhabaRowWithPK
  {
    [Field] public ulong C_Issue { get; set; }
    [Field] public ulong C_User { get; set; }
    [Field] public DateTime Note_Date { get; set; }
    [Field] public string Note { get; set; }
    [Field] public string First_Name { get; set; }
    [Field] public string Last_Name { get; set; }
    [Field] public string Login { get; set; }
    [Field]
    public string Name
    {
      get
      {
        return "{0} {1}".Args(First_Name, Last_Name);
      }
    }
  }
}
