using System;
using System.Collections.Generic;
using Couchbase.Configuration.Client;
using Couchbase.Core;

namespace Couchbase.ClientConfigurationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Note: change the IP's below to your own cluster's IP's to run
            var config = new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    new Uri("http://localhost:8091/"),
                },
                UseSsl = false,
                
                BucketConfigs = new Dictionary<string, BucketConfiguration>
                {
                    {"default", new BucketConfiguration
                    {
                        UseSsl = false,
                        Password = "",
                        PoolConfiguration = new PoolConfiguration
                        {
                            MaxSize = 10,
                            MinSize = 5
                        },
                        
                    }}
                }
            };

            using (var cluster = new Cluster(config))
            {            
                IBucket bucket = null;
                try
                {
                    bucket = cluster.OpenBucket("default");
                    //use the bucket here
                }
                finally
                {
                    if (bucket != null)
                    {
                        cluster.CloseBucket(bucket);
                    }
                }
            }
        }
    }
}
