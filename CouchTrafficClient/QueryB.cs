using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CouchTrafficClient
{
    class QueryB : QueryBase
    {
        public override string Run()
        {
            var result = Query("queryb", "queryb"); // , new List<object> {"foo", "bar"};
            // As a special case, null must be quoted coming back
            var value = result[QueryNullResult];
            return string.Format("The total volume is {0}.", value.ToString());
        }
    }
}
