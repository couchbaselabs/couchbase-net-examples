using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.Core;
using Couchbase.IO;

namespace Couchbase.Examples.PessimisticConcurrencyWithLock
{
    class Program
    {
        static void Main(string[] args)
        {
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    new Uri("http://localhost:8091/")
                }
            });
            var bucket = ClusterHelper.GetBucket("default");

            var revision0 = new Post
            {
                PostId = "p0001",
                Author = "Stuie",
                Published = new DateTime(2012, 1, 13),
                Content = "A really cool blog post!",
                Views = 0
            };

            //insert the first revision
            bucket.Upsert(revision0.PostId, revision0);

            Task.Run(() => UpdatePostWithLockAsync(revision0));

            Console.Read();
            ClusterHelper.Close();
        }

        static async Task<bool> UpdatePostWithLockAsync(Post modified)
        {
            var bucket = ClusterHelper.GetBucket("default");
            var success = false;

            //get the original document - if it doesn't exist fail
            var result = await bucket.GetWithLockAsync<Post>(modified.PostId, TimeSpan.FromSeconds(5));
            if (result.Success)
            {
                //update the original documents fields
                var original = result.Value;
                original.Content = modified.Content;
                original.Author = modified.Author;
                original.Views = original.Views++;

                //perform the mutation passing in the CAS value
                var updated = await bucket.UpsertAsync(original.PostId, original, result.Cas);
                if (updated.Success)
                {
                    success = true;
                }
                await bucket.UnlockAsync(original.PostId, result.Cas);
            }
            return success;
        }
    }
}
