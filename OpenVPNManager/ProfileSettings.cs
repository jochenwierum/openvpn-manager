using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace OpenVPNManager
{
    [Serializable]
    public class ProfileSettings
    {
        private ProfileSettings()
        {
        }
        public ProfileSettings(String profileName)
        {
            this.profileName = profileName;
        }
        public String profileName;
        private bool _storeUserName;
        private String _userName;
        public bool storeUserName
        {
            get
            {
                return _storeUserName;
            }
            set
            {
                if (storeUserName != value)
                {
                    _storeUserName = value;
                    Settings.current.changed = true;
                }
                if (_storeUserName == false)
                    userName = "";
            }
        }
        public String userName{
            get{
                return _userName == null ? "" : _userName;
            }
            set{
                if (userName != value)
                {
                    _userName = value;
                    Settings.current.changed = true;
                }
            }
        }
    }

    [Serializable]
    public class OpenVPNManagerSettings
    {
        public List<ProfileSettings> profiles;
    }

    class Settings
    {
        private static Settings _settings;//singleton
        public bool changed;// set to True if the settings need re-saving to file.
        private Settings()
        {
            //constructor without parameters required for de-serialization
        }

        static public Settings current{
            get{
                if (_settings == null)
                {
                    _settings = new Settings();
                    _settings.Load();
                }
                return _settings;
            }
        }

        Serializer s = new Serializer();
        OpenVPNManagerSettings profilesSettings = null;
        String settingsFile
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\JoWiSoftware\\OpenVPNManagerSettings.xml"; }
        }
        void Load()
        {
            if (File.Exists(settingsFile))
            {
                profilesSettings = s.DeSerializeObject(settingsFile);
            }
            else
                profilesSettings = new OpenVPNManagerSettings();
        }
        public void Save()
        {
            if (changed)
            {
                s.SerializeObject(settingsFile, profilesSettings);
                changed = false;
            }
        }

        public ProfileSettings getProfile(string config)
        {
            if (profilesSettings.profiles == null)
                profilesSettings.profiles = new List<ProfileSettings>();
            foreach (var profile in profilesSettings.profiles)
            {
                if (profile.profileName == config)
                    return profile;
            }
            ProfileSettings settings = new ProfileSettings(config);
            profilesSettings.profiles.Add(settings);
            return settings;
        }
    }

    public class Serializer
    {
        public void SerializeObject(string filename, OpenVPNManagerSettings objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            XmlSerializer x = new XmlSerializer(objectToSerialize.GetType());
            x.Serialize(stream, objectToSerialize);
            stream.Close();            
        }

        public OpenVPNManagerSettings DeSerializeObject(string filename)
        {
            OpenVPNManagerSettings objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            XmlSerializer x = new XmlSerializer(typeof(OpenVPNManagerSettings));
            objectToSerialize = (OpenVPNManagerSettings)x.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}
