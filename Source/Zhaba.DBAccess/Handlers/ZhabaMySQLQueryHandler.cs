using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using NFX;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.MySQL;

namespace Zhaba.DBAccess.Handlers
{
  public abstract class ZhabaMySQLQueryHandler : ICRUDQueryHandler
  {
    protected ZhabaMySQLQueryHandler(MySQLDataStore store)
    {
      m_DataStore = store;
    }

    protected readonly MySQLDataStore m_DataStore;

    public string Name
    {
      get
      {
        var result = GetType().Name;
        return result;
      }
    }

    public ICRUDDataStore Store
    {
      get { return m_DataStore; }
    }

    public abstract RowsetBase Execute(ICRUDQueryExecutionContext context, Query query, bool oneRow = false);

    public Task<RowsetBase> ExecuteAsync(ICRUDQueryExecutionContext context, Query query, bool oneRow = false)
    {
      return TaskUtils.AsCompletedTask(() => this.Execute(context, query, oneRow));
    }

    public int ExecuteWithoutFetch(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException();
    }

    public Task<int> ExecuteWithoutFetchAsync(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException();
    }

    public Schema GetSchema(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException();
    }

    public Task<Schema> GetSchemaAsync(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException();
    }

    public Cursor OpenCursor(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException();
    }

    public Task<Cursor> OpenCursorAsync(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException();
    }
  }

  /// <summary>
  /// Denotes a base for programmatic query handlers based on some QueryParams object
  /// </summary>
  public abstract class ZhabaMySQLQueryHandler<TQueryParameters> : ZhabaMySQLQueryHandler where TQueryParameters : class
  {
    public ZhabaMySQLQueryHandler(MySQLDataStore store) : base(store) { }


    public sealed override RowsetBase Execute(ICRUDQueryExecutionContext context, Query query, bool oneRow = false)
    {
      if (query.Count < 1) return null;
      var qParams = query[0].Value as TQueryParameters;
      if (qParams == null) return null;
      var ctx = (NFX.DataAccess.MySQL.MySQLCRUDQueryExecutionContext)context;
      return DoExecuteFilteredQuery(ctx, query, qParams);
    }

    protected virtual RowsetBase DoExecuteFilteredQuery(MySQLCRUDQueryExecutionContext ctx, Query query, TQueryParameters queryParameters)
    {
      using (var cmd = ctx.Connection.CreateCommand())
      {
        DoBuildCommandAndParms(ctx, cmd, queryParameters);

        ctx.ConvertParameters(cmd.Parameters);

        cmd.Transaction = ctx.Transaction;

        MySqlDataReader reader = null;
        try
        {
          reader = cmd.ExecuteReader();
          GeneratorUtils.LogCommand(ctx.DataStore.LogLevel, "DoExecuteFilteredQuery-ok", cmd, null);
        }
        catch (Exception error)
        {
          GeneratorUtils.LogCommand(ctx.DataStore.LogLevel, "DoExecuteFilteredQuery-error", cmd, error);
          throw;
        }

        using (reader)
          return MySQLCRUDScriptQueryHandler.PopulateRowset(ctx, reader, ctx.DataStore.TargetName, query, null, false);

      }//using command

    }

    protected abstract void DoBuildCommandAndParms(MySQLCRUDQueryExecutionContext context, MySqlCommand cmd, TQueryParameters queryParameters);
  }
}
