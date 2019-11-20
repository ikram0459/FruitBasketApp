using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FruitBasketTestApp.Startup))]
namespace FruitBasketTestApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
