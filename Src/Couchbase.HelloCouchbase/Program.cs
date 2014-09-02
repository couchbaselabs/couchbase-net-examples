using System;

namespace Couchbase.HelloCouchbase
{
    class Program
    {
        public static CouchbaseCluster Cluster = new CouchbaseCluster();

        static void Main(string[] args)
        {
            using (var bucket = Cluster.OpenBucket())
            {
                var document = new Document<dynamic>
                {
                    Id = "Hello",
                    Value = new
                    {
                        Name = "Couchbase"
                    }
                };

                var result = bucket.Upsert(document);
                var msg = string.Format("Inserted document '{0}': {1}", 
                    document.Id, result.Success);

                Console.WriteLine(msg);
            }
        }
    }
}
    