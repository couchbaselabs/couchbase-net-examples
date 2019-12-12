using System;

namespace Couchbase.N1QLExamples
{
    class Program
    {
        static Cluster _cluster = new Cluster();

        static void Main(string[] args)
        {
            using (var bucket = _cluster.OpenBucket("beer-sample"))
            {
                const string query = "SELECT name FROM `beer-sample` LIMIT 100";
                var result = bucket.Query<dynamic>(query);
                foreach (var row in result.Rows)
                {
                    Console.WriteLine(row);
                }
            }
            _cluster.Dispose();
            Console.Read();
        }
    }
}
