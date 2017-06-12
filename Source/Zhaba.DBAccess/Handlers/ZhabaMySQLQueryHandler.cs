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
  public abstract class ZhabaMySQLQueryHandler : CRUDQueryHandler<MySQLDataStore>
  {

    protected ZhabaMySQLQueryHandler(MySQLDataStore store, string name) : base(store, name)
    {
    }

    public override RowsetBase Execute(ICRUDQueryExecutionContext context, Query query, bool oneRow = false)
    {
      throw new NotImplementedException(this.GetType().FullName+".Execute()");
    }

    public override Task<RowsetBase> ExecuteAsync(ICRUDQueryExecutionContext context, Query query, bool oneRow = false)
    {
      throw new NotImplementedException(this.GetType().FullName+".ExecuteAsync()");
    }

    public override Cursor OpenCursor(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException(this.GetType().FullName+".OpenCursor()");
    }

    public override Task<Cursor> OpenCursorAsync(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException(this.GetType().FullName+".OpenCursorAsync()");
    }

    public override int ExecuteWithoutFetch(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException(this.GetType().FullName+".ExecuteWithoutFetch()");
    }

    public override Task<int> ExecuteWithoutFetchAsync(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException(this.GetType().FullName+".ExecuteWithoutFetchAsync()");
    }

    public override Schema GetSchema(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException(this.GetType().FullName+".GetSchema()");
    }

    public override Task<Schema> GetSchemaAsync(ICRUDQueryExecutionContext context, Query query)
    {
      throw new NotImplementedException(this.GetType().FullName+".GetSchemaAsync()");
    }

  }

  /// <summary>
  /// Denotes a base for programmatic query handlers based on some QueryParams object
  /// </summary>
  public abstract class ZhabaMySQLQueryHandler<TQueryParameters> : ZhabaMySQLQueryHandler where TQueryParameters : class
  {
    public ZhabaMySQLQueryHandler(MySQLDataStore store, string name) : base(store, name){}


    public sealed override RowsetBase Execute(ICRUDQueryExecutionContext context, Query query, bool oneRow = false)
    {
       if (query.Count<1) return null;
       var qParams = query[0].Value as TQueryParameters;
       if (qParams==null) return null;
       var ctx = (NFX.DataAccess.MySQL.MySQLCRUDQueryExecutionContext)context;
       return DoExecuteFilteredQuery(ctx, query, qParams);
    }

    public sealed override Task<RowsetBase> ExecuteAsync(ICRUDQueryExecutionContext context, Query query, bool oneRow = false)
    {
       return TaskUtils.AsCompletedTask( () => this.Execute(context, query, oneRow) );
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
          catch(Exception error)
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
