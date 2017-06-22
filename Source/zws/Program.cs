using NFX.ApplicationModel;
using NFX.DataAccess.CRUD;
using NFX.Wave;
using System;

namespace zws
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      try
      {
        using (var app = new ServiceBaseApplication(args, null))
        using (var server = new WaveServer())
        {
          server.Configure(null);
          server.Start();

          Console.WriteLine("server started...");
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