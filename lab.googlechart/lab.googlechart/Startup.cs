using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lab.ngdemo.Startup))]
namespace lab.ngdemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
