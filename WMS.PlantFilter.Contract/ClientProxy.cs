using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Protocol;
using Thrift.Transport;
using System.Configuration;

namespace WMS.PlantFilter.Contract
{
    public class ClientProxy
    {
        private static string ThriftHost = "127.0.0.1";
        private static int ThriftPort = 8800;

        public static T ClientPlantExecute<T>(Func<PlantSourceService.Client, T> func)
        {
            T t = default(T);
            using (TStreamTransport tsocket = new TSocket(SoaConfig.ThriftHost, SoaConfig.ThriftPort))
            using (TTransport transport = new TBufferedTransport(tsocket))
            using (TProtocol protocol = new TBinaryProtocol(transport))
            using (var protocolSourceService = new TMultiplexedProtocol(protocol, "plantService"))
            using (var clientSource = new PlantSourceService.Client(protocolSourceService))
            {
                try
                {
                    transport.Open();
                    t = func.Invoke(clientSource);
                }
                catch (Exception ex) { throw ex; }
                finally
                {
                    transport.Close();
                }
            }
            return t;
        }

        public static T ClientRelExecute<T>(Func<RelPlantWebService.Client, T> func)
        {
            T t = default(T);
            using (TStreamTransport tsocket = new TSocket(SoaConfig.ThriftHost, SoaConfig.ThriftPort))
            using (TTransport transport = new TBufferedTransport(tsocket))
            using (TProtocol protocol = new TBinaryProtocol(transport))
            using (var protocolSourceService = new TMultiplexedProtocol(protocol, "relPlantWebService"))
            using (var clientSource = new RelPlantWebService.Client(protocolSourceService))
            {
                try
                {
                    transport.Open();
                    t = func.Invoke(clientSource);
                }
                catch (Exception ex) { throw ex; }
                finally
                {
                    transport.Close();
                }
            }
            return t;
        }

    }
}
