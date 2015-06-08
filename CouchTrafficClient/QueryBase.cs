using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Dynamic;

namespace CouchTrafficClient
{
    class QueryException : Exception
    {

    }
    class QueryBase
    {
        public const string QueryNullResult = "null";
        public string Run()
        {
            return "Query Client Not Implemented";
    }
    public string Server { get { return "http://52.10.252.48:5984/traffic/"; } }
    public Dictionary<object, object> Query(string designDocumentName, string viewName, IList<object> keys = null)
    {
        dynamic queryResult = InternalQuery(designDocumentName, viewName, keys);
        IList<object> a = queryResult.rows;
        var result = new Dictionary<object, object>();
        foreach (dynamic data in a)
        {
            result.Add(data.key ?? QueryNullResult, data.value);
        }
        return result;
    }
    private ExpandoObject InternalQuery(string designDocumentName, string viewName, IList<object> keys = null)
    {
        try
        {
            var keyString = "";
            if (keys != null)
            {
                keyString = string.Format("?keys={0}", Uri.EscapeDataString(JsonConvert.SerializeObject(keys)));
            }
            var url = Server + "_design/" + designDocumentName + "/_view/" + viewName + keyString;
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E)";
                string str = wc.DownloadString(url);
                return JsonConvert.DeserializeObject<ExpandoObject>(str);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("Error in WebClient: " + e.ToString());
            throw new QueryException();
        }
    }
}

    
}
