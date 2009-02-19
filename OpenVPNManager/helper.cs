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
        static private void getConfigFiles(DirectoryInfo di, List<string> dest) 
        {
            // add all files
            FileInfo[] files = di.GetFiles("*.ovpn");
            foreach (FileInfo fi in files)
                dest.Add(fi.FullName);

            // search in subdirectories
            foreach(DirectoryInfo d in di.GetDirectories())
                getConfigFiles(d, dest);
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
                getConfigFiles(new DirectoryInfo(configdir), files);
            }
            catch (DirectoryNotFoundException)
            {
            }
            return files.ToArray();
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
        {            RegistryKey k = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            k.DeleteValue("OpenVPN Manager");
            k.Close();
        }

        /// <summary>
        /// determines, whether autostart is currently installed
        /// </summary>
        /// <returns>true, if autostart is set, otherwise false</returns>
        public static bool doesAutostart()
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            bool ret = k.GetValue("OpenVPN Manager",null) != null;
            k.Close();

            return ret;
        }
    }
}
