using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core;

namespace Couchbase.IncrementExample
{
    class Program
    {
        static Cluster _cluster = new Cluster();

        static void Main(string[] args)
        {
            using (var bucket = _cluster.OpenBucket())
            {
                var key = "stats::counter1";
                Increment(bucket, key);
                Increment(bucket, key);
                Decrement(bucket, key);
                Decrement(bucket, key);
                Decrement(bucket, key);
            }
            _cluster.Dispose();
            Console.Read();
        }

        static void Increment(IBucket bucket, string key)
        {
            var result = bucket.Increment(key);
            if (result.Success)
            {
                Console.WriteLine(result.Value);
            }
        }

        static void Decrement(IBucket bucket, string key)
        {
            var result = bucket.Decrement(key);
            if (result.Success)
            {
                Console.WriteLine(result.Value);
            }
        }
    }
}
