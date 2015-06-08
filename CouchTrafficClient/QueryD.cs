using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CouchTrafficClient
{
    class QueryD : QueryBase
    {
        public string Run()
        {
            return base.Run(); // return Query("querya", "querya").ToString();
        }
    }
}
