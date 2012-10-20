using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Security;
using OpenVPN;
using System.Reflection;

namespace OpenVPNManager
{
    /// <summary>
    /// provides some static helper function
    /// </summary>
    static class helper
    {
        public delegate void Action();
        public delegate void Action<T1>(T1 a);
        public delegate void Action<T1, T2>(T1 a, T2 b);

        public delegate T0 Function<T0>();
        public delegate T0 Function<T0, T1>(T1 a);
        public delegate T0 Function<T0, T1, T2>(T1 a, T2 b);

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
        static public List<String> locateOpenVPNConfigs(string configdir)
        {
            List<string> files = new List<string>();
            if (configdir == null || configdir.Length == 0)
                return files;
            try
            {
                getConfigFiles(new DirectoryInfo(configdir), files, 
                    "ovpn", true);
            }
            catch (DirectoryNotFoundException)
            { }
            return files;
        }

        /// <summary>
        /// check if the configuration file is to be used with a management console
        /// </summary>
        /// <param name="config">the config filename</param>
        /// <returns>returns if the config file is to be used as as service</returns>
        static public bool isConfigForService(string config)
        {
            // if only a single management configuration item is pressent still asume it is meant for use with the service.
            ConfigParser cf = new ConfigParser(config);
            foreach (var directive in OpenVPN.ServiceConnection.managementConfigItems)
            {
                if (directive.serviceOnly)
                    if (cf.GetValue(directive.name) != null)
                        return true;
            }
            //cf.Dispose();
            return false;
        }

        /// <summary>
        /// Returns a list of files which are used by the OpenVPN Service.
        /// </summary>
        /// <returns></returns>
        public static List<string> locateOpenVPNServiceConfigs()
        {
            List<string> files = new List<string>();
            if (!canUseService())
                return files;

            try
            {
                getConfigFiles(
                    new DirectoryInfo(helper.locateOpenVPNServiceDir()),
                    files, helper.locateOpenVPNServiceFileExt(), false);
            }
            catch (DirectoryNotFoundException)
            { }
            return files;
        }

        /// <summary>
        /// Returns the path used by the OpenVPNManager Service
        /// </summary>
        public static String fixedConfigDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\config";
            }
        }

        /// <summary>
        /// Returns the path used for log files by the OpenVPN processes controled by the OpenVPNManager Service
        /// </summary>
        public static String fixedLogDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\log";
            }
        }

        /// <summary>
        /// Returns a list of files which are used by the OpenVPNManager Service.
        /// </summary>
        /// <returns></returns>
        public static List<String> locateOpenVPNManagerConfigs(bool managedServices)
        {
            List<string> files = new List<string>();
            try
            {
                getConfigFiles(
                    new DirectoryInfo(fixedConfigDir),
                    files, "ovpn", true);
            }
            catch (DirectoryNotFoundException)
            { }
            List<string> filesResult = new List<string>();
            foreach (String file in files)
            {
                if (helper.isConfigForService(file) == managedServices)
                    filesResult.Add(file);
            }
            return filesResult;
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

            string serviceDir = helper.locateOpenVPNServiceDir();
            if (serviceDir == null || serviceDir.Length == 0) return false;
            serviceDir = (new DirectoryInfo(serviceDir)).FullName.ToUpperInvariant();

            string fileExt = helper.locateOpenVPNServiceFileExt();
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
        public static bool serviceKeyExists()
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
        /// returns the directory which is used by the OpenVPN server.
        /// </summary>
        /// <returns>the directory or an empty string on errors</returns>
        public static string locateOpenVPNServiceDir()
        {
            string ret;

            RegistryKey k = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\OpenVPN", false);
            if (k == null)
            {
                k = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Wow6432Node\OpenVPN", false);
            }
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

        /// <summary>
        /// Enables autostart
        /// </summary>
        public static void installAutostart()
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
        public static void removeAutostart()
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
        public static bool doesAutostart()
        {
            bool ret = false;
            RegistryKey k = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            if(k != null)
            {
                ret = k.GetValue("OpenVPN Manager", null) != null;
                k.Close();
            }
            return ret;
        }

        internal static bool UpdateSettings()
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
    }
}
