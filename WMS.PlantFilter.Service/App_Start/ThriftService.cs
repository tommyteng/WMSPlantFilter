using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;
using WMS.PlantFilter.Core;
using System.DirectoryServices;

namespace WMS.PlantFilter.WebServer
{
    public class ThriftService
    {
        public static void Run(PFConfig config)
        {
            using (var cts = new CancellationTokenSource())
            {
                var token = cts.Token;
                Task.Factory.StartNew(() =>
                {
                    var resolver = GlobalConfiguration.Configuration.DependencyResolver;
                    if (resolver != null)
                    {
                        using (var scope = resolver.BeginScope())
                        {
                            TServerTransport transport = null;
                            TServer server = null;
                            try
                            {
                                var plantSourceService = (PlantSourceServiceImp)scope.GetService(typeof(PlantSourceServiceImp));
                                var relPlantWebService = (RelPlantWebServiceImp)scope.GetService(typeof(RelPlantWebServiceImp));

                                var processorPlantService = new WMS.PlantFilter.Contract.PlantSourceService.Processor(plantSourceService);
                                var processorRelPlantWebService = new WMS.PlantFilter.Contract.RelPlantWebService.Processor(relPlantWebService);
                                var processorMulti = new TMultiplexedProcessor();
                                processorMulti.RegisterProcessor("plantService", processorPlantService);
                                processorMulti.RegisterProcessor("relPlantWebService", processorRelPlantWebService);

                                //设置传输协议工厂
                                TBinaryProtocol.Factory factory = new TBinaryProtocol.Factory();
                                transport = new TServerSocket(config.ThriftPort, config.ThriftTimeOut, true);
                                server = new TThreadPoolServer(processorMulti, transport, new TTransportFactory(), factory);
                                LogHelper.WriteLog("thrift服务启动并开启监听....", "ThriftServer-");
                                server.Serve();
                            }
                            catch (Exception ex)
                            {
                                if (server != null)
                                    server.Stop();

                                if (transport != null)
                                    transport.Close();

                                LogHelper.WriteLog("thrift服务停止并取消当前任务....", "ThriftServer-");

                                cts.Cancel();
                            }
                        }
                    }
                }, token);
            }
        }

        public static void HealthCheck(PFConfig config)
        {
            Task.Factory.StartNew(() => {
                while (true) {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    try
                    {
                        string uri = WebApiApplication.RootRui + "/api/health";  //config.CheckURL;
                        string result = HTTPClient.GetResponseByGet(null, uri);

                        if (result.Contains("已停止"))
                            throw new Exception(result);

                        LogHelper.WriteLog(result, "心跳检测-");
                    }
                    catch (Exception ex){
                        LogHelper.WriteLog(ex.Message, "心跳检测-");
                        Run(config);
                    }
                }
                
            });
        }
    }
}