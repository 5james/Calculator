using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CalcASP.Startup))]
namespace CalcASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
