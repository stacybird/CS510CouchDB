using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Dynamic;

namespace CouchTrafficClient
{
    class QueryA : QueryBase
    {
        public string Run()
        {
            var result = Query("querya", "querya");
            // As a special case, null must be quoted coming back
            var value = result[QueryNullResult];
            return String.Format("The total count is {0}.", value.ToString());
        }
    }
}
