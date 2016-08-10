using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lab.ngdemo.postgresql.Startup))]
namespace lab.ngdemo.postgresql
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
