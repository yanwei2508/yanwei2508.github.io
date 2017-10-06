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
    public class Elasticsearch
    {
        private const string UsersIndexName = "indexcompany";
        private static IElasticClient elastic; 
        public  static void InitialElasticSearch()
        {
            /********initial ElasticClient********/
            elastic = ElasticC.CreateElasticClient();

            /********import data**********/
            ImportEs.ImportJson(elastic,UsersIndexName);

            /***************************query test from UsersIndexName********************/
            esSearch();
            /*****************************************************************************/

            /***************************key word query with all of fields from UsersIndexName********************/
            int l = esSearchString("Milton").Fields.Count();
            /*****************************************************************************/

            /***********************key word query with multi fields from UsersIndexName*******************************/
            esMultiFieldsSearch("lud");
            /**************************************************************************************/
        }

        public static ISearchResponse<company> esSearch()
        {
            var searchResponse = elastic.Search<company>(e => e
                                                        .Type("company")
                                                        .Index(UsersIndexName)
                                                        .Size(1000)
                                                    );
            return searchResponse;
        }

        public static ISearchResponse<company> esSearchString(string quertStr)
        {
            var searchResponse = elastic.Search<company>(es => es
                                                                 .Query(q => q
                                                                              .QueryString(qs => qs.Query(quertStr))
                                                                       )
                                                        );
            return searchResponse;
        }

        public static ISearchResponse<company> esMultiFieldsSearch(string quertStr)
        {
            var searchResponse = elastic.Search<company>(s => s
                    .Index(UsersIndexName)
                    .Type("company")
                    .Query(q => q
                        .MultiMatch(m => m
                            .Query("vig")
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
            return searchResponse;
        }

    }
}
