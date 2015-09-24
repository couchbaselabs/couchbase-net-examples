using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Couchbase.Examples.BulkOperations;

namespace Couchbase.BulkOperationsWithWhenAll
{
    class Program
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

        static Task<List<Task<IOperationResult<Post>>>> GetTasks(int count)
        {
            var posts = new List<Task<IOperationResult<Post>>>();
            for (int i = 0; i < count; i++)
            {
                var key = "post" + i;
                posts.Add(_bucket.InsertAsync(key, new Post
                {
                    Author = "Author" + i,
                    PostId = key,
                    Content = "[Add content here]"
                }));
            }

            return Task.FromResult(posts);
        }

        static async Task<IEnumerable<IOperationResult<Post>>> BulkUpsertAsync(List<Task<IOperationResult<Post>>> posts)
        {
            return await Task.WhenAll(posts);
        }

        static void WriteResults(IEnumerable<IOperationResult<Post>> results)
        {
            foreach (var operationResult in results)
            {
                Console.WriteLine(operationResult.Success);
            }
        }
    }
}
