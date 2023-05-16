using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AEDFirst.Startup))]
namespace AEDFirst
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
