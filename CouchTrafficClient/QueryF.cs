using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CouchTrafficClient
{
    class QueryF : QueryBase
    {
        public override string Run()
        {
            var result = Query("QueryF", "part1");
            // As a special case, null must be quoted coming back
            // var value = result[QueryNullResult];
            // var value = result[1045].First().downstream;
            dynamic record = result["1045"].First();
            var value = record.downstream;
            // take values
            // start at Johnson Creek location
            // walk through values
            // return the walked through list
            // 
            return string.Format("The total count is {0}.", value.ToString());
        }
    }
}
