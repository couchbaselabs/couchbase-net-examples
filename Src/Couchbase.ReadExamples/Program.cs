using System;
using Newtonsoft.Json;

namespace Couchbase.ReadExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Cluster cluster = new Cluster())
            {
                using (var bucket = cluster.OpenBucket("beer-sample"))
                {
                    var result = bucket.Get<dynamic>("21st_amendment_brewery_cafe");
                    if (result.Success)
                    {
                        Console.WriteLine(JsonConvert.SerializeObject(result.Value));
                    }
                }
            }

            Console.Read();
        }
    }
}
