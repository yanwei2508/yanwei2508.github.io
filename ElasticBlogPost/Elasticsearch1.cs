using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
//using BotEsNest.Ext;

namespace ElasticBlogPost
{
    public class Elasticsearch1
    {
       public static string indexName = "bank";
        public static Uri node = new Uri("http://localhost:9200");
        public static ConnectionSettings settings = new ConnectionSettings(node).DefaultIndex(indexName);
        public static ElasticClient elastic = new ElasticClient(settings);
        /*public static ISearchResponse<Bank> esSearchNumber()
        {
            //
            string dictionaryKey = "balance";
            var dictionary = Extension.BankDictionary();
            var rangeField = dictionary[dictionaryKey];
            var gt = 40000;
            var lt = 40100;
            var searchResponse = elastic.Search<Bank>(es => es
                                                                                                  .Query(q => q
                                                                                                        .Range(r => r
                                                                                                            .Name("")
                                                                                                            .Field(rangeField)
                                                                                                                .GreaterThan(gt)
                                                                                                                .LessThan(lt)
                                                                                                                .Boost(2.0))));
            return searchResponse;
        }*/
        public static ISearchResponse<company> esSearchString()
        {
            string quertStr = "Milton";
      
            var searchResponse = elastic.Search<company>(es => es
                                                                                      .Query(q =>
                                                                                      q.QueryString(qs => qs.Query(quertStr))
                                                                                                                ));
            return searchResponse;
        }
        /*public static ISearchResponse<Bank> esSearchField()
        {
            string queryStr = "35";
            string dictionaryKey = "age";
            var dictionary = Extension.BankDictionary();
            var rangeField = dictionary[dictionaryKey];
            var searchResponse = elastic.Search<Bank>(es => es
                                            .Query(q => q
                                                    .Match(m => m
                                                        .Field(rangeField)
                                                            .Query(queryStr))));
            return searchResponse;
        }*/
        
    }
}
