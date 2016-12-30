using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace OpenVPNUtils
{
    // TODO: implement write functions
    /// <summary>
    /// This class can be used to read or write a openvpn configuration file.
    /// </summary>
    public class ConfigParser
    {
        /// <summary>
        /// The configuration file to parse
        /// </summary>
        private string m_cfile;

        private Dictionary<string, string[]> m_directives; 

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="configfile">the file to work with</param>
        public ConfigParser(string configfile)
        {
            m_cfile = configfile;
        }

        public static Dictionary<string, string[]> ParseConfigFile(string file)
        {
            var directiveRegex = new Regex(@"(^|(?<=\r?\n))[ \t]*(?:(?<directive>[^#;<\s]\S+)(?:[ \t]+(?:""(?<args>(?:\\.|[^\r\n""])*)""|(?<args>[^""\s][^\s]*)))*|<(?<tag>[^>\n\r]+)>\s*(?<tagcontent>[^<]*)</\k<tag>>)[\t ]*(?:$|(?=\r?\n))", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var directives = new Dictionary<string, string[]>();

            string fileContent = null;
            using (var fs = new FileInfo(file).OpenText())
            {
                fileContent = fs.ReadToEnd();
            }

            var matches = directiveRegex.Matches(fileContent);
            var args = new List<string>();
            foreach (Match match in matches)
            {
                if (!string.IsNullOrEmpty(match.Groups["directive"].Value))
                {
                    var key = match.Groups["directive"].Value.ToLower();
                    args.Add(key);
                    foreach (Capture capture in match.Groups["args"].Captures)
                    {
                        args.Add(capture.Value);
                    }

                    directives.Add(key, args.ToArray());
                    args.Clear();
                }
                else if (!string.IsNullOrEmpty(match.Groups["tag"].Value))
                {
                    var key = $"<{match.Groups["tag"].Value.ToLower()}>";
                    directives.Add(key, new [] { key, match.Groups["tagcontent"].Value });
                }
            }

            return directives;
        }

        /// <summary>
        /// Reads a directive.
        /// </summary>
        /// <param name="directive">name of the directive</param>
        /// <returns>
        ///     null, if the directive is not found, otherwise an array: 
        ///     the first element is the name of the directive in uppercase,
        ///     the other (optional) elements are the parameters
        /// </returns>
        public string[] GetValue(string directive)
        {
            if (m_directives == null)
                m_directives = ParseConfigFile(m_cfile);

            string[] result;
            m_directives.TryGetValue(directive, out result);
            return result;
        }

        /// <summary>
        /// Checks whether a directive exists.
        /// </summary>
        /// <param name="directive">name of the directive</param>
        /// <returns>true, if it exists, false otherwise</returns>
        public bool DirectiveExists(string directive)
        {
            if (m_directives == null)
                m_directives = ParseConfigFile(m_cfile);

            return m_directives.ContainsKey(directive);
        }
    }
}