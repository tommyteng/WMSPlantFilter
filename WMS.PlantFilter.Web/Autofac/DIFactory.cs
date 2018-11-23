using Autofac;
using System.Linq;
using System.Reflection;
using Autofac.Integration.Mvc;

namespace WMS.PlantFilter.Web
{
    public class DIFactory
    {
        public static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
           
            var assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            builder.RegisterControllers(assemblies);

            return builder.Build();
        }
    }
}