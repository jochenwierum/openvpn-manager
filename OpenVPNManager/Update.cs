using System;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace OpenVPNManager
{
    /// <summary>
    /// provides information if an update is available and where to get it
    /// </summary>
    class Update
    {
        /// <summary>
        /// XML Node which represents the update information for this program
        /// </summary>
        private XmlNode n;

        /// <summary>
        /// Parent form for messageboxes.
        /// </summary>
        private Form m_parent;

        /// <summary>
        /// loads update information, may show errors in dialog boxes
        /// </summary>
        /// <param name="silence">don't show message boxes on error</param>
        public Update(bool silence, Form parent)
        {
            refresh(silence);
            m_parent = parent;
        }

        /// <summary>
        /// refreshs the XML data, may reset hadErrors
        /// </summary>
        /// <param name="silence">don't show message boxes on error</param>
        public void refresh(bool silence)
        {
            XmlDocument doc;

            // TODO: Check what happens if we are not connected
            try
            {
                doc = new XmlDocument();
                doc.Load(XmlReader.Create("http://openvpn.jowisoftware.de/versions.xml"));
            }
            catch (WebException e)
            {
                if(!silence)
                RTLMessageBox.Show(m_parent, Program.res.GetString(
                    "BOX_UpdateError") + ": " + e.Message,
                    MessageBoxIcon.Error);
                return;
            }
            catch (XmlException e)
            {
                if (!silence)
                RTLMessageBox.Show(m_parent, Program.res.GetString(
                    "BOX_UpdateFormat") + ": " + e.Message,
                    MessageBoxIcon.Error);
                return;
            }

            n = doc.DocumentElement.SelectSingleNode("application[@name='OpenVPN Manager']");
        }

        /// <summary>
        /// true, if the constructor could not load and parse the xml document
        /// </summary>
        public bool hadErrors
        {
            get { return n == null; }
        }

        /// <summary>
        /// looks for updates; if an update is available, true is returned
        /// </summary>
        /// <returns>true if an update is available, false otherwise</returns>
        public bool checkUpdate()
        {
            string v = getVersion();

            if(v == null)
                return false;

            return isNewerVersion(v);
        }

        /// <summary>
        /// returns the url to the update page
        /// </summary>
        /// <returns>the url to the update page</returns>
        public string getUpdateUrl()
        {
            if (n == null)
                return null;

            string url;

            try
            {
                url = n.SelectSingleNode("child::url").InnerText;
            }
            catch (NullReferenceException)
            {
                RTLMessageBox.Show(m_parent,
                    Program.res.GetString("BOX_UpdateMissing"),
                    MessageBoxIcon.Error);
                return null;
            }

            return url;
        }

        /*/// <summary>
        /// returns the url to the update file itself
        /// </summary>
        /// <returns>the url to the update file itself</returns>
        public string getDownloadFileUrl()
        {
            if (n == null)
                return null;

            string url;

            try
            {
                url = n.SelectSingleNode("child::download").InnerText;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(Program.res.GetString("BOX_UpdateMissing"),
                    "OpenVPN Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return url;
        }*/

        /// <summary>
        /// returns the version of the update
        /// </summary>
        /// <returns>version of the update</returns>
        public string getVersion()
        {
            if (n == null)
                return null;

            try
            {
                return n.SelectSingleNode("child::version").InnerText;
            }
            catch (NullReferenceException)
            {
                RTLMessageBox.Show(m_parent,
                    Program.res.GetString("BOX_UpdateMissing"),
                    MessageBoxIcon.Error);
                return null;
            }
        }
        
        /// <summary>
        /// compares a given version with the present one
        /// </summary>
        /// <param name="version">version to compare</param>
        /// <returns>true, if given version is newer than this one</returns>
        private static bool isNewerVersion(string version)
        {
            string[] v1parts = Application.ProductVersion.Split(new char[] { '.' });
            string[] v2parts = version.Split(new char[] { '.' });

            int[] v1 = new int[v1parts.GetUpperBound(0)+1];
            int[] v2 = new int[v2parts.GetUpperBound(0)+1];

            for(int i = 0; i <= v2parts.GetUpperBound(0); ++i)
                if(!int.TryParse(v2parts[i], out v2[i]))
                    return false;

            for (int i = 0; i <= v1parts.GetUpperBound(0); ++i)
                if (!int.TryParse(v1parts[i], out v1[i]))
                    return false;

            for (int i = 0; i <=
                Math.Min(v1.GetUpperBound(0), v2.GetUpperBound(0)); ++i)
            {
                if (v2[i] > v1[i])
                    return true;
                else if (v1[i] > v2[i])
                    return false;
            }

            return false;
        }
    }
}
