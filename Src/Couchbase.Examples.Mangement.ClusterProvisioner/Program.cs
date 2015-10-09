using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.Configuration.Server.Serialization;
using Couchbase.Core;
using Couchbase.Management;

namespace Couchbase.Examples.Mangement.ClusterProvisioner
{
    class Program
    {
        private ICluster _cluster;
        private IClusterManager _manager;

        static void Main(string[] args)
        {
                try
                {
                    var provisioner = new Program();
                    provisioner.BootStrapCluster();
                }
                catch (BootstrapException e)
                {
                   Console.WriteLine(e.Message);
                   Console.WriteLine(e.StackTrace);
                }
            Console.Read();
        }

        static void PrintExceptions(AggregateException e)
        {

        }

        public void BootStrapCluster()
        {
            //get a config pointing at the initial entry point node (EP)
            var config = new ClientConfiguration
            {
                Servers = new List<Uri>
                {
                    new Uri("http://192.168.77.101:8091/")
                }
            };

            //create the cluster object
            _cluster = new Cluster(config);

            //use the factory method to create a ClusterManager
            _manager = _cluster.CreateManager("Administrator", "password");
        }

        /// <summary>
        /// Adds a node to the cluster.
        /// </summary>
        /// <param name="uriForNode">The URI for node.</param>
        /// <returns></returns>
        public async Task AddNode(Uri uriForNode)
        {
            var result = await _manager.AddNodeAsync(uriForNode.Host);
            if (result.Success)
            {
                Console.WriteLine("Node {0} added successfully.", uriForNode);
                return;
            }
            if (result.Exception != null)
            {
                Console.WriteLine("Failed to add node {0}: {1}\n{2}",
                    uriForNode, result.Message, result.Exception);
                throw result.Exception;
            }
        }
    }
}
