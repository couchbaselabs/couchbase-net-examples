using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Couchbase.ClientConfigurationExample.WebConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            //Note: change the IP's in the App.Config to your own cluster's IP's to run
            using (var cluster = new Cluster("couchbaseClients/couchbase"))
            {
                using (var bucket = cluster.OpenBucket())
                {
                    //use the bucket here
                }
            }
            Console.Read();
        }
    }
}
