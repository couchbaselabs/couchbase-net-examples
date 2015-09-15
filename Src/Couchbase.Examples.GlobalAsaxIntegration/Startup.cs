using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Couchbase.Examples.GlobalAsaxIntegration.Startup))]
namespace Couchbase.Examples.GlobalAsaxIntegration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
