using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuickSearchWeb.Startup))]
namespace QuickSearchWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }


    }
}
