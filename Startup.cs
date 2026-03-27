using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Genii_Assessment.Startup))]
namespace Genii_Assessment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
