using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using OpenVPNUtils;

namespace OpenVPNManagerService
{
    /// <summary>
    /// 
    /// </summary>
    class OpenVPNService : IDisposable
    {
        private static int counter = 0;

        private string config;
        private int configID;
        private EventWaitHandle terminateOpenVPNclient;
        private string terminatorEventName;
        private string vpnPath;
        private OpenVPNServiceRunner openVPNserviceRunner;
        private Process openVPNprocess;

        private bool stopping = false;

        public OpenVPNService(OpenVPNServiceRunner openVPNserviceRunner, string vpnPath, string config)
        {
            this.openVPNserviceRunner = openVPNserviceRunner;
            this.config = config;
            this.vpnPath = vpnPath;
            this.configID = counter++;
            terminatorEventName = "OpenVPNManager_OpenVPNClientsReset_" + configID.ToString();
            terminateOpenVPNclient = new EventWaitHandle(false, EventResetMode.ManualReset, terminatorEventName);
        }

        public void Dispose()
        {
            openVPNprocess.Dispose();
            terminateOpenVPNclient.Close();
        }

        public void Start()
        {
            string configPath = Path.GetDirectoryName(config);
            string configFile = Path.GetFileName(config);

            if (!Directory.Exists(UtilsHelper.FixedLogDir))
                Directory.CreateDirectory(UtilsHelper.FixedLogDir);
            string logFile = UtilsHelper.FixedLogDir + "\\" +
                configFile + ".log";

            openVPNprocess = new Process();
            
            openVPNprocess.StartInfo.FileName = vpnPath;
            openVPNprocess.StartInfo.WorkingDirectory = configPath;
            openVPNprocess.StartInfo.Arguments = string.Format("--service {0} --config \"{1}\" --log \"{2}\"", terminatorEventName, configFile, logFile);
            openVPNprocess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            openVPNprocess.StartInfo.CreateNoWindow = true;
            openVPNprocess.EnableRaisingEvents = true;
            openVPNprocess.Exited += new EventHandler(openVPNclient_exit);
            openVPNserviceRunner.logOnConsole("Starting OpenVPN configuration: " + configFile);
            openVPNprocess.Start();
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
