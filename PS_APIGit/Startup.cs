using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PS_APIGit.Startup))]
namespace PS_APIGit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
