using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Core;

namespace CouchbaseAsyncExample
{
    class Program
    {
        public class Person
        {
            private static readonly string _type = "person";

            public Person()
            {
                Type = _type;
            }

            public string Id { get; set; }

            public string Name { get; set; }

            public int Age { get; set; }

            public string Type { get; private set; }
        }

        private static void Main(string[] args)
        {
            var count = 100000;
            var config = new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    //change this to your cluster to bootstrap
                    new Uri("http://localhost:8091/pools")
                }
            };

            ClusterHelper.Initialize(config);
            var bucket = ClusterHelper.GetBucket("default");

            var items = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                items.Add(new Person {Age = 21, Name = "Some name" + i, Id = i.ToString()});
            }

            Task.Run(async () => await UpsertAllAsync(items, bucket)).ConfigureAwait(false);

            Console.Read();
            ClusterHelper.Close();
        }

        public static async Task UpsertAllAsync(List<Person> persons, IBucket bucket)
        {
            var tasks = new List<Task<IOperationResult<Person>>>();
            persons.ForEach(x => tasks.Add(bucket.UpsertAsync(x.Id, x)));

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            Console.WriteLine("Total upserted: {0}", results.Length);
        }
    }
}
