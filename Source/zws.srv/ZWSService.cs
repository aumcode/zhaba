using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using NFX;

namespace zws.srv
{
  public partial  class ZWSService : ServiceBase
  {
    public const string SERVICE_NAME        = "ZWSService";
    public const string SERVICE_DESCRIPTION = "Zhaba Root Daemon Service";
    public const string DISPLAY_NAME        = "Zhaba Root Daemon";
    
    public const string FOR_SERVICE_HANDLER = "server started...";

    public ZWSService()
    {
      ServiceName = SERVICE_NAME;
    }
    
    private bool m_FirstLaunchOK;
    private bool m_Stopping;
    private string m_ZWSExecutable;
    private Process m_ZWSProcess;

    protected override void OnStart(string[] args)
    {
      try
      {
        m_ZWSExecutable = Utils.GetZWSExecutablePath();

        startProcess(true);
        m_FirstLaunchOK = true;
      }
      catch (Exception error)
      {
        EventLog.WriteEntry(string.Format("ZWS Service start failed with exception '{0}'. Message: {1}", error.GetType().FullName, error.Message), EventLogEntryType.Error);
        throw error;
      }
    }
    
    protected override void OnStop()
    {
      m_Stopping = true;
      closeProcess();
    }

    private void ZwsProcessExited(object sender, EventArgs args)
    {
      if (!m_FirstLaunchOK) return;//if this is not a subsequent restart
      if (m_Stopping) return;//if the Win Service stopping then do not restart anyting


      EventLog.WriteEntry("ZWS.SRV exited. Will check for newer version now and the respawn the AHGOV");
      closeProcess();

      var updateProblem = false;

      //need to find latest version, then rename it into "RUN"
      var updateDir = Utils.GetUpdateDir();
      if (Utils.UpdatePathValid(updateDir))
      {
        EventLog.WriteEntry("Newer version of run packages found. Replacing current RUN dir with: " + updateDir);
        try
        {
          Utils.ReplaceRUN_With_UPDATE(updateDir);
        }
        catch (Exception error)
        {
          updateProblem = true;
          EventLog.WriteEntry(string.Format("ZWS Service replace RUN with UPDATE failed with exception: '{0}'", error.Message), EventLogEntryType.Error);
        }
      }//else, if there is nothing to update, just restart
      try
      {
        startProcess(false, updateProblem);
      }
      catch (Exception error)
      {
        EventLog.WriteEntry(string.Format("ZWS Service respawn failed with exception '{0}'. Message: {1}", error.GetType().FullName, error.Message), EventLogEntryType.Error);
        this.Stop();
      }
    }
    
    private void startProcess(bool onStart, bool updateProblem = false)
    {
      string args="";

/*      if (updateProblem)
        args = string.Format("-{0} -{1}", Utils.ZWS_PARENT_CMD_PARAM, Utils.ZWS_UPDATE_PROBLEM_CMD_PARAM);
      else
        args = string.Format("-{0}", Utils.ZWS_PARENT_CMD_PARAM);*/

      m_ZWSProcess = new Process();
      m_ZWSProcess.StartInfo.FileName = m_ZWSExecutable;
      m_ZWSProcess.StartInfo.Arguments = args;
      m_ZWSProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(m_ZWSExecutable);
      m_ZWSProcess.StartInfo.UseShellExecute = false;
      m_ZWSProcess.StartInfo.CreateNoWindow = true;
      m_ZWSProcess.StartInfo.RedirectStandardInput = true;
      m_ZWSProcess.StartInfo.RedirectStandardOutput = true;
      m_ZWSProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      m_ZWSProcess.EnableRaisingEvents = true;//this must be true to get events
      m_ZWSProcess.Exited += ZwsProcessExited;

      EventLog.WriteEntry("Starting zws.srv: " + m_ZWSExecutable);
      m_ZWSProcess.Start();
      EventLog.WriteEntry("zws.srv process started, Waiting for OK..");

      if (onStart && !Environment.UserInteractive) this.RequestAdditionalTime(2000);

      const int timeout = 15000;
      var watch = Stopwatch.StartNew();

      while (!m_ZWSProcess.HasExited &&
              m_ZWSProcess.StandardOutput.EndOfStream &&
              watch.ElapsedMilliseconds < timeout)
      {
        if (onStart && !Environment.UserInteractive) this.RequestAdditionalTime(1000);
        Thread.Sleep(500);
      }

      if (m_ZWSProcess.HasExited) throw new Exception("ZWS.SRV process crashed while startup, see its logs");
      if (!m_ZWSProcess.StandardOutput.EndOfStream)
      {

        while (!m_ZWSProcess.StandardOutput.EndOfStream)
        {
          var result = m_ZWSProcess.StandardOutput.ReadLine();
          Console.WriteLine(result);
          if ("server started...".EqualsOrdIgnoreCase(result)) return;
        }

      }

      throw new Exception("zws.srv did not return success code, see its logs (OR maybe something was written to STDOUT by mistake before OK?)");
    }

    private void closeProcess()
    {
      if (m_ZWSProcess != null)
      {
        if (!m_ZWSProcess.HasExited)
        {
          EventLog.WriteEntry("Sending zws.srv a line to gracefully exit and waiting...");
          m_ZWSProcess.StandardInput.WriteLine("");//Gracefully tells AHGOV to exit
          m_ZWSProcess.WaitForExit();
          EventLog.WriteEntry("zws.srv Exited");
        }
        m_ZWSProcess.Close();
        m_ZWSProcess = null;
      }
    }

    internal void TestStartupAndStop(string[] args)
    {
      Console.WriteLine("Waiting for line to terminate...");
      try
      {
        this.OnStart(args);
        Console.ReadLine();
      }
      finally
      {
        this.OnStop();
      }
    }

  }
}