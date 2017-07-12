using NFX;
using NFX.Environment;
using NFX.RelationalModel;

namespace Zhaba.Data.Domains
{
  public class ZhabaHumanName : ZhabaDomain
  {
    public const int MIN_LEN = 1;
    public const int MAX_LEN = 32;

    public ZhabaHumanName() : base() { }

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "varchar({0})".Args(MAX_LEN);
    }
  }

  public class ZhabaDescription : ZhabaDomain
  {
    public const int MAX_LEN = 512;

    public ZhabaDescription() : base() { }

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "varchar({0})".Args(MAX_LEN);
    }
  }

  public class ZhabaMnemonic : ZhabaDomain
  {
    public const int MIN_LEN = 1;
    public const int MAX_LEN = 80;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "char({0})".Args(MAX_LEN);
    }
  }

  public class ZhabaUserLogin : ZhabaDomain
  {
    public const int MIN_LEN = 3;
    public const int MAX_LEN = 32;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "varchar({0})".Args(MAX_LEN);
    }
  }

  public class ZhabaEMail : ZhabaDomain
  {
    public const int MIN_LEN = 5;
    public const int MAX_LEN = 64;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "varchar({0})".Args(MAX_LEN);
    }
  }

  public class ZhabaPasswordHash : ZhabaDomain
  {
    public const int MAX_LEN = 1024;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "varchar({0})".Args(MAX_LEN);
    }
  }

  public class ZhabaConfigScript : ZhabaDomain
  {
    public const int MIN_LEN = 6; // {z:{}}
    public const int MAX_LEN = 1024 * 128;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "TEXT";
    }

    public static ConfigSectionNode MapToConfigRoot(string content, string name = null)
    {
       if (name.IsNullOrWhiteSpace()) name = Consts.ZHABA_CONFIG_VECTOR_ROOT;

       if (content.IsNullOrWhiteSpace()) return Configuration.NewEmptyRoot(name);

       return content.AsJSONConfig(wrapRootName: name, handling: ConvertErrorHandling.Throw);
    }

    public static string MapToValue(IConfigSectionNode node)
    {
       if (node == null || !node.Exists) return null;

       return node.ToJSONString(NFX.Serialization.JSON.JSONWritingOptions.Compact);
    }
  }

  public class ZhabaSecurityRights : ZhabaConfigScript
  {
    public new const int MAX_LEN = 1024 * 8;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "TEXT";
    }

    public static NFX.Security.Rights MapToRights(string content)
    {
      if (content.IsNullOrWhiteSpace()) return NFX.Security.Rights.None;
      content = "{'" + NFX.Security.Rights.CONFIG_ROOT_SECTION + "': \n" + content + "\n}";
      var rights = content.AsJSONConfig(wrapRootName: null, handling: ConvertErrorHandling.Throw);
      return new NFX.Security.Rights(rights.Configuration);
    }

    private static readonly NFX.Serialization.JSON.JSONWritingOptions RIGHTS_OPTIONS =
                                             new NFX.Serialization.JSON.JSONWritingOptions
                                             {
                                                IndentWidth = 0,
                                                SpaceSymbols = true
                                             };

    public static string MapToValue(NFX.Security.Rights rights)
    {
      if (rights == null || rights == NFX.Security.Rights.None) return null;

      var content = rights.Root.ToJSONString(RIGHTS_OPTIONS);
      return content;
    }
  }

  public class ZhabaNote : ZhabaDomain
  {
    public const int MAX_LEN = 1024 * 2;

    public override string GetTypeName(RDBMSCompiler compiler)
    {
      return "TEXT";
    }
  }
}
