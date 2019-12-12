﻿using System;

namespace Couchbase.LoggingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var cluster = new Cluster())
            {
                using (var bucket = cluster.OpenBucket())
                {
                    bucket.Upsert("somekey", "somevalue");
                }
            }
            Console.Read();
        }
    }
}
