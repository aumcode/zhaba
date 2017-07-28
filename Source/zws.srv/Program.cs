using System;
using System.ServiceProcess;

namespace zws.srv
{
  public class Program
  {
    static void Main(string[] args)
    {
      if (Environment.UserInteractive)
      {
        try
        {
          run(args);
          Environment.ExitCode = 0;
        }
        catch (Exception error)
        {
          Console.WriteLine(error.ToString());
          Environment.ExitCode = -1;
        }
      }
      else
      {
        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[] { new ZWSService() };
        ServiceBase.Run(ServicesToRun);
      }
    }

    static void run(string[] args)
    {
      if (args.Length == 1 && "/i".Equals(args[0], StringComparison.OrdinalIgnoreCase))
      {
        ZWSInstaller.Install();
      }
      else if (args.Length == 1 && "/u".Equals(args[0], StringComparison.OrdinalIgnoreCase))
      {
        ZWSInstaller.Uninstall();
      }
      else
      {
        ZWSService srv = new ZWSService();
        srv.TestStartupAndStop(args);
      }
    }
  }
}