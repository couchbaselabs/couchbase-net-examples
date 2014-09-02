using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Core;

namespace Couchbase.ViewExamples
{
    class Program
    {
        static CouchbaseCluster _cluster = new CouchbaseCluster();

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
            var query = bucket.CreateQuery(false).
                DesignDoc("beer").
                View("brewery_beers").
                Limit(5);

            var result = bucket.Query<dynamic>(query);
            foreach (var row in result.Rows)
            {
                Console.WriteLine(row);
            }
        }
    }
}
