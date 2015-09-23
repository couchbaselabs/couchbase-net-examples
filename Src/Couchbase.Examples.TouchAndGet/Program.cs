using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;

namespace Couchbase.Examples.TouchAndGet
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
            GetAndTouchAsync();
            Console.Read();
            ClusterHelper.Close();
        }

        private static async void GetAndTouchAsync()
        {
            var bucket = ClusterHelper.GetBucket("default");
            var result = bucket.Upsert("foo", "bar", new TimeSpan(0, 0, 0, 3));

            if (result.Success)
            {
                Thread.Sleep(1000);

                result = bucket.GetAndTouch<string>("foo", new TimeSpan(0, 0, 0, 3));
                Console.WriteLine(result.Success);
            }
        }
    }
}
