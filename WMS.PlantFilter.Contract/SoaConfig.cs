using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using WMS.PlantFilter.Core;

namespace WMS.PlantFilter.Contract
{
    public class SoaConfig
    {
        /// <summary>
        /// Thrift服务HOST
        /// </summary>
        public static string ThriftHost { get{ return WebConfig.GetWebConfig("ThriftHost");}}

        /// <summary>
        /// Thrift服务端口
        /// </summary>
        public static int ThriftPort { get { return int.Parse(WebConfig.GetWebConfig("ThriftPort")); } }

    }
}
