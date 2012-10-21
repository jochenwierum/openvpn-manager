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
    class OpenVPNservice : IDisposable
    {
        static int counter = 0;
        String config;
        int configID;
        EventWaitHandle terminateOpenVPNclient;
        String terminatorEventName;
        bool stopping = false;
        OpenVPNserviceRunner openVPNserviceRunner;
        Process openVPNprocess;

        public OpenVPNservice(OpenVPNserviceRunner openVPNserviceRunner, String config)
        {
            this.openVPNserviceRunner = openVPNserviceRunner;
            this.config = config;
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
            String configPath = Path.GetDirectoryName(config);
            String configFile = Path.GetFileName(config);

            if (!Directory.Exists(helper.fixedLogDir))
                Directory.CreateDirectory(helper.fixedLogDir);
            String logFile = helper.fixedLogDir + "\\" + VPNConfig.getDescriptiveName(config)+".log";

            openVPNprocess = new Process();
            
            openVPNprocess.StartInfo.FileName = helper.openVPNexe;
            openVPNprocess.StartInfo.WorkingDirectory = configPath;
            openVPNprocess.StartInfo.Arguments = String.Format("--service {0} --config \"{1}\" --log \"{2}\"", terminatorEventName, configFile, logFile);
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
