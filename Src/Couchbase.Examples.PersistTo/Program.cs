using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Couchbase.IO.Operations;

namespace Couchbase.Examples.PersistTo
{
    class Program
    {
        private static IBucket _bucket;
        static void Main(string[] args)
        {
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                     new Uri("http://localhost:8091/")
                }
            });
            _bucket = ClusterHelper.GetBucket("default");

            Task.Run(async () =>
            {
                var result = await InsertWithPersistenceOptions(new Post
                {
                    PostId = GenerateUniqueValue(),
                    Author = GenerateUniqueValue("Author"),
                    Content = GenerateUniqueValue("Content "),
                    Published = DateTime.Now
                }, Couchbase.PersistTo.One);

                Console.WriteLine(result);
            });

            Console.Read();
            ClusterHelper.Close();
        }

        private static string GenerateUniqueValue(string prefix = null)
        {
            return prefix != null ? prefix + Guid.NewGuid() : Guid.NewGuid().ToString();
        }

        static async Task<bool> InsertWithPersistenceOptions(Post value, Couchbase.PersistTo persistenceOptions)
        {
            var result = await _bucket.InsertAsync(value.PostId, value, ReplicateTo.Zero, persistenceOptions);
            return result.Success;
        }
    }
}
