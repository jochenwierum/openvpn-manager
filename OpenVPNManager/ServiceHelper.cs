using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.Security;
using System.ComponentModel;
using System.Resources;

namespace OpenVPNManager
{
    class ServiceHelper
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        // 'Static handler' trick to avoid VisualStudio debugging bug, where the SetConsoleCtrlHandler causes strange errors..
        static HandlerRoutine _handler; 
        // http://connect.microsoft.com/VisualStudio/feedback/details/524889/debugging-c-console-application-that-handles-console-cancelkeypress-is-broken-in-net-4-0

        static OpenVPNserviceRunner service;

        internal static void installService()
        {
            try
            {
                var serviceInstaller = new ServiceInstaller();
                serviceInstaller.ServiceName = "OpenVPNManager";
                serviceInstaller.StartType = ServiceStartMode.Automatic;

                var serviceProcessInstaller = new ServiceProcessInstaller();
                serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
                serviceProcessInstaller.Password = null;
                serviceProcessInstaller.Username = null;

                var projectInstaller = new Installer();
                projectInstaller.Installers.Add(serviceInstaller);
                projectInstaller.Installers.Add(serviceProcessInstaller);

                var transactedInstaller = new TransactedInstaller();
                transactedInstaller.Installers.Add(projectInstaller);
                transactedInstaller.Context = new InstallContext();
                transactedInstaller.Context.Parameters["assemblypath"] = Assembly.GetExecutingAssembly().Location + "\" \"/EXECUTESERVICE";
                transactedInstaller.Install(new Hashtable());
            }
            catch (InvalidOperationException e)
            {
                if (e.InnerException != null && e.InnerException is Win32Exception)// Probably: "Service already exists." 
                    MessageBox.Show("Error: " + e.InnerException.Message);
                else if (e.InnerException != null && e.InnerException is InvalidOperationException && e.InnerException.InnerException != null && e.InnerException.InnerException is Win32Exception)// Probably: "Permission denied"
                {
                    String MSG_ServiceInstallPermissionErrorAdvice = Program.res.GetString("MSG_ServiceInstallPermissionErrorAdvice");
                    MessageBox.Show("Error: " + e.InnerException.InnerException.Message + "\r\n\r\n" + MSG_ServiceInstallPermissionErrorAdvice);
                }
                else
                    throw;
            }
        }

        internal static void uninstallService()
        {
            try
            {
                ServiceInstaller serviceInstaller = new ServiceInstaller();
                serviceInstaller.ServiceName = "OpenVPNManager";
                TransactedInstaller transactedInstaller = new TransactedInstaller();
                transactedInstaller.Installers.Add(serviceInstaller);
                transactedInstaller.Uninstall(null);
            }
            catch (InstallException e)
            {
                if (e.InnerException != null && e.InnerException is SecurityException)// Probably: "Permission denied"
                {
                    String MSG_ServiceInstallPermissionErrorAdvice = Program.res.GetString("MSG_ServiceInstallPermissionErrorAdvice");
                    MessageBox.Show("Error: " + e.InnerException.Message + "\r\n\r\n" + MSG_ServiceInstallPermissionErrorAdvice);
                }
                else if (e.InnerException != null && e.InnerException is Win32Exception)// Probably: "Service does not exist."
                    MessageBox.Show("Error: " + e.InnerException.Message);
                else
                    throw;
            }
        }

        internal static void executeService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new OpenVPNserviceRunner() };
            ServiceBase.Run(ServicesToRun);
        }

        internal static void executeServiceAsConsole()
        {
            AllocConsole();
            service = new OpenVPNserviceRunner();
            _handler += new HandlerRoutine(ConsoleCtrlCheck);
            SetConsoleCtrlHandler(_handler, true);// Allow console to properly take shutdown also on Ctrl+C or closing the Console window.

            service.StartAsConsole();
            try
            {
                string input = string.Empty;
                Console.WriteLine("\r\nType \"exit\" and press Enter to stop.");
                while (input != null && input.ToLower() != "exit")
                    input = Console.ReadLine();
                service.Shutdown();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            service.Shutdown();
            return true;
        }

        [DllImport("psapi.dll")]
        static extern int EmptyWorkingSet(IntPtr hwProc);

        public static void MinimizeFootprint()
        {
            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
        }
    }
}
