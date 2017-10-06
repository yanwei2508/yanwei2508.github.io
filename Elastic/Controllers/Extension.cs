using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Elastic
{
    class Extension
    {
        public static Dictionary<string, Expression<Func<companyMainInfo, object>>> CompanyDictionary()
        {
            return new Dictionary<string, Expression<Func<companyMainInfo, object>>>
            {
                { "id",p=>p.id},
                { "name",p=>p.name},
                {"address",p=>p.address},
                {"city", p=> p.city}
            };
        }
    }
}
