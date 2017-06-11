using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX.DataAccess.CRUD;

namespace Zhaba.Web
{
  public interface ISiteControl{}

  public interface IGridControl
  {
    RowsetBase DataSource { get; set;}
  }


  public enum GridSection { Undefined=0, Table, Head, Body, Foot }

  public delegate object TableGridGetColumnValueFunc(ITableGridControl control,
                                                     Schema.FieldDef fdef,
                                                     bool isHead,
                                                     object value,
                                                     ref bool encode);

  public delegate object TableGridGetValueFunc(ITableGridControl control,
                                               Schema.FieldDef fdef,
                                               Row row,
                                               object value,
                                               ref bool encode);

  public interface ITableGridControl : IGridControl
  {
    string ID{ get;}
    bool HasHead { get; set; }
    bool HasFoot { get; set; }

    TableGridGetColumnValueFunc OnGetColumnValue {get; set;}
    TableGridGetValueFunc OnGetValue {get; set;}
  }

}
