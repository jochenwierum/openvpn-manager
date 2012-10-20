using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace OpenVPNManager
{
    partial class OpenVPNserviceRunner : ServiceBase
    {
        List<OpenVPNservice> openVPNservices = new List<OpenVPNservice>();
        bool runningOnConsole = false;
        public OpenVPNserviceRunner()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServiceHelper.MinimizeFootprint();
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

        void Startup()
        {
            List<string> configs = helper.locateOpenVPNManagerConfigs(true);
            foreach (String config in configs)
            {
                OpenVPNservice openVPNservice = new OpenVPNservice(this, config);
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

        public void Shutdown()
        {
            foreach (OpenVPNservice openVPNservice in openVPNservices)
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
