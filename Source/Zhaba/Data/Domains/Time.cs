using NFX;
using NFX.RelationalModel;

namespace Zhaba.Data.Domains
{
  public class ZhabaDate : ZhabaDomain
  {
    public ZhabaDate() : base() { }

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "DATE";
    }
  }
}
