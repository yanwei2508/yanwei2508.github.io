using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Nest;
using Elasticsearch.Net;
using Stormpath;
using Stormpath.SDK;
namespace ElasticBlogPost
{
    public class ImportEs
    {
        public static void ImportJson(IElasticClient elastic,string UsersIndexName)
        {
            StreamReader r = new StreamReader(@"C:\Users\xuyan\Downloads\companyData - Copy.json");
            var json1 = r.ReadToEnd();
            List<company> items = JsonConvert.DeserializeObject<List<company>>(json1);

            // here list from json can be indexed in one time
            var indexResponse = elastic.IndexMany(items, UsersIndexName, "company");
            if (!(indexResponse.Items.Count > 0))
                Console.WriteLine(indexResponse.DebugInformation);
            elastic.Refresh(UsersIndexName);
        }

        
    }
}
