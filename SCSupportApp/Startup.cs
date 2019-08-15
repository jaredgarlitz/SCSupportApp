using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SCSupportApp.Startup))]
namespace SCSupportApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
