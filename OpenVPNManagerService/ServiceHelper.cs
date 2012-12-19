using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;
using System.Collections;
using System.Security;
using System.ComponentModel;
using System.Resources;
using System.Windows.Forms;

namespace OpenVPNManagerService
{
    public static class ServiceHelper
    {
        class NativeMethods
        {
            [DllImport("psapi.dll")]
            public static extern int EmptyWorkingSet(IntPtr hwProc);
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
        }

        // 'Static handler' trick to avoid VisualStudio debugging bug, where the SetConsoleCtrlHandler causes strange errors..
        // http://connect.microsoft.com/VisualStudio/feedback/details/524889/debugging-c-console-application-that-handles-console-cancelkeypress-is-broken-in-net-4-0
        static NativeMethods.HandlerRoutine _handler; 

        static OpenVPNServiceRunner service;

        public static void ExecuteService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new OpenVPNServiceRunner() };
            ServiceBase.Run(ServicesToRun);
        }

        public static void ExecuteServiceAsConsole()
        {
            NativeMethods.AllocConsole();
            service = new OpenVPNServiceRunner();
            _handler += new NativeMethods.HandlerRoutine(ConsoleCtrlCheck);

            // Allow console to properly take shutdown also on Ctrl+C or closing the Console window.
            NativeMethods.SetConsoleCtrlHandler(_handler, true);

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

        private static bool ConsoleCtrlCheck(NativeMethods.CtrlTypes ctrlType)
        {
            service.Shutdown();
            return true;
        }

        public static void MinimizeFootprint()
        {
            NativeMethods.EmptyWorkingSet(Process.GetCurrentProcess().Handle);
        }
    }
}
