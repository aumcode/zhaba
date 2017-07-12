using NFX;
using NFX.DataAccess.CRUD;
using System;

namespace Zhaba.Data.Rows
{
  public class IssueChatFilterRow :IssueChatRow
  {

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

    [Field]
    public bool HasEdit { get; set; } = false;


    public static bool CheckEdit(IssueChatRow item)
    {
      int hours = (App.TimeSource.UTCNow - item.Note_Date).Hours;
      int day = (App.TimeSource.UTCNow - item.Note_Date).Days;
      return (day == 0 && hours < 24) ;
    }
  }
}
