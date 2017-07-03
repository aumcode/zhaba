using MySql.Data.MySqlClient;
using NFX;
using NFX.DataAccess.MySQL;
using Zhaba.Data.Filters;
using Zhaba.DBAccess.Handlers;

namespace Zhaba.DBAccess.SQL.Filters
{
  class UserList : ZhabaFilteredMySQLQueryHandler<UserListFilter>
  {
    public UserList(MySQLDataStore store, string name) : base(store, name)
    {
    }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd,
      UserListFilter filter)
    {
      string where = string.Empty;

      var login = filter.Login.ParseName();
      if (login != null)
      {
        where += "AND (TP.LOGIN LIKE ?pLogin)";
        cmd.Parameters.AddWithValue("pLogin", login);
      }

      var firstName = filter.First_Name.ParseName();
      if (firstName != null)
      {
        where += "AND (TP.FIRST_NAME LIKE ?pFirstName)";
        cmd.Parameters.AddWithValue("pFirstName", firstName);
      }

      var lastName = filter.Last_Name.ParseName();
      if (lastName != null)
      {
        where += "AND (TP.LAST_NAME LIKE ?pLastName)";
        cmd.Parameters.AddWithValue("pLastName", lastName);
      }

      var email = filter.EMail.ParseName();
      if (email != null)
      {
        where += "AND (TP.EMAIL LIKE ?pEMail)";
        cmd.Parameters.AddWithValue("pEMail", email);
      }

      // first - number of column, second - OrderBy direction
      var orderBy = "1 ASC";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        orderBy = filter.OrderBy;
      }

      cmd.CommandText =
        @"SELECT *
FROM tbl_user TP
WHERE (1 = 1) {0}
ORDER BY IN_USE DESC, {1}".Args(where, orderBy);
    }
  }
}