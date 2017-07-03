using System;
using MySql.Data.MySqlClient;
using NFX.DataAccess.MySQL;
using Zhaba.Data.Filters;
using Zhaba.DBAccess.Handlers;
using NFX;

namespace Zhaba.DBAccess.SQL.Filters
{
  public class CategoryList : ZhabaFilteredMySQLQueryHandler<CategoryListFilter>
  {
    public CategoryList(MySQLDataStore store, string name) : base(store, name)
    {
    }

    protected override void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd,
      CategoryListFilter filter)
    {
      string where = string.Empty;

      var name = filter.Name.ParseName();
      if (name != null)
      {
        where += "AND (TP.NAME LIKE ?pName)";
        cmd.Parameters.AddWithValue("pName", name);
      }

      var description = filter.Description.ParseName();
      if (description != null)
      {
        where += "AND (TP.DESCRIPTION LIKE ?pDescription)";
        cmd.Parameters.AddWithValue("pDescription", description);
      }

      // first - number of column, second - OrderBy direction
      var orderBy = "1 ASC";
      if (filter.OrderBy.IsNotNullOrWhiteSpace())
      {
        orderBy = filter.OrderBy;
      }

      cmd.CommandText =
@"SELECT *
FROM tbl_category TP
WHERE (IN_USE = 'T') {0}
ORDER BY {1}".Args(where, orderBy);
    }
  }
}
