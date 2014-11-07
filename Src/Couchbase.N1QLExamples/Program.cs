using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Couchbase.N1QLExamples
{
    class Program
    {
        static Cluster _cluster = new Cluster();

        static void Main(string[] args)
        {
            using (var bucket = _cluster.OpenBucket())
            {
                const string query = "SELECT c FROM tutorial as c";
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
