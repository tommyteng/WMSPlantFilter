using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WMS.PlantFilter.Web.Startup))]
namespace WMS.PlantFilter.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
