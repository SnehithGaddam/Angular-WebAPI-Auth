using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AngularWebApiAuthExample.WebApis.Startup))]

namespace AngularWebApiAuthExample.WebApis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
