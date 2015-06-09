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
            var result = Query("queryC", "partXXX", null, "traffic_subset");
            var dataForKeyString = result["keyString"].First();
            return string.Format("The result data for keyString was: {0}.", dataForKeyString.ToString());
        }
    }
}
