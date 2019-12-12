﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.Core;

namespace Couchbase.Examples.ReplicateTo
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
                    new Uri("http://localhost:8091/")
                }
            });
            _bucket = ClusterHelper.GetBucket("default");

            Task.Run(async () =>
            {
                var result = await InsertWithReplicateTo(new Post
                {
                    PostId = "p-0002",
                    Author = "Bingo Bailey",
                    Content = "Some nice content",
                    Published = DateTime.Now
                }, Couchbase.ReplicateTo.Two);
                Console.WriteLine(result);
            });

            Console.Read();
            ClusterHelper.Close();
        }

        private static async Task<bool> InsertWithReplicateTo(Post value, Couchbase.ReplicateTo replicateOptions)
        {
            var result = await _bucket.UpsertAsync(value.PostId, value, replicateOptions);
            if (result.Success)
            {
                return true;
            }
            Console.WriteLine(result.Status);
            return false;
        }
    }
}
