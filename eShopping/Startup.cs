using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eShopping.Startup))]
namespace eShopping
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
