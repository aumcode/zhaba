using NFX;
using NFX.RelationalModel;
using NFX.RelationalModel.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaba.Data.Domains
{
  public abstract class ZhabaDomain : RDBMSDomain
  {
    protected ZhabaDomain() : base() { }
  }

  public class ZhabaDate : ZhabaDomain
  {
    public ZhabaDate() : base() { }

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "DATE";
    }
  }

  public abstract class ZhabaEnum : ZhabaDomain
  {
    public readonly DBCharType Type;
    public readonly int Size;
    public readonly string[] Values;

    protected ZhabaEnum(DBCharType type, string values)
    {
      Type = type;
      var vlist = values.Split('|');
      Size = vlist.Max(v => v.Trim().Length);
      if (Size < 1) Size = 1;
      Values = vlist;
    }

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return Type == DBCharType.Varchar ? "VARCHAR({0})".Args(Size) : "CHAR({0})".Args(Size);
    }

    public override string GetColumnCheckScript(RDBMSCompiler compiler, RDBMSEntity column, Compiler.Outputs outputs)
    {
      var enumLine = string.Join(", ", Values.Select(v => compiler.EscapeString(v.Trim())));
      return compiler.TransformKeywordCase("check ({0} in ({1}))")
                      .Args(
                            compiler.GetQuotedIdentifierName(RDBMSEntityType.Column, column.TransformedName),
                            enumLine
                          );
    }
  }
}
