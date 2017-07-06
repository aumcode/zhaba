using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.RelationalModel.DataTypes;

namespace Zhaba.Data.Domains
{
  public class ZhabaBool : ZhabaEnum
  {
    public const int MAX_LEN = 1;

    public ZhabaBool() : base(DBCharType.Char, "T|F") { }
  }

  public class ZhabaUserStatus : ZhabaEnum
  {
    public const int MAX_LEN = 1;

    public const string USER  = "U";
    public const string ADMIN = "A";
    public const string SYSTEM = "S";

    public const string VALUE_LIST = "U: User, A: Admin, S: System";

    public ZhabaUserStatus() : base(DBCharType.Char, "U|A|S") { }

    public static string MapDescription(string value)
    {
      if (value.EqualsOrdIgnoreCase(USER))
        return "User";
      if (value.EqualsOrdIgnoreCase(ADMIN))
        return "Admin";
      if (value.EqualsOrdIgnoreCase(SYSTEM))
        return "System";

      return "Invalid";
    }

    public static NFX.Security.UserStatus MapStatus(string value)
    {
      if (value.EqualsOrdIgnoreCase(USER))
        return NFX.Security.UserStatus.User;
      if (value.EqualsOrdIgnoreCase(ADMIN))
        return NFX.Security.UserStatus.Admin;
      if (value.EqualsOrdIgnoreCase(SYSTEM))
        return NFX.Security.UserStatus.System;

      return NFX.Security.UserStatus.Invalid;
    }
  }

  public class ZhabaIssueStatus : ZhabaEnum
  {
    public const int MAX_LEN = 3;

    public const string NEW      = "N"; // N-> A,F,X
    public const string REOPEN   = "R"; // R-> A,F,X
    public const string ASSIGNED = "A"; // A-> D,F,X
    public const string DONE     = "D"; // D-> A,C,X
    public const string DEFER    = "F"; // F-> N,A,X
    public const string CANCELED = "X"; // X-> null
    public const string CLOSED   = "C"; // C-> R

    public const string VALUE_LIST = "N: New, R: Reopen, A: Assigned, D: Done, F: Defer, C: Closed, X: Canceled";

    public ZhabaIssueStatus() : base(DBCharType.Char, "N|A|D|C|F|X|R") { }

    public static string MapDescription(string value)
    {
      if (value.EqualsOrdIgnoreCase(NEW))
        return "New";
      if (value.EqualsOrdIgnoreCase(REOPEN))
        return "Reopen";
      if (value.EqualsOrdIgnoreCase(ASSIGNED))
        return "Assigned";
      if (value.EqualsOrdIgnoreCase(DONE))
        return "Done";
      if (value.EqualsOrdIgnoreCase(DEFER))
        return "Defer";
      if (value.EqualsOrdIgnoreCase(CLOSED))
        return "Closed";
      if (value.EqualsOrdIgnoreCase(CANCELED))
        return "Canceled";

      return "Invalid";
    }

    public static string[] NextState(string status)
    {
      if (NEW.EqualsOrdIgnoreCase(status))      return new string[] { ASSIGNED, DEFER,    CANCELED };   // N-> A,F,X
      if (REOPEN.EqualsOrdIgnoreCase(status))   return new string[] { ASSIGNED, DEFER,    CANCELED };   // R-> A,F,X
      if (ASSIGNED.EqualsOrdIgnoreCase(status)) return new string[] { DONE,     DEFER,    CANCELED };   // A-> D,F,X
      if (DONE.EqualsOrdIgnoreCase(status))     return new string[] { ASSIGNED, CLOSED,   CANCELED };   // D-> A,C,X
      if (DEFER.EqualsOrdIgnoreCase(status))    return new string[] { NEW,      ASSIGNED, CANCELED };   // F-> N,A,X
      if (CLOSED.EqualsOrdIgnoreCase(status))   return new string[] { REOPEN };                         // C-> R
      if (CANCELED.EqualsOrdIgnoreCase(status)) return new string[] { };                                // X-> null
      return new string[] { };
    }

    public static bool IsNextStateValid(string status, string next)
    {
      return NextState(status).Any(s => s.EqualsOrdIgnoreCase(next));
    }
    
  }

  public class ZhabaMeetingParticipationType : ZhabaEnum
  {
    public const int MAX_LEN = 3;

    public const string REMOTE = "R";
    public const string PHYSICAL = "P";

    public const string VALUE_LIST = "R: Remote, P: Physical";

    public ZhabaMeetingParticipationType() : base(DBCharType.Char, "R|P") { }

    public static string MapDescription(string value)
    {
      if (value.EqualsOrdIgnoreCase(REMOTE))
        return "Remote";
      if (value.EqualsOrdIgnoreCase(PHYSICAL))
        return "Physical";

      return "Invalid";
    }
  }
}
