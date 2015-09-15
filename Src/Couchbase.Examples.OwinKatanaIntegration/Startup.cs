using System;
using System.Collections.Generic;
using System.Threading;
using Couchbase.Configuration.Client;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using Owin;

[assembly: OwinStartupAttribute(typeof(Couchbase.Examples.OwinKatanaIntegration.Startup))]
namespace Couchbase.Examples.OwinKatanaIntegration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //initialize the ClusterHelper
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    new Uri("http://localhost:8091/")
                }
            });

            //Register a callback that will dispose of the ClusterHelper on app shutdown
            var properties = new AppProperties(app.Properties);
            var token = properties.OnAppDisposing;
            if (token != CancellationToken.None)
            {
                token.Register(() =>
                {
                    ClusterHelper.Close();
                });
            }
        }
    }
}
