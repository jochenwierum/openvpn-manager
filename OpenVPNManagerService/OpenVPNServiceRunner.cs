using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using OpenVPNUtils;
using Microsoft.Win32;

namespace OpenVPNManagerService
{
    partial class OpenVPNServiceRunner : ServiceBase
    {
        List<OpenVPNService> openVPNservices = new List<OpenVPNService>();
        bool runningOnConsole = false;
        public OpenVPNServiceRunner()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Startup();
        }

        public void StartAsConsole()
        {
            runningOnConsole = true;
            Startup();
        }

        public void logOnConsole(String line)
        {
            if (runningOnConsole)
                Console.WriteLine(line);
        }

        private void Startup()
        {
            string vpnPath = GetVPNPath();

            if (vpnPath == null) {
                string eventlogAppName = "OpenVPNManager";
                if (!EventLog.SourceExists(eventlogAppName))
                    EventLog.CreateEventSource(eventlogAppName, "Application");
                EventLog.WriteEntry(eventlogAppName, "Will not start - no service parameters found", EventLogEntryType.Error, 0);
                return;
            }

            List<string> configs = UtilsHelper.LocateOpenVPNManagerConfigs(true);
            foreach (String config in configs)
            {
                OpenVPNService openVPNservice = new OpenVPNService(this, vpnPath, config);
                openVPNservices.Add(openVPNservice);
                openVPNservice.Start();
            }
            ServiceHelper.MinimizeFootprint();
            Timer timerReference = new Timer(
                delegate(object state)
                {
                    ServiceHelper.MinimizeFootprint();
                    timerReference = null;
                }, null, 5000, 0);
        }

        private string GetVPNPath()
        {
            RegistryKey k = Registry.LocalMachine.OpenSubKey(
                @"SYSTEM\CurrentControlSet\services\OpenVPNManager", true);

            String result = null;
            if (k != null)
            {
                result = (string) k.GetValue("Parameters");
                k.Close();
            }

            return result;
        }

        public void Shutdown()
        {
            foreach (OpenVPNService openVPNservice in openVPNservices)
            {
                openVPNservice.Stop();
            }
        }

        protected override void OnStop()
        {
            Shutdown();
        }
    }
}
