using Microsoft.Owin;

using OrdersPortal.WebUI;

using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace OrdersPortal.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
