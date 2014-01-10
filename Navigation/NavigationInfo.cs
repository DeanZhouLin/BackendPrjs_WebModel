using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Com.BaseLibrary.Configuration;
using Com.BaseLibrary.Utility;

namespace Jufine.Backend.WebModel.Navigation
{
    [Serializable]
    [XmlRoot("navigation")]
    public class NavigationConfiguration
    {
        internal const string AppSettingName = "NavigationConfigFile";
        public static NavigationConfiguration Current
        {
            get { return ConfigurationManager.LoadConfiguration<NavigationConfiguration>(ConfigurationHelper.GetConfigurationFile(AppSettingName)); }
        }
        [XmlElement("item")]
        public List<NavigationItemInfo> NavigationItemList { get; set; }
    }
    [Serializable]
    public class NavigationItemInfo
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        [XmlAttribute("Url")]
        public string Url { get; set; }

        [XmlElement("item")]
        public List<NavigationItemInfo> SubItemList { get; set; }
    }
}

