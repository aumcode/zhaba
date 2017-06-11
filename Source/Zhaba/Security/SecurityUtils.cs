using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFX;

using Zhaba.Data.Rows;

namespace Zhaba.Security
{
  public static class SecurityUtils
  {
    /// <summary>
    /// Hashes plain user password. DB must store only hashed passwords
    /// </summary>
    public static string HashUserPassword(string pwd, string salt)
    {
      return (pwd+salt).ToMD5String();
    }
  }
}
