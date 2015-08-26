using System;
using System.Collections.Generic;
using System.Linq;
using Couchbase.Configuration.Client;
using Couchbase.Linq;

namespace Couchbase.Linq2CouchbaseExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //configure and initialize - you must call ClusterHelper.Initialize()!
            var config = new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://localhost:8091") }
            };
            ClusterHelper.Initialize(config);

            //Create a query
            var db = new DbContext(ClusterHelper.Get(), "beer-sample");
            var query = from b in db.Query<Beer>()
                        select b;

            //display the results
            foreach (var beer in query)
            {
                Console.WriteLine(beer.Type + ": " + beer.Name);
            }
            Console.Read();
            ClusterHelper.Close();
        }
    }
}
