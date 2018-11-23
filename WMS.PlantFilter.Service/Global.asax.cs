using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using WMS.PlantFilter.Core;
using WMS.PlantFilter.Data;
using System.Data;
using System.Data.OracleClient;
using WMS.PlantFilter.IRepository;
using WMS.PlantFilter.Repository;
using WMS.PlantFilter.Interceptor;
using Autofac.Extras.DynamicProxy;
using System.Data.OleDb;
using System.Configuration;
using WMS.PlantFilter.Core.Fakes;
using WMS.PlantFilter.Core.Caching;
using Autofac.Core;

namespace WMS.PlantFilter.WebServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = ConfigurationManager.GetSection("PFConfig") as PFConfig;
            MapperConfig.RegisterMappings();
            var container = CreateContainer(config);
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ThriftService.Run(config);
            ThriftService.HealthCheck(config);
            
        }

        private IContainer CreateContainer(PFConfig config)
        {
            var builder = new ContainerBuilder();

            builder.Register(c =>
             HttpContext.Current != null ? (new HttpContextWrapper(HttpContext.Current) as HttpContextBase)
             : (new FakeHttpContext("~/") as HttpContextBase))
             .As<HttpContextBase>().InstancePerLifetimeScope();

            builder.RegisterType<TransactionInterceptor>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly())
                 .WithParameter(new TypedParameter(typeof(PFConfig), config));

            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>()
                    .WithParameter(new TypedParameter(typeof(PFConfig), config))
                    .SingleInstance();

                builder.RegisterType<RedisCacheManager>().As<ICacheManager>()
                    .Named<ICacheManager>("nop_cache_static")
                    .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_per_request"))
                    .InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<MemoryCacheManager>().As<ICacheManager>()
                    .Named<ICacheManager>("nop_cache_static").SingleInstance();
            }

            builder.RegisterType<PerRequestCacheManager>().As<ICacheManager>()
                .Named<ICacheManager>("nop_cache_per_request").InstancePerLifetimeScope();


            if (config.DataBase == "Oracel")
                builder.RegisterType<OracleConnection>().As<IDbConnection>();
            else
                builder.RegisterType<System.Data.SqlClient.SqlConnection>().As<IDbConnection>();

            builder.RegisterType<PlantSourceRepository>().As<IPlantSourceRepository>();
            builder.RegisterType<RelPlantWebRepository>().As<IRelPlantWebRepository>();
            builder.RegisterType<DapperPlusDB>().As<IDapperPlusDB>().WithParameter(new TypedParameter(typeof(PFConfig), config));

            builder.RegisterType<PlantSourceServiceImp>().AsSelf()
                 .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                 .SingleInstance();

            builder.RegisterType<RelPlantWebServiceImp>().AsSelf()
                 .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"))
                .SingleInstance()
                .InterceptedBy(typeof(TransactionInterceptor))
                .EnableClassInterceptors(); 
            
            return builder.Build();
        }

        static bool IsFirst = false;
        public static string RootRui = "";
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!IsFirst)
            {
                IsFirst = true;
                var req = HttpContext.Current.Request;
                var url = req.Url;
                var port = url.Port;
                RootRui = GetRootURI();
                IsFirst = true;
            }
        }
        public static string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;

                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在 Web 站点 
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下 
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        } 
    }
}
