using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;


namespace Elastic.Controllers
{
    [RoutePrefix("api/SearchByTyping")]
    public class SearchByTypingController : ApiController
    {
        private const string UsersIndexName = "indexcompany";
        private static IElasticClient elastic;

        [HttpGet]
        //GET api/Default
        //GET api/Default?name=John%20Doe 
        //GET api/Default/John%20Doe
        [Route("{strKey?}")]
        public string Get(String strKey)
        {
            elastic = ElasticC.CreateElasticClient();

            var searchResponse = elastic.Search<company>(s => s
                    .Index(UsersIndexName)
                    .Type("company")
                    .Query(q => q
                        .MultiMatch(m => m
                            .Query(strKey)
                            .Type(TextQueryType.PhrasePrefix)
                            .Fields(f => f
                                .Field(ff => ff.name)
                                .Field(ff => ff.address)
                                .Field(ff => ff.city)
                            )
                        )
                    )
                    .Highlight(h => h
                        .PreTags("")
                        .PostTags("")
                        .Fields(
                           fs => fs.Field(fsf => fsf.address),
                           fs => fs.Field(fsf => fsf.city),
                           fs => fs.Field(fsf => fsf.name)
                        )

                    )
                );

            var doc = searchResponse.Documents;
            var hits = searchResponse.Hits;
            var docCount = doc.Count;
            List<textInfo> lstTextInfo = new List<textInfo>();
            textInfo textinfo;
            if (docCount > 0)
            {
                List<company> list = doc.ToList<company>();
                for (int i = 0; i < hits.Count; i++)
                {
                    var id = searchResponse.Hits.ElementAt(i).Id;
                    var hitghtText = "";  
                    //var document1 = searchResponse.Hits.Select(h => h.Highlights.Values.Select(v => string.Join(", ", v.Highlights)));
                    if (hits.ElementAt(i).Highlights.Count>1)
                    { 
                        for (int j = 0; j < hits.ElementAt(i).Highlights.Count; j ++)
                        {
                            hitghtText = searchResponse.Hits.ElementAt(i).Highlights.ElementAt(j).Value.Highlights.ElementAt(0);
                            textinfo = new textInfo();
                            textinfo.id = Int32.Parse(id);
                            textinfo.text = hitghtText.ToString();
                            lstTextInfo.Insert(i, textinfo);
                        }
                    }
                    else
                    {
                        hitghtText = searchResponse.Hits.ElementAt(i).Highlights.ElementAt(0).Value.Highlights.ElementAt(0);
                        textinfo = new textInfo();
                        textinfo.id = Int32.Parse(id);
                        textinfo.text = hitghtText.ToString();
                        lstTextInfo.Insert(i, textinfo);
                    }
                }
            }
            try
            {
                return JsonConvert.SerializeObject(lstTextInfo);
            }
            catch
            {
                return null;
            }
            
            /*try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(lstTextInfo.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, lstTextInfo);
                   return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                return null;
            }*/
        }
    }
}
