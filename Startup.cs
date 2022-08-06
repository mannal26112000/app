using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Estorenew.Startup))]
namespace Estorenew
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
