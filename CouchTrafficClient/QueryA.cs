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
            dynamic result = Query("querya", "querya");
            IList<object> a = result.rows;
            dynamic data = a[0];
            var value = data.value;
            return String.Format("The total count is {0}.", value.ToString());
        }
    }
}
