using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace OpenVPNManager
{
    /// <summary>
    /// 
    /// </summary>
    class OpenVPNservice
    {
        static int counter = 0;
        String config;
        int configID;
        EventWaitHandle terminateOpenVPNclient;
        String terminatorEventName;
        bool stopping = false;
        OpenVPNserviceRunner openVPNserviceRunner;

        public OpenVPNservice(OpenVPNserviceRunner openVPNserviceRunner, String config)
        {
            this.openVPNserviceRunner = openVPNserviceRunner;
            this.config = config;
            this.configID = counter++;
            terminatorEventName = "OpenVPNManager_OpenVPNClientsReset_" + configID.ToString();
            terminateOpenVPNclient = new EventWaitHandle(false, EventResetMode.ManualReset, terminatorEventName);
        }

        public void Start()
        {
            String configPath = Path.GetDirectoryName(config);
            String configFile = Path.GetFileName(config);
            String logFile = helper.fixedLogDir + "\\" + VPNConfig.getDescriptiveName(config)+".log";

            Process p = new Process();

            p.StartInfo.FileName = "openvpn.exe";
            p.StartInfo.WorkingDirectory = configPath;
            p.StartInfo.Arguments = String.Format("--service {0} --config \"{1}\" --log \"{2}\"", terminatorEventName, configFile, logFile);
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler(openVPNclient_exit);
            openVPNserviceRunner.logOnConsole("Starting OpenVPN configuration: " + configFile);
            p.Start();
            ServiceHelper.MinimizeFootprint();
        }

        public void Stop()
        {
            stopping = true;
            terminateOpenVPNclient.Set();
        }

        private void openVPNclient_exit(object sender, System.EventArgs e)
        {
            if (!stopping)
            {
                // restart OpenVPN.exe becouse stop was unexpected.
                // this can happen when OpenVPN enqounters a 'unresovlable' situation (like asking for a password which needs to be aborted..)
                Start();
            }
        }
    }
}
