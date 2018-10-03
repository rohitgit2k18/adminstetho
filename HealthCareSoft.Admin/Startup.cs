using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HealthCareSoft.Admin.Startup))]
namespace HealthCareSoft.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
