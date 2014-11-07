using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.Core;

namespace Couchbase.SslExample
{
    class Program
    {
        private static Cluster _cluster;
        static void Main(string[] args)
        {
            var config = new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    new Uri("http://192.168.56.101:8091/pools")
                }
            };

            const int numOperations = 100;
            var opCount = 0;

            using (_cluster = new Cluster(config))
            {
                using (var bucket = _cluster.OpenBucket())
                {
                    Console.WriteLine(bucket.IsSecure ? "SSL enabled" : "SSL disabled");
                    SynchronousInsert(bucket, numOperations);

                    /*while (opCount++ < numOperations)
                    {
                        int count = opCount;
                        var key = "Key" + count;
                        var document = new Document<int>
                        {
                            Id = "Key" + count,
                            Value = count
                        };
                        var result = bucket.Upsert(key, count);

                        Console.WriteLine("Key {0} was stored: {1} -[{2}]", 
                            key, 
                            result.Status, 
                            Thread.CurrentThread.ManagedThreadId);
                    }*/

                    Console.Read();
                }
            } 
        }

        static void SynchronousInsert(IBucket bucket, int n)
        {
            for (int i = 0; i < n; i++)
            {
                var key = "key" + i;
                var value = "value" + i;

                var result = bucket.Upsert(key, value);
                if (result.Success)
                {
                    Console.WriteLine("Write Key: {0} - Value: {1}", key, value);
                    var result2 = bucket.Get<string>(key);
                    if (result2.Success)
                    {
                        Console.WriteLine("Read Key: {0} - Value: {1}", key, result2.Value);
                    }
                    else
                    {
                        Console.WriteLine("Read Error: {0} - {1}", key, result2.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Write Error: {0} - {1}", key, result.Message);
                }
            }
        }
    }
}
