using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HealthCareSoft.WebPortal.Startup))]
namespace HealthCareSoft.WebPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
