using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.DataAccess.CRUD;

namespace Zhaba.Data
{
  public class UserInfo : TypedRow
  {
    [Field] public ulong  Counter    { get; set; }
    [Field] public string Login      { get; set; }
    [Field] public string First_Name { get; set; }
    [Field] public string Last_Name  { get; set; }
  }
}
