using System;
using Couchbase.Core;
using Newtonsoft.Json;

namespace Couchbase.ViewExamples
{
    class Program
    {
        static Cluster _cluster = new Cluster();

        static void Main(string[] args)
        {
            using (var bucket = _cluster.OpenBucket("beer-sample"))
            {
                BasicQuery(bucket);
            }
            _cluster.Dispose();
            Console.Read();
        }

        static void BasicQuery(IBucket bucket)
        {
            var query = bucket.CreateQuery("beer", "brewery_beers").
                Limit(5);

            var result = bucket.Query<dynamic>(query);
            foreach (var row in result.Rows)
            {
                Console.WriteLine(JsonConvert.SerializeObject(row));
            }
        }
    }
}
