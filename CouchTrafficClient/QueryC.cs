using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CouchTrafficClient
{
    class QueryC : QueryBase
    {
        public override string Run()
        {
            var result = Query("QueryC", "part4working", new List<object>() {0, 1, 2});
            var dataForKeyString = result["1"].First();
            return string.Format("The travel time for 5 minute span #0 is {0} seconds.", Math.Round((1.6 / float.Parse(((List<object>)dataForKeyString).First().ToString())) * 3600, 2).ToString());
        }
    }
}
