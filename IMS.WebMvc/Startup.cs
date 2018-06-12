using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IMS.WebMvc.Startup))]
namespace IMS.WebMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
