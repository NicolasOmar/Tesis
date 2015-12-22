using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TesisCon2013.Startup))]
namespace TesisCon2013
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
