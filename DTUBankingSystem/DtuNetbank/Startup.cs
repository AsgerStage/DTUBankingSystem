using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DtuNetbank.Startup))]
namespace DtuNetbank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
