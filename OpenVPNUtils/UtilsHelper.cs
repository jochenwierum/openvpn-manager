using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Security;
using System.Reflection;

namespace OpenVPNUtils
{
    /// <summary>
    /// provides some static helper function
    /// </summary>
    public static class UtilsHelper
    {
        /// <summary>
        /// Describes properties required in the ovpn file
        /// </summary>
        public class ServiceConfigProperty
        {
            /// <summary>
            /// name of the property
            /// </summary>
            public string name;
            /// <summary>
            /// if the propery can be used to detect if the ovpn file is meant to be used for a service.
            /// </summary>
            public bool serviceOnly;
            /// <summary>
            /// constructor which imidiatly initializes the class variables
            /// </summary>
            public ServiceConfigProperty(String name, bool serviceOnly)
            {
                this.name = name;
                this.serviceOnly = serviceOnly;
            }
        }

        public delegate void Action();
        public delegate void Action<T1>(T1 a);
        public delegate void Action<T1, T2>(T1 a, T2 b);

        public delegate T0 Function<T0>();
        public delegate T0 Function<T0, T1>(T1 a);
        public delegate T0 Function<T0, T1, T2>(T1 a, T2 b);

        public static ServiceConfigProperty[] managementConfigItems = new ServiceConfigProperty[]{ 
            new ServiceConfigProperty("management-query-passwords",true), 
            new ServiceConfigProperty("management-hold",true), 
            new ServiceConfigProperty("management-signal",true), 
            new ServiceConfigProperty("management-forget-disconnect",true), 
            new ServiceConfigProperty("management",true), 
            new ServiceConfigProperty("auth-retry interact",false)};

        /// <summary>
        /// tries to find openvpn binary in %PATH% and in %PROGRAMS%\openvpn\bin
        /// </summary>
        /// <returns>path to openvpn.exe or null</returns>
        static public string LocateOpenVPN()
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
                if (File.Exists(pathVar))
                    return pathVar;
            }
            catch (DirectoryNotFoundException)
            {
            }

            RegistryKey openVPNkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\OpenVPN");
            if (openVPNkey == null)
                openVPNkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\OpenVPN");
            if (openVPNkey != null)
            {
                using (openVPNkey)
                {
                    String OpenVPNexe = null;
                    try
                    {
                        if (openVPNkey.GetValueKind("exe_path") == RegistryValueKind.String)
                            OpenVPNexe = (String)openVPNkey.GetValue("exe_path");

                        if (File.Exists(OpenVPNexe))
                            return OpenVPNexe;
                    }
                    catch (IOException)
                    {
                    }
                }
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
        public static void GetConfigFiles(DirectoryInfo di, List<string> dest,
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
                    GetConfigFiles(d, dest, extension, recursive);
                }
            }
        }

        /// <summary>
        /// try to locate the configuration directory of openvpn
        /// </summary>
        /// <param name="vpnbin">path where openvpn lies</param>
        /// <returns>path to configuration directory or null</returns>
        static public string LocateOpenVPNConfigDir(string vpnbin)
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
        static public List<String> LocateOpenVPNConfigs(string configdir)
        {
            List<string> files = new List<string>();
            if (configdir == null || configdir.Length == 0)
                return files;
            try
            {
                GetConfigFiles(new DirectoryInfo(configdir), files, 
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
        static public bool IsConfigForService(string config)
        {
            // if only a single management configuration item is pressent still asume it is meant for use with the service.
            ConfigParser cf = new ConfigParser(config);
            foreach (var directive in managementConfigItems)
            {
                if (directive.serviceOnly)
                    if (cf.GetValue(directive.name) != null)
                        return true;
            }
            //cf.Dispose();
            return false;
        }

        /// <summary>
        /// Returns the path used by the OpenVPNManager Service
        /// </summary>
        public static String FixedConfigDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\config";
            }
        }

        /// <summary>
        /// Returns the path used for log files by the OpenVPN processes controled by the OpenVPNManager Service
        /// </summary>
        public static String FixedLogDir
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
        public static List<String> LocateOpenVPNManagerConfigs(bool managedServices)
        {
            List<string> files = new List<string>();
            try
            {
                GetConfigFiles(
                    new DirectoryInfo(FixedConfigDir),
                    files, "ovpn", true);
            }
            catch (DirectoryNotFoundException)
            { }
            List<string> filesResult = new List<string>();
            foreach (String file in files)
            {
                if (IsConfigForService(file) == managedServices)
                    filesResult.Add(file);
            }
            return filesResult;
        }

        /// <summary>
        /// returns the directory which is used by the OpenVPN server.
        /// </summary>
        /// <returns>the directory or an empty string on errors</returns>
        public static string LocateOpenVPNServiceDir()
        {
            string ret;

            RegistryKey k = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\OpenVPN", false);
            if (k == null)
            {
                k = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Wow6432Node\OpenVPN", false);
            }
            ret = (string)k.GetValue("config_dir", "");
            k.Close();

            return ret;
        }
    }
}
