using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.PlantFilter.Core
{
    public class WebConfig
    {
        public static string GetWebConfig(string key,string defaultValue="")
        {
            if (System.Configuration.ConfigurationManager.AppSettings[key] == null)
                return defaultValue;
            else
                return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
