using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FetchingGlookoCode.Startup))]
namespace FetchingGlookoCode
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
