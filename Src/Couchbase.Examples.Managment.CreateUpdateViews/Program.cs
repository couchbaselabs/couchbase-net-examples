﻿using System;

namespace Couchbase.Examples.Managment.CreateUpdateViews
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var cluster = new Cluster())
            {
                using (var bucket = cluster.OpenBucket())
                {
                    var manager = bucket.CreateManager("Administrator", "password");
                    var designDoc = "{\"views\":{\"all_docs\":{\"map\":\"function (doc, meta) { emit(meta.id, doc); }\"}}}";
                    var insertResult = manager.InsertDesignDocumentAsync("dd_docs", designDoc).Result;

                    if (insertResult.Success)
                    {
                        var designDocModified = "{\"views\": {}}";
                        var updateResult = manager.UpdateDesignDocumentAsync("dd_docs", designDocModified).Result;
                        Console.WriteLine(updateResult.Success);
                    }
                }
            }
            Console.Read();
        }
    }
}
