﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CouchTrafficClient
{
    class QueryE : QueryBase
    {
        public string Run()
        {
            return base.Run(); // return Query("querya", "querya").ToString();
        }
    }
}
