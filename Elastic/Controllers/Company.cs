using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic
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

    public class companyMainInfo
    {
        public long id { get; set; }

        public string name { get; set; }

        public string address { get; set; }

        public string city { get; set; }
    }

    public class textInfo
    {
        public long id { get; set; }

        public string text { get; set; }

    }
}
