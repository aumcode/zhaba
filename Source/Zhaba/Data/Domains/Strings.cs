using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.RelationalModel;

namespace Zhaba.Data.Domains
{
  public class ZhabaName : ZhabaDomain
  {
    public const int MIN_LEN = 3;
    public const int MAX_LEN = 50;

    public ZhabaName() : base() { }

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "VARCHAR({0})".Args(MAX_LEN);
    }
  }

  public class ZhabaDescription : ZhabaDomain
  {
    public const int MAX_LEN = 80;

    public ZhabaDescription() : base() { }

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "VARCHAR({0})".Args(MAX_LEN);
    }
  }

  public class ZhabaMnemonic : ZhabaDomain
  {
    public const int MIN_LEN = 1;
    public const int MAX_LEN = 20;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "CHAR({0})".Args(MAX_LEN);
    }
  }
}
