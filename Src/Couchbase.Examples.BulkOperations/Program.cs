using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.Core;

namespace Couchbase.Examples.BulkOperations
{
    internal class Program
    {
        private static IBucket _bucket;

        private static void Main(string[] args)
        {
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    new Uri("http://192.168.77.101:8091/")
                }
            });
            _bucket = ClusterHelper.GetBucket("default");

            Task.Run(async () =>
            {
                var tasks = await GetTasks(1000);
                var results = await BulkUpsertAsync(tasks);
                WriteResults(results);
                Console.WriteLine();
            });

            Console.Read();
            ClusterHelper.Close();
        }

        static Task<Dictionary<string, Post>> GetTasks(int count)
        {
            var posts = new Dictionary<string, Post>();
            for (int i = 0; i < count; i++)
            {
                var key = "post" + i;
                posts.Add(key, new Post
                {
                    Author = "Author" + i,
                    PostId = key,
                    Content = "[Add content here]"
                });
            }

            return Task.FromResult(posts);
        }

        static async Task<IDictionary<string, IOperationResult<Post>>> BulkUpsertAsync(Dictionary<string, Post> posts)
        {
            return await Task.FromResult(_bucket.Upsert(posts,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 4
                }));
        }

        static void WriteResults(IDictionary<string, IOperationResult<Post>> results)
        {
            foreach (var operationResult in results.Values)
            {
                Console.WriteLine(operationResult.Success);
            }
        }
    }
}
