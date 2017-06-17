using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX.Security;

namespace Zhaba.Security.Permissions
{
    /// <summary>
    /// All Zhaba permissions derive from this one
    /// </summary>
    public abstract class ZhabaPermission : TypedPermission
    {
      protected ZhabaPermission(int level) : base(level) { }

      public override string Path
      {
        get
        {
          var ns = GetType().Namespace;
          ns = ns.Replace("Zhaba.Security.Permissions", string.Empty); //get rid of unneeded prefix for Zhaba permissions
          ns = ns.Replace('.', '/');
          if (!ns.StartsWith("/")) ns = '/' + ns;
          return ns;
        }
      }
    }
}
