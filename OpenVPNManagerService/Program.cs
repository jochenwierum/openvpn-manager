using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using OpenVPNUtils;

namespace OpenVPNManagerService
{
    /// <summary>
    /// Main program
    /// </summary>

    static class Program
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Parameters passed to the binary</param>
        [STAThread]
        static void Main(string[] args)
        {

            try
            {
                if (args.Length == 0)
                {
                    ServiceHelper.ExecuteService();
                }
                else if (args.Length == 1)
                {
                    RunDevHelper(args);
                }
                else if (args.Length == 2)
                {
                    RunSetup(args);
                }
            }
            catch (Exception ex)
            {
                string eventlogAppName = "OpenVPNManager";
                if (!EventLog.SourceExists(eventlogAppName))
                    EventLog.CreateEventSource(eventlogAppName, "Application");
                EventLog.WriteEntry(eventlogAppName, ex.ToString(), EventLogEntryType.Error, 0);
            }
        }

        private static void RunDevHelper(string[] args)
        {
            string command = args[0];
            if (command.Equals("EXECUTESERVICEASCONSOLE", StringComparison.InvariantCultureIgnoreCase))
            {
                ServiceHelper.ExecuteServiceAsConsole();
            }
        }

        private static void RunSetup(string[] args)
        {
            string command = args[0];
            string openVPNPath = args[1];

            if (command.Equals("INSTALL", StringComparison.InvariantCultureIgnoreCase))
            {
                OpenVPNManagerServiceInstaller.Install(false, openVPNPath);
            }
            else if (command.Equals("REINSTALL", StringComparison.InvariantCultureIgnoreCase))
            {
                OpenVPNManagerServiceInstaller.SetParameters(openVPNPath);
            }
            else if (command.Equals("UNINSTALL", StringComparison.InvariantCultureIgnoreCase))
            {
                OpenVPNManagerServiceInstaller.Install(true, openVPNPath);
            }
        }
    }
}