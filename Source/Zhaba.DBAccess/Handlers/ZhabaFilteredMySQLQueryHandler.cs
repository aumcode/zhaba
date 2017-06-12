using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.MySQL;

using Zhaba.Data.Forms;

namespace Zhaba.DBAccess.Handlers
{
  /// <summary>
  /// Denotes query handlers that operate on filter forms
  /// </summary>
  public abstract class ZhabaFilteredMySQLQueryHandler<TFilter> : ZhabaMySQLQueryHandler<TFilter> where TFilter : ZhabaFilterForm
  {
    public ZhabaFilteredMySQLQueryHandler(MySQLDataStore store, string name) : base(store, name)
    {
    }

    protected StringBuilder BuildDatesFilter(string colName, List<Tuple<DateTime?, DateTime?>> dates, MySqlCommand cmd)
    {
      var sb = new StringBuilder();

      for (var i = 0; i < dates.Count; i++)
      {
        var sd = dates[i].Item1;
        var ed = dates[i].Item2;
        if (i > 0) sb.Append(" OR ");
        sb.Append(" (");
        if (sd.HasValue)
        {
          sb.AppendLine("({0} >= ?pSSD{1})".Args(colName, i));
          cmd.Parameters.AddWithValue("pSSD{0}".Args(i), sd.Value);
        }

        if (ed.HasValue)
        {
          if (sd.HasValue) sb.Append(" AND ");
          sb.AppendLine("({0} <= ?pSED{1})".Args(colName, i));
          cmd.Parameters.AddWithValue("pSED{0}".Args(i), ed.Value);
        }
        sb.Append(") ");
      }

      return sb;
    }

  }
}
