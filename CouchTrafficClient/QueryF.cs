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
            var list = new List<string>();
            var key = "1046";
            while (key != "0") {
                dynamic record = result[key].First();
                var name = record.locationtext;
                key = record.downstream.ToString();
                list.Add(name);
            }
            var value = string.Join(", ", list);
            // take values
            // start at Johnson Creek location
            // walk through values
            // return the walked through list
            // 
            return string.Format("The route is: {0}.", value);
        }
    }
}
