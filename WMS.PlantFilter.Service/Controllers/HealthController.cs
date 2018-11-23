using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web.Http;
using WMS.PlantFilter.Core;

namespace WMS.PlantFilter.WebServer.Controllers
{
    /// <summary>
    /// 健康检查
    /// </summary>
    public class HealthController : ApiController
    {
        private readonly PFConfig _app;
        public HealthController(PFConfig app)
        {
            this._app = app;
        }

        // GET: api/Default
        public IHttpActionResult Get()
        {
            bool isConnect = isConnectPort(this._app.ThriftPort);
            var result = isConnect ? "thrift服务正常运行!" : "thrift服务已停止！";
            return Ok(result);
        }
        #region //验证thrift服务
        private string GetIpAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();   //获取本机名
                IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
                //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
                IPAddress localaddr = localhost.AddressList[0];
                return localaddr.ToString();
            }
            catch (Exception ex)
            {
                return " ";
            }
        }
        private bool isConnectPort(int portNum)
        {
            string ipAddress = GetIpAddress();
            System.Net.IPAddress myIpAddress = IPAddress.Parse(ipAddress);
            IPEndPoint point = new IPEndPoint(myIpAddress, portNum);
            try
            {
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    sock.Connect(point);
                    return true;
                }
            }
            catch (SocketException ex)
            {
                return false;
            }
        }
        #endregion
    }
}
