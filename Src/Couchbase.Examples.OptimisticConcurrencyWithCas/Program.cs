
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.IO;

namespace Couchbase.Examples.OptimisticConcurrencyWithCas
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

            Task.Run(() => UpdatePostWithCasAsync(revision0));

            Console.Read();
            ClusterHelper.Close();
        }

        static async Task<bool> UpdatePostWithCasAsync(Post post)
        {
            var bucket = ClusterHelper.GetBucket("default");
            const int maxAttempts = 10;
            var attempts = 0;

            do
            {
                //get the original document - if it doesn't exist fail
                var result = await bucket.GetAsync<Post>(post.PostId);
                if (result.Status == ResponseStatus.KeyNotFound)
                {
                    return false;
                }

                //update the original documents fields
                var original = result.Value;
                original.Content = post.Content;
                original.Author = post.Author;
                original.Views = original.Views++;

                //perform the mutation passing in the CAS value
                result = await bucket.UpsertAsync(original.PostId, original, result.Cas);
                if (result.Success)
                {
                    return true;
                }

                //failed so try again up until maxAttempts
            } while (attempts++ < maxAttempts);

            //we failed within maxAttemps
            return false;
        }
    }
}
