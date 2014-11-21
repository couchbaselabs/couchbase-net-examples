using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    new Uri("http://192.168.56.101:8091/pools"),
                    new Uri("http://192.168.56.102:8091/pools"),
                    new Uri("http://192.168.56.103:8091/pools"),
                    new Uri("http://192.168.56.104:8091/pools"),
                },
                UseSsl = false,
                BucketConfigs = new Dictionary<string, BucketConfiguration>
                {
                    {"default", new BucketConfiguration
                    {
                        BucketName = "default",
                        UseSsl = true,
                        Password = "",
                        PoolConfiguration = new PoolConfiguration
                        {
                            MaxSize = 10,
                            MinSize = 5
                        }
                    }}
                }
            };

            using (var cluster = new Cluster(config))
            {
                IBucket bucket = null;
                try
                {
                    bucket = cluster.OpenBucket();
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
    