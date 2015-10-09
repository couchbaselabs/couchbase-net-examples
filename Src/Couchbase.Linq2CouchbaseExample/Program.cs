using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.Linq;

namespace Couchbase.Linq2CouchbaseExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://localhost:8091") }
            };

            ClusterHelper.Initialize(config);
            var db = new BucketContext(ClusterHelper.GetBucket("beer-sample"));
            var query = from b in db.Query<Beer>()
                        select b;

            foreach (var beer in query)
            {
                Console.WriteLine(beer.Type + ": " + beer.Name);
            }
            Console.Read();
            ClusterHelper.Close();
        }
    }
}
