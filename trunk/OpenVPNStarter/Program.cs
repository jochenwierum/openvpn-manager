using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace OpenVPNStarter
{
    static class Program
    {
        /// <summary>
        /// This Program just runs as a normal user and starts OpenVPN Manager.
        /// This Software is needed in Vista if you want to use the Autostart function.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = Path.GetFullPath(Path.Combine(
                System.Windows.Forms.Application.ExecutablePath,
                ".." + Path.DirectorySeparatorChar + "OpenVPNManager.exe"));

            psi.UseShellExecute = true;

            try
            {
                Process.Start(psi);
            }
            catch (Win32Exception)
            {
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
