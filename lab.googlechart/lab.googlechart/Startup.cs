using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lab.googlechart.Startup))]
namespace lab.googlechart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
