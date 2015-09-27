using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Planeta_Online.Startup))]
namespace Planeta_Online
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
