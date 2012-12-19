using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Security;
using System.Reflection;
using OpenVPNUtils;


namespace OpenVPNManager
{
    static class Helper
    {


        /// <summary>
        /// Returns a list of files which are used by the OpenVPN Service.
        /// </summary>
        /// <returns></returns>
        public static List<string> LocateOpenVPNServiceConfigs()
        {
            List<string> files = new List<string>();
            if (!CanUseService())
                return files;

            try
            {
                UtilsHelper.GetConfigFiles(
                    new DirectoryInfo(UtilsHelper.LocateOpenVPNServiceDir()),
                    files, Helper.LocateOpenVPNServiceFileExt(), false);
            }
            catch (DirectoryNotFoundException)
            { }
            return files;
        }

        public static String IconsDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\icons";
            }
        }

        /// Indicates whether the OpenVPN Service can be used.
        /// </summary>
        /// <returns>true, if the OpenVPN Service can be used, false otherwise.</returns>
        public static bool CanUseService()
        {
            if (!Helper.ServiceKeyExists())
                return false;

            // Config directory AND file extension are the same

            string serviceDir = UtilsHelper.LocateOpenVPNServiceDir();
            if (serviceDir == null || serviceDir.Length == 0) return false;
            serviceDir = (new DirectoryInfo(serviceDir)).FullName.ToUpperInvariant();

            string fileExt = Helper.LocateOpenVPNServiceFileExt();
            if (fileExt == null || fileExt.Length == 0) return false;

            string confDir = Properties.Settings.Default.vpnconf;
            if (confDir == null || confDir.Length == 0) return true;
            confDir = (new DirectoryInfo(confDir)).FullName.ToUpperInvariant();

            return !(serviceDir.StartsWith(confDir, StringComparison.OrdinalIgnoreCase) &&
                fileExt.Equals("OVPN", StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// indicates wether the service key exists in the registry.
        /// </summary>
        /// <returns>true if the key exists, false otherwise.</returns>
        public static bool ServiceKeyExists()
        {
            bool exists = false;

            try
            {
                RegistryKey k = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE", false);

                if (k != null)
                {
                    RegistryKey k2 = k.OpenSubKey("OpenVPN");
                    if (k2 != null)
                    {
                        exists = true;
                        k2.Close();
                    }
                    else
                    {
                        k2 = k.OpenSubKey("Wow6432Node");
                        if (k2 != null)
                        {
                            RegistryKey k3 = k2.OpenSubKey("OpenVPN");
                            if (k3 != null)
                            {
                                exists = true;
                                k3.Close();
                            }
                            k2.Close();
                        }
                    }
                    k.Close();
                }
            }
            catch (SecurityException)
            { }

            return exists;
        }

        /// <summary>
        /// returns the file extension which is used by the OpenVPN server.
        /// </summary>
        /// <returns>the extention (without dot) or an empty string on errors</returns>
        public static string LocateOpenVPNServiceFileExt()
        {
            string ret;

            RegistryKey k = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\OpenVPN", false);
            if (k == null)
            {
                k = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Wow6432Node\OpenVPN", false);
            }
            if (k == null)
                return "ovpn";// Default value if not found

            ret = (string)k.GetValue("config_ext", "");
            k.Close();

            return ret;
        }

        public static bool UpdateSettings()
        {
            bool ret = false;
            if (Properties.Settings.Default.callUpdate)
            {
                ret = true;
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.callUpdate = false;
                Properties.Settings.Default.Save();
            }
            return ret;
        }

        public static string OpenVPNexe
        {
            get
            {
                String result = Properties.Settings.Default.vpnbin;
                if (result == null || !File.Exists(result))
                    result = UtilsHelper.LocateOpenVPN();
                return result;
            }
        }

        /// <summary>
        /// Enables autostart
        /// </summary>
        public static void InstallAutostart()
        {
            string startfile;
            startfile = System.Windows.Forms.Application.ExecutablePath;

            RegistryKey k = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (k != null)
            {
                k.SetValue("OpenVPN Manager", startfile);
                k.Close();
            }
        }

        /// <summary>
        /// Disables autostart
        /// </summary>
        public static void RemoveAutostart()
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (k != null)
            {
                k.DeleteValue("OpenVPN Manager");
                k.Close();
            }
        }

        /// <summary>
        /// determines, whether autostart is currently installed
        /// </summary>
        /// <returns>true, if autostart is set, otherwise false</returns>
        public static bool DoesAutostart()
        {
            bool ret = false;
            RegistryKey k = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            if (k != null)
            {
                ret = k.GetValue("OpenVPN Manager", null) != null;
                k.Close();
            }
            return ret;
        }
    }
}
