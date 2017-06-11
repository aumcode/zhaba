using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;
using NFX.Security;
using NFX.Environment;

namespace Zhaba.Security
{
  /// <summary>
  /// Represents very simple uint ID textual credentials
  /// </summary>
  [Serializable]
  public class ULongIDCredentials : Credentials, IStringRepresentableCredentials
  {
     public ULongIDCredentials(ulong id)
     {
       m_ID = id;
     }

     /// <summary>
     /// Warning: storing plain credentials in config file is not secure. Use this method for the most simplistic cases
     /// like unit testing
     /// </summary>
     public ULongIDCredentials(IConfigSectionNode cfg)
     {
       if (cfg == null || !cfg.Exists)
         throw new SecurityException("ULongIDCredentials.ctor(cfg=null|!exists)");

       ConfigAttribute.Apply(this, cfg);
     }

     [Config] private ulong m_ID;

     public ulong ID { get { return m_ID; } }

     public override void Forget()
     {
       m_ID = 0;
       base.Forget();
     }

     public string RepresentAsString()
     {
       if (Forgotten)
         throw new SecurityException("Credentials are forgotten");

       return m_ID.ToString();
     }

     public override string ToString()
     {
       return "{0}({1})".Args(GetType().Name, ID);
     }
  }
}
