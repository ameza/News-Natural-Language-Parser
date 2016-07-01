using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsParserParadigmas.Startup))]
namespace NewsParserParadigmas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
