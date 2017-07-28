using System;

using NFX.ApplicationModel;
using NFX.Wave;

namespace zws
{
  internal class Program
  {
    public const string FOR_SERVICE_HANDLER = "server started...";
    
    private static void Main(string[] args)
    {
      try
      {
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        using (var app = new ServiceBaseApplication(args, null))
        using (var server = new WaveServer())
        {
          server.Configure(null);
          server.Start();

          Console.WriteLine(FOR_SERVICE_HANDLER);
          Console.ReadLine();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Critical error (press Enter to continue):");
        Console.WriteLine(ex);
        Environment.ExitCode = -1;
        Console.ReadLine();
      }
    }
  }
}