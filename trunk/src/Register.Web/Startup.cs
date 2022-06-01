using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Register.Web.Startup))]
namespace Register.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
