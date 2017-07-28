using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration.Install;

namespace zws.srv
{
  [RunInstaller(true)]
  public class ZWSInstaller : Installer
  {
    public ZWSInstaller()
    {
      var svcInst = new ServiceInstaller
      {
        Description = ZWSService.SERVICE_DESCRIPTION,
        DisplayName = ZWSService.DISPLAY_NAME,
        ServiceName = ZWSService.SERVICE_NAME,
        StartType = ServiceStartMode.Automatic
      };
      var svcProcInst = new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem };
      var elInst = new EventLogInstaller
      {
        Source = ZWSService.SERVICE_NAME,
        Log = "Application"
      };

      Installers.AddRange(new Installer[] { svcInst, svcProcInst, elInst });

    }
    
    internal static void Install()
    {
      var assembly = typeof(ZWSService).Assembly;
      var logFile = assembly.GetName().Name + ".InstallLog";
      var installer = new AssemblyInstaller(assembly, new[] { "/LogToConsole=false", "/LogFile=" + logFile});
      installer.UseNewContext = true;

      var data = new Hashtable();
      try
      {
        installer.Install(data);
        installer.Commit(data);
        Console.WriteLine("Installed.");
      }
      catch (ArgumentException)
      {
        Console.WriteLine(string.Format("See log: {0}", logFile));
      }
    }

    internal static void Uninstall()
    {
      var assembly = typeof(ZWSService).Assembly;
      var logFile = assembly.GetName().Name + ".UninstallLog";
      var installer = new AssemblyInstaller(assembly, new[] { "/LogToConsole=false", "/LogFile=" + logFile });
      installer.UseNewContext = true;

      try
      {
        installer.Uninstall(null);
        Console.WriteLine("Uninstalled.");
      }
      catch (ArgumentException)
      {
        Console.WriteLine(string.Format("See log: {0}", logFile));
      }
    }
    
  }
}