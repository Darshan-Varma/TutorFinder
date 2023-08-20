using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tutor_Finder.Startup))]
namespace Tutor_Finder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
