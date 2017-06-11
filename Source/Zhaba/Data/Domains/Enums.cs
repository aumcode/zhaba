using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.RelationalModel.DataTypes;

namespace Zhaba.Data.Domains
{
  public class ZhabaUserRoleType : ZhabaEnum
  {
    public const int MAX_LEN = 1;

    public const string ADMIN = "A";
    public const string USER  = "U";

    public const string VALUE_LIST = "A: Admin, U: User";

    public ZhabaUserRoleType() : base(DBCharType.Char, "A|U") { }

    public static string MapDescription(string value)
    {
      if (value.EqualsOrdIgnoreCase(ADMIN))
        return "Admin";
      if (value.EqualsOrdIgnoreCase(USER))
        return "User";

      return "User";
    }
  }

  public class ZhabaIssueStatusType : ZhabaEnum
  {
    public const int MAX_LEN = 1;

    public const string NEW = "N";
    public const string ASSIGNED = "A";
    public const string DONE = "D";
    public const string CLOSED = "C";

    public const string VALUE_LIST = "N: New, A: Assigned, D: Done, C: Closed";

    public ZhabaIssueStatusType() : base(DBCharType.Char, "N|A|D|C") { }

    public static string MapDescription(string value)
    {
      if (value.EqualsOrdIgnoreCase(NEW))
        return "New";
      if (value.EqualsOrdIgnoreCase(ASSIGNED))
        return "Assigned";
      if (value.EqualsOrdIgnoreCase(DONE))
        return "Done";
      if (value.EqualsOrdIgnoreCase(CLOSED))
        return "Closed";

      return "New";
    }
  }
}
