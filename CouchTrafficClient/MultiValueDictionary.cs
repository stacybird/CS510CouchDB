using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchTrafficClient
{
    class MultiValueDictionary : Dictionary<object, List<object>> 
    {
        public void Add(object k, object v) {
            if(!base.ContainsKey(k))
                base.Add(k, new List<object>());
            base[k].Add(v);
        }


    }
}
