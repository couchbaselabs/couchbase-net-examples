using System;
using System.Linq;

namespace Couchbase.ClientConfigurationExample.WebConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            //Note: change the IP's in the App.Config to your own cluster's IP's to run
            using (var cluster = new Cluster("couchbaseClients/couchbase"))
            {
                using (var bucket = cluster.OpenBucket(cluster.Configuration.BucketConfigs.First().Key))
                {
                    //use the bucket here
                    Console.WriteLine($"first bucket from config: {cluster.Configuration.BucketConfigs.First().Key}");
                    Console.WriteLine($"buckets from config: {string.Join(", ", cluster.Configuration.BucketConfigs.Select(y => y.Key).ToArray())}");
                    Console.Write("Bucket '{0}' is ready for use ...", bucket.Name);
                }
            }            

            Console.Read();
        }
    }
}
