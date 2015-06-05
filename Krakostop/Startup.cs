using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Krakostop.Startup))]
namespace Krakostop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
