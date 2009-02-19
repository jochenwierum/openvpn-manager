using System;
using System.IO;

namespace OpenVPN
{
    //TODO: implement write functions
    /// <summary>
    /// This class can be used to read or write a openvpn configuration file.
    /// </summary>
    public class OVPNConfigFile
    {
        /// <summary>
        /// The configuration file to parse
        /// </summary>
        private string m_cfile;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="configfile">the file to work with</param>
        public OVPNConfigFile(string configfile)
        {
            m_cfile = configfile;
        }

        /// <summary>
        /// Reads a directive.
        /// </summary>
        /// <param name="directive">name of the directive</param>
        /// <returns>
        ///     null, if the directive is not found, otherwise an array: 
        ///     the first element is the name of the directive in lowercase,
        ///     the other (optional) elements are the parameters
        /// </returns>
        public string[] get(string directive)
        {
            // open the file
            StreamReader fsr = (new FileInfo(m_cfile)).OpenText();
            
            // read the whole file
            while(!fsr.EndOfStream) 
            {
                // read a line
                string line = fsr.ReadLine().Trim();

                // if this line is the directive we are looking for
                if(line.ToLower().StartsWith(directive.ToLower() + " ") || 
                    line.ToLower().StartsWith(directive.ToLower() + "\t") ||
                    line.ToLower().Equals(directive.ToLower()))
                {
                    // stop here
                    fsr.Close();

                    // split the directive, return it
                    string[] ret = line.Split(" \t".ToCharArray());
                    ret[0] = ret[0].ToLower();
                    return ret;
                }
            }

            // nothing was found
            fsr.Close();
            return null;
        }

        /// <summary>
        /// Checks whether a directive exists.
        /// </summary>
        /// <param name="directive">name of the directive</param>
        /// <returns>true, if it exists, false otherwise</returns>
        public bool exists(string directive)
        {
            // open the file
            StreamReader fsr = (new FileInfo(m_cfile)).OpenText();

            // read the whole file
            while (!fsr.EndOfStream)
            {
                // read a line
                string line = fsr.ReadLine();

                // if this line is the directive we are looking for
                if (line.ToLower().StartsWith(directive.ToLower() + " ") || 
                    line.ToLower().StartsWith(directive.ToLower() + "\t") ||
                    line.ToLower().Equals(directive.ToLower()))
                {

                    // we found it, return true
                    fsr.Close();
                    return true;
                }
            }

            // we did not found it
            fsr.Close();
            return false;
        }

        /// <summary>
        /// Set a directive to a value.
        /// </summary>
        /// <param name="directive">directive to set</param>
        /// <param name="values">
        ///     array of values to set,
        ///     can be empty, if the directive is a flag
        /// </param>
        public void set(string directive, string[] values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a given directive.
        /// </summary>
        /// <param name="directive">directive to delete</param>
        public void delete(string directive)
        {
            throw new NotImplementedException();
        }
    }
}