using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch;
using Nest;

namespace Elastic
{
    public class ElasticC
    {
        private const string DefaultIndexName = "esbot";
        private const string ElasticSearchServerUri = @"http://localhost:9200";
        private const string UsersIndexName = "indexcompany";
        public static IElasticClient CreateElasticClient()
        {
            var settings = CreateConnectionSettings();
            var client = new ElasticClient(settings);

            if (!client.IndexExists(UsersIndexName).Exists)
            {

                // client.DeleteIndex(UsersIndexName);
                client.CreateIndex(UsersIndexName, descriptor => descriptor
                                     .Mappings(ms => ms
                                                 .Map<company>(m => m
                                                     .AutoMap()
                                                 )
                                              )
                                  );
            }
            return client;
        }
        private static ConnectionSettings CreateConnectionSettings()
        {
            var uri = new Uri(ElasticSearchServerUri);
            var settings = new ConnectionSettings(uri)
                .DefaultIndex(DefaultIndexName);
            return settings;
        }
    }
}
