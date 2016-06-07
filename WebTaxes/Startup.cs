using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTaxes.Startup))]
namespace WebTaxes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
