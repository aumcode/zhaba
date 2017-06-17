using NFX;
using NFX.RelationalModel;

namespace Zhaba.Data.Domains
{
  public class ZhabaIDRef : ZhabaDomain
  {
    public ZhabaIDRef() : base() { }

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "BIGINT(8) UNSIGNED";
    }

    public override void TransformColumnName(RDBMSCompiler compiler, RDBMSEntity column)
    {
      column.TransformedName = "C_{0}".Args(column.TransformedName);
    }
  }

  public class ZhabaIntPercent : ZhabaDomain
  {
    public const int MIN_VALUE = 0;
    public const int MAX_VALUE = 100;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "int unsigned";
    }
  }

}
