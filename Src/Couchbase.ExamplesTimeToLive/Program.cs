using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;

namespace Couchbase.ExamplesTimeToLive
{
    class Program
    {
        static void Main(string[] args)
        {
            ClusterHelper.Initialize(new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    new Uri("http://192.168.77.101/:8091/")
                }
            });

            UpsertWithTtl();

            UpsertWithDocumentTtl();

            Console.Read();
            ClusterHelper.Close();
        }

        static async void UpsertWithTtl()
        {
            var bucket = ClusterHelper.GetBucket("default");

            var result = await bucket.UpsertAsync("foo", "bar", new TimeSpan(2, 0, 0, 0));
            if (result.Success)
            {
                //upsert was successful
            }
        }

        static async void UpsertWithDocumentTtl()
        {
            var bucket = ClusterHelper.GetBucket("default");

            var result = await bucket.UpsertAsync(new Document<string>
            {
                Id = "foo",
                Content = "bar",
                Expiry = (uint) new TimeSpan(2, 0, 0, 0).TotalMilliseconds
            });

            if (result.Success)
            {
                //upsert was successful
            }
        }
    }
}
