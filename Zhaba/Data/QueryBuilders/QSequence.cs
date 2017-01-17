using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX.DataAccess.CRUD;

namespace Zhaba.Data.QueryBuilders
{
  public static class QSequence
  {
    public static Query<TRow> SequenceByName<TRow>(string scopeName, string sequenceName) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Sequence.ByName")
      {
         new Query.Param("scopeName", scopeName),
         new Query.Param("sequenceName", sequenceName)
      };
    }

    public static Query<TRow> SequencesByScopeName<TRow>(string scopeName) where TRow : Row
    {
      return new Query<TRow>("SQL.CRUD.Sequence.ByScopeName")
      {
         new Query.Param("name", scopeName)
      };
    }

    public static Query SequenceScopes()
    {
      return new Query("SQL.CRUD.Sequence.AllScopes");
    }
  }
}
