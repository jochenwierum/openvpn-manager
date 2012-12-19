using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using System.ComponentModel;
using System.Configuration.Install;
using System.Collections;
using Microsoft.Win32;

namespace OpenVPNManagerService
{
    [RunInstaller(true)]
    public sealed class OpenVPNManagerServiceInstaller : ServiceInstaller
    {
        public OpenVPNManagerServiceInstaller()
        {
            this.Description = "Automatically starts and restart OpenVPN processes for OpenVPN Manager";
            this.DisplayName = "OpenVPN Manager";
            this.ServiceName = "OpenVPNManager";
            this.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
        }

        public static void Install(bool undo, string openvpn)
        {
            try
            {
                using (AssemblyInstaller inst = new AssemblyInstaller(typeof(OpenVPNServiceRunner).Assembly, new String[0]))
                {
                    IDictionary state = new Hashtable();
                    inst.UseNewContext = true;
                    try
                    {
                        if (undo)
                        {
                            inst.Uninstall(state);
                        }
                        else
                        {
                            inst.Install(state);
                            inst.Commit(state);
                            SetParameters(openvpn);
                        }
                    }
                    catch
                    {
                        try
                        {
                            inst.Rollback(state);
                        }
                        catch { }
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public static void SetParameters(string openvpn)
        {
            RegistryKey k = Registry.LocalMachine.OpenSubKey(
                @"SYSTEM\CurrentControlSet\services\OpenVPNManager", true);

            if (k != null)
            {
                k.SetValue("Parameters", openvpn);
                k.Close();
            }
        }
    }
}
