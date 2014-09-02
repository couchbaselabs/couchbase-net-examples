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
            using (var cluster = new CouchbaseCluster("couchbaseClients/couchbase"))
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
