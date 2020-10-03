using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NeomSoft.Startup))]
namespace NeomSoft
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
