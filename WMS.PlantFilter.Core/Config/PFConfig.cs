using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace WMS.PlantFilter.Core
{
    public class PFConfig : IConfigurationSectionHandler
    {
        public bool RedisCachingEnabled { get; private set; }
        public string RedisCachingConnectionString { get; private set; }
        public int ThriftPort { get; private set; }
        public int ThriftTimeOut { get; private set; }
        public string DataBase { get; private set; }
        public string DBConnection { get; private set; }
        public string LogPath { get; private set; }


        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            var config = new PFConfig();

            var redisCachingNode = section.SelectSingleNode("RedisCaching");
            config.RedisCachingEnabled = GetBool(redisCachingNode, "Enabled");
            config.RedisCachingConnectionString = GetString(redisCachingNode, "ConnectionString");

            var ThriftServerNode = section.SelectSingleNode("ThriftServer");
            config.ThriftPort = GetInt(ThriftServerNode, "ThriftPort");
            config.ThriftTimeOut = GetInt(ThriftServerNode, "ThriftTimeOut");

            var LogNode = section.SelectSingleNode("Log");
            config.LogPath = GetString(LogNode, "LogPath");

            var DBNode = section.SelectSingleNode("DB");
            config.DataBase = GetString(DBNode, "DataBase");
            config.DBConnection = GetString(DBNode, "DBConnection");

            return config;
        }
        private int GetInt(XmlNode node, string attrName)
        {
            return SetByXElement<int>(node, attrName, Convert.ToInt32);
        }


        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node == null || node.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }
    }
}
