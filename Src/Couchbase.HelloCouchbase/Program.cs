using System;

namespace Couchbase.HelloCouchbase
{
    class Program
    {
        public static Cluster Cluster = new Cluster();

        static void Main(string[] args)
        {
            using (var bucket = Cluster.OpenBucket())
            {
                var document = new Document<dynamic>
                {
                    Id = "Hello",
                    Content = new
                    {
                        Name = "Couchbase"
                    }
                };

                var upsert = bucket.Upsert(document);
                if (upsert.Success)
                {
                    var get = bucket.GetDocument<dynamic>(document.Id);
                    document = get.Document;
                    var msg = string.Format("{0} {1}!", document.Id, document.Content.Name);
                    Console.WriteLine(msg);
                }
                Console.Read();
            }
        }
    }
}
    