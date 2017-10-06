using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticBlogPost
{
    public class company
    {

        public long id { get; set; }

        public string name { get; set; }

        public string address { get; set; }

        public long areaCode { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string latestActivityTimestamp { get; set; }

        public location location { get; set; }
    }


    public class location
    {

        public double latitude { get; set; }

        public double longitude { get; set; }
    }

    /* [Nest.ElasticsearchType]
    public class company
    {
        [Nest.Number(Store = true)]
        public long id { get; set; }
        [Nest.Text(Store = true, Index = true, Analyzer= "standard", TermVector = Nest.TermVectorOption.WithPositionsOffsets)]
        public string name { get; set; }
        [Nest.Text(Store = true, Index = true, Analyzer = "standard", TermVector = Nest.TermVectorOption.WithPositionsOffsets)]
        public string address { get; set; }
        [Nest.Number(Store = true, Index =true)]
        public long areaCode { get; set; }
        [Nest.Text(Store = true, Index = true, Analyzer = "standard", TermVector = Nest.TermVectorOption.WithPositionsOffsets)]
        public string city { get; set; }
        [Nest.Text(Store = true, Index = true, Analyzer = "standard", TermVector = Nest.TermVectorOption.WithPositionsOffsets)]
        public string state { get; set; }
        [Nest.Date(Store = true, Index =true)]
        public string latestActivityTimestamp { get; set; }
        [Nest.Nested(Store = true)]
        public location location { get; set; }
    }

    [Nest.ElasticsearchType]
    public class location
    {
        [Nest.Number(Store = true)]
        public double latitude { get; set; }
        [Nest.Number(Store = true)]
        public double longitude { get; set; }
    }*/
}
