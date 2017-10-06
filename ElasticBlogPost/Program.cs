using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Nest;
using Elasticsearch;
using Elasticsearch.Net;

namespace ElasticBlogPost
{
    class Program
    {
        private const string DefaultIndexName = "esbot";
        private const string ElasticSearchServerUri = @"http://localhost:9200";
        private const string UsersIndexName = "indexcompany";
        public static void Main()
        {

            var elastic = CreateElasticClient();

            /************old code for building elasticClient***********************
            var node = new Uri("http://localhost:9200");
            var indexName = "esbot"; //"index-here";
            var settings = new ConnectionSettings(node).DefaultIndex(indexName);
            var elastic = new ElasticClient(settings);
            if (!elastic.IndexExists("index-here").Exists)
            {
                var createIndexResponse = elastic.CreateIndex(indexName);
                var mappingBlogPost = elastic.Map<Resume>(s => s.AutoMap());
            }
            ************************************************************************/

            /****************************It is an example json data,it can be indexed by follow method*************************************
                   var indexResponse = elastic.LowLevel.Index<string>("index-here", "type-test", json);
                   But the companyData.json, can not be indexed by this way ,so I find other way to index them.
                   can look the codes after this json data.*
           ********************************************************************************************************************************
                            var json = @"{
                    ""BookName"": ""Book1"",
                    ""ISBN"": ""978-3-16-148410-0"",
                    ""chapter"" : [
                        {
                            ""chapter_name"": ""Chapter1"",
                            ""chapter_desc"": ""Before getting into computer programming, let us first understand computer programs and what they...""
                        },
                        {
                    ""chapter_name"": ""Chapter2"",
                            ""chapter_desc"": ""Today computer programs are being used in almost every field, household, agriculture, medical, entertainment, defense..""
                        },
                        {
                    ""chapter_name"": ""Chapter3"",
                            ""chapter_desc"": ""MS Word, MS Excel, Adobe Photoshop, Internet Explorer, Chrome, etc., are...""
                        },
                        {
                    ""chapter_name"": ""Chapter4"",
                            ""chapter_desc"": ""Computer programs are being used to develop graphics and special effects in movie...""
                        }
                    ]
                }";
                *******************************************************************************************************************************/
            StreamReader r = new StreamReader(@"C:\Users\xuyan\Downloads\companyData - Copy.json");
            //StreamReader r = new StreamReader(@"C:\Users\xuyan\Downloads\companyData.json");
            //here I try to modify the companydata.json to the same as the up example json data, but it still doesn't work. So find the other method to index .
            //var json1 = r.ReadToEnd().Replace("\n\t", "\r\n    ");
             var json1 = r.ReadToEnd();
            List<company> items = JsonConvert.DeserializeObject<List<company>>(json1);

            // here I use the same way as the sample json data to index companydata.json, just need to do it in a loop
            /*foreach (var item in items)
            {
                //var indexResponse = elastic.Index(item);
                var indexResponse = elastic.LowLevel.Index<string>("index-here", "type-company1", item.ToString());
                if (!indexResponse.Success)
                    Console.WriteLine(indexResponse.DebugInformation);
            }*/

            // here list from json can be indexed in one time
            var indexResponse = elastic.IndexMany(items, UsersIndexName, "company");
            if (!( indexResponse.Items.Count > 0 ) )
            Console.WriteLine(indexResponse.DebugInformation);
            elastic.Refresh(UsersIndexName);

            /***************************query test from UsersIndexName********************/
            var readResult = elastic.Search<company>(e => e
                                                        .Type("company")
                                                        .Index(UsersIndexName)
                                                        .Size(1000)
                                                    );
            
            /****************************************************************************/

            /***********************query test from UsersIndexName*******************************
            var searchResponse = elastic.Search<company>(s => s
                    .Index(UsersIndexName)
                    .Type("company")
                    .Query(q => q
                        .MultiMatch(m => m
                            .Query("vigs")
                            .Type(TextQueryType.PhrasePrefix)
                            .Fields(f => f
                                .Field(ff => ff.name)
                                .Field(ff => ff.address)
                                .Field(ff => ff.city)
                            )
                        )
                    )
                );

              Console.WriteLine(searchResponse.DebugInformation);
            /*************************************************************************************/

            var searchResponse = elastic.Search<company>(s => s
                    .Index(UsersIndexName)
                    .Type("company")
                    .Query( q => q

                        .MultiMatch(m => m
                            .Query("Lud")
                            .Type(TextQueryType.PhrasePrefix)
                            .Fields(f => f
                                .Field(ff => ff.name)
                                .Field(ff => ff.address)
                                .Field(ff => ff.city)
                            )
                        )
                    )
                    .Highlight(h=> h
                        //.RequireFieldMatch(false)
                        .PreTags("<b>")
                        .PostTags("</b>")
                        
                        .Fields(
                             fs => fs.Field(fsf => fsf.address),
                             fs => fs.Field( fsf=> fsf.city),
                             fs => fs.Field(fsf=> fsf.name)
                            
                        )
                            
                    )
                );

            Console.WriteLine(searchResponse.DebugInformation);

            //Elasticsearch.InitialElasticSearch();
        }

        public static IElasticClient CreateElasticClient()
        {
            var settings = CreateConnectionSettings();
            var client = new ElasticClient(settings);

            if (!client.IndexExists(UsersIndexName).Exists)
            {

                client.DeleteIndex(UsersIndexName);
                // }
                //client.CreateIndex(UsersIndexName);

                // client.Map<company1>(s => s.AutoMap());

                client.CreateIndex(UsersIndexName, descriptor => descriptor
                                        .Mappings(ms => ms
                                                    .Map<company>(m => m
                                                                    .AutoMap()
                                                                    /*.Properties(ps => ps
                                                                                    .Text(s => s
                                                                                            .Name(n => n.name)
                                                                                            //.Analyzer("stand")
                                                                                         )
                                                                                    .Text(s => s
                                                                                             .Name(n => n.address)
                                                                                             //.Analyzer("stand")
                                                                                             //.SearchAnalyzer("substring_analyzer")
                                                                                         )
                                                                                )   */
                                                                )
                                                )
                                       /* .Settings(s => s
                                            .Analysis(a => a
                                                .Analyzers(analyzer => analyzer
                                                    .Custom("substring_analyzer", analyzerDescriptor => analyzerDescriptor
                                                        .Tokenizer("standard")
                                                        .Filters("lowercase", "substring")
                                                    )
                                                )
                                                .TokenFilters(tf => tf
                                                    .NGram("substring", filterDescriptor => filterDescriptor
                                                        .MinGram(2)
                                                        .MaxGram(15)
                                                    )
                                                )
                                            )
                                        )*/
                );
            }
            return client;
        }
        private static ConnectionSettings CreateConnectionSettings()
        {
            var uri = new Uri(ElasticSearchServerUri);
            var settings = new ConnectionSettings(uri)
                .DefaultIndex(DefaultIndexName)
                //.InferMappingFor<company>(d => d
                //    .IndexName(UsersIndexName)
                //)
                ;

            return settings;
        }
    }
}