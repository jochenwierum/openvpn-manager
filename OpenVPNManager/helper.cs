using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace OpenVPNManager
{
    /// <summary>
    /// provides some static helper function
    /// </summary>
    static class helper
    {
        /// <summary>
        /// tries to find openvpn binary in %PATH% and in %PROGRAMS%\openvpn\bin
        /// </summary>
        /// <returns>path to openvpn.exe or null</returns>
        static public string locateOpenVPN()
        {
            // split %path%
            string pathVar = System.Environment.GetEnvironmentVariable("PATH");
            string[] path = pathVar.Split(new Char[] { Path.PathSeparator });

            // search openvpn in each path
            foreach (string p in path)
            {
                string pa = Path.Combine(p, "openvpn.exe");
                try
                {
                    if ((new FileInfo(pa)).Exists)
                        return pa;
                }
                catch(DirectoryNotFoundException)
                {
                }
            }

            // search openvpn in program files
            pathVar = Path.Combine(System.Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles),
                "openvpn" + Path.DirectorySeparatorChar + "bin" + 
                Path.DirectorySeparatorChar + "openvpn.exe");

            try
            {
                if ((new FileInfo(pathVar)).Exists)
                    return pathVar;
            }
            catch (DirectoryNotFoundException)
            {
            }

            // it was not found, return
            return null;
        }

        /// <summary>
        /// search all config files in a specific directory and all subdirectories
        /// </summary>
        /// <param name="di">start directory</param>
        /// <param name="dest">list to save results</param>
        /// <param name="extension">file extension</param>
        static private void getConfigFiles(DirectoryInfo di, List<string> dest,
            string extension, bool recursive)
        {
            // add all files
            FileInfo[] files = di.GetFiles("*." + extension);
            foreach (FileInfo fi in files)
            {
                dest.Add(fi.FullName);
            }

            if (recursive)
            {
                foreach (DirectoryInfo d in di.GetDirectories())
                {
                    getConfigFiles(d, dest, extension, recursive);
                }
            }
        }

        /// <summary>
        /// try to locate the configuration directory of openvpn
        /// </summary>
        /// <param name="vpnbin">path where openvpn lies</param>
        /// <returns>path to configuration directory or null</returns>
        static public string locateOpenVPNConfigDir(string vpnbin)
        {
            string p = Path.GetFullPath(Path.Combine(vpnbin,
                string.Join(Path.DirectorySeparatorChar.ToString(),
                new string[] { "..", "..", "config"})));

            try
            {
                if ((new DirectoryInfo(p)).Exists)
                    return p;
            }
            catch (DirectoryNotFoundException)
            {
            }

            return null;
        }

        /// <summary>
        /// find all configuration files in a specific directory
        /// </summary>
        /// <param name="configdir">the directory</param>
        /// <returns>list of configuration files or null</returns>
        static public string[] locateOpenVPNConfigs(string configdir)
        {
            if (configdir == null || configdir.Length == 0)
                return null;
            List<string> files = new List<string>();
            try
            {
                getConfigFiles(new DirectoryInfo(configdir), files, 
                    "ovpn", true);
            }
            catch (DirectoryNotFoundException)
            { }
            return files.ToArray();
        }

        /// <summary>
        /// Returns a list of files which are used by the OpenVPN Service.
        /// </summary>
        /// <returns></returns>
        public static string[] locateOpenVPNServiceConfigs()
        {
            if (!canUseService())
                return null;

            List<string> files = new List<string>();
            try
            {
                getConfigFiles(
                    new DirectoryInfo(helper.locateOpenVPNServiceDir()),
                    files, helper.locateOpenVPNServiceFileExt(), false);
            }
            catch (DirectoryNotFoundException)
            { }
            return files.ToArray();
        }


        /// <summary>
        /// Indicates whether the OpenVPN Service can be used.
        /// </summary>
        /// <returns>true, if the OpenVPN Service can be used, false otherwise.</returns>
        public static bool canUseService()
        {
            if(!helper.serviceKeyExists())
                return false;

            // Config directory AND file extension are the same
            return !((new DirectoryInfo(helper.locateOpenVPNServiceDir()))
                .FullName.ToLower().Equals(
                (new DirectoryInfo(Properties.Settings.Default.vpnconf))
                .FullName.ToLower()) &&
                helper.locateOpenVPNServiceFileExt().ToLower().Equals("ovpn"));
        }

        /// <summary>
        /// indicates wether the service key exists in the registry.
        /// </summary>
        /// <returns>true if the key exists, false otherwise.</returns>
        public static bool serviceKeyExists()
        {
            bool exists = false;

            RegistryKey k = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE", false);
            RegistryKey k2 = k.OpenSubKey("OpenVPN");

            if (k2 != null)
            {
                exists = true;
                k2.Close();
            }
            k.Close();

            return exists;
        }

        /// <summary>
        /// returns the directory which is used by the OpenVPN server.
        /// </summary>
        /// <returns>the directory or an empty string on errors</returns>
        public static string locateOpenVPNServiceDir()
        {
            string ret;

            RegistryKey k = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\OpenVPN", false);
            ret = (string) k.GetValue("config_dir", "");
            k.Close();

            return ret;
        }

        /// <summary>
        /// returns the file extension which is used by the OpenVPN server.
        /// </summary>
        /// <returns>the extention (without dot) or an empty string on errors</returns>
        public static string locateOpenVPNServiceFileExt()
        {
            string ret;

            RegistryKey k = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\OpenVPN", false);
            ret = (string) k.GetValue("config_ext", "");
            k.Close();

            return ret;
        }

        /// <summary>
        /// Enables autostart
        /// </summary>
        public static void installAutostart()
        {
            string startfile;
            startfile = System.Windows.Forms.Application.ExecutablePath;

            RegistryKey k = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            k.SetValue("OpenVPN Manager", startfile);
            k.Close();
        }

        /// <summary>
        /// Disables autostart
        /// </summary>
        public static void removeAutostart()
        {            
            RegistryKey k = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            k.DeleteValue("OpenVPN Manager");
            k.Close();
        }

        /// <summary>
        /// determines, whether autostart is currently installed
        /// </summary>
        /// <returns>true, if autostart is set, otherwise false</returns>
        public static bool doesAutostart()
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            bool ret = k.GetValue("OpenVPN Manager", null) != null;
            k.Close();

            return ret;
        }
    }
}
