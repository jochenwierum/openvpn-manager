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
        /// On windows vista, autostart can not start UAC enabled applications.
        /// This Code checks, if a workaround is needed.
        /// </summary>
        /// <returns></returns>
        public static bool autostartNeedBatch()
        {
            if (System.Environment.OSVersion.Version.Major == 6)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Enables autostart
        /// </summary>
        public static void installAutostart()
        {
            string startfile;
            if (autostartNeedBatch())
            {
                /*startfile =
                    Path.GetFullPath(Path.Combine(
                    System.Windows.Forms.Application.ExecutablePath,
                    String.Join(Path.DirectorySeparatorChar.ToString(), 
                    new String[]{"..", "autostart.bat"})));

                StreamWriter sw = (new FileInfo(startfile)).CreateText();
                sw.WriteLine("@echo off");
                sw.WriteLine("rem This file is generated automatically");
                sw.WriteLine("rem Don't change it, changes can be lost very quickly");
                sw.WriteLine("start \"\" /MIN \"" + System.Windows.Forms.Application.ExecutablePath + "\"");
                sw.WriteLine("@echo on");
                sw.Close();*/

                // CRAZY DIRTY HACK!
                // The Program can not be run directly, because Vista does not start
                // programs as admin, so we start a batch-file which starts the software.
                // To keep the command-window minimized (hiding it is impossible) we start
                // it with start and a second cmd using /MIN. Sorry, but it works :-)
                /*startfile = "cmd /C start \"OpenVPN Manager\" /MIN \"cmd /C start " +
                    "\"\"OpenVPN Manager\"\" \"\"" +
                    System.Windows.Forms.Application.ExecutablePath + "\"\"\"";*/


                startfile = Path.Combine(Path.GetDirectoryName(
                    System.Windows.Forms.Application.ExecutablePath),
                    "OpenVPNStarter.exe");
            }
            else
            {
                startfile = System.Windows.Forms.Application.ExecutablePath;
            }

            RegistryKey k = Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            k.SetValue("OpenVPN Manager", startfile.ToString());
            k.Close();
        }

        /// <summary>
        /// Disables autostart
        /// </summary>
        public static void removeAutostart()
        {
            /*string batfile =
                Path.GetFullPath(Path.Combine(
                System.Windows.Forms.Application.ExecutablePath,
                String.Join(Path.PathSeparator.ToString(),
                new String[] { "..", "autostart.bat" })));

            FileInfo fi = new FileInfo(batfile);
            if (fi.Exists)
                fi.Delete();*/


            RegistryKey k = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
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
