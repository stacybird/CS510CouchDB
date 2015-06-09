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
    abstract class QueryBase
    {
        /// <summary>
        /// Key used when the results from a Query had a row with a "null" key value.
        /// </summary>
        public const string QueryNullResult = "null";

        /// <summary>
        ///  Form where intermediate results from your execution can live if needed.
        /// </summary>
        protected Form resultForm;
        public void RunAsyncWithForm(Form myForm, Button button)
        {
            resultForm = myForm;
            button.Enabled = false;
            var t = new Task(() =>
            {
                try
                {
                    var result = this.Run();
                    if (resultForm == null)
                    {
                        MessageBox.Show(result);
                    }
                    button.BeginInvoke((Action)delegate() { button.Enabled = true; });
                }
                catch (QueryException)
                {
                    if (resultForm != null)
                    {
                        resultForm.Close();
                    }
                    button.BeginInvoke((Action)delegate() { button.Enabled = true; });
                }
                catch (Exception x)
                {
                    MessageBox.Show("Error in Application: " + x.ToString());
                    if (resultForm != null)
                    {
                        resultForm.Close();
                    }
                    button.BeginInvoke((Action)delegate() { button.Enabled = true; });
                }
            });
            t.Start();
        }
        public abstract string Run();
    private string Server { get { return "http://52.10.252.48:5984/"; } }

    /// <summary>
    /// Query a view from our CouchDB Server, returning a Dictionary of keys to values!
    /// </summary>
    /// <param name="designDocumentName">Name of the Design that the View lives within.</param>
    /// <param name="viewName">Name of the View that you would like to query.</param>
    /// <param name="keys">Optional list of keys to query the view for.</param>

    /// <returns></returns>
    public MultiValueDictionary Query(string designDocumentName, string viewName, IList<object> keys = null, string db = "traffic")
    {
        return InternalQuery(designDocumentName, viewName, null, null, keys, db);
    }
    /// <summary>
    /// Query a view from our CouchDB Server, returning a Dictionary of keys to values!
    /// </summary>
    /// <param name="designDocumentName">Name of the Design that the View lives within.</param>
    /// <param name="viewName">Name of the View that you would like to query.</param>
    /// <param name="startKey">Optional list of startkeys to query the view for.</param>
    /// <param name="endKey">Optional list of endkeys to query the view for.</param>
    /// <returns></returns>
    public MultiValueDictionary Query(string designDocumentName, string viewName, object startKey, object endKey, string db = "traffic")
    {
        return InternalQuery(designDocumentName, viewName, startKey, endKey, null, db);
    }
    private MultiValueDictionary InternalQuery(string designDocumentName, string viewName, object startKey = null, object endKey = null, IList<object> keys = null, string db = "traffic")
    {
        try
        {
            var keyString = "";
            if (keys != null)
            {
                keyString = string.Format("?keys={0}&group=true", Uri.EscapeDataString(JsonConvert.SerializeObject(keys)));
            }
            else if (startKey != null && endKey != null)
            {
                keyString = string.Format("?startkey={0}&endkey={1}&group=true", Uri.EscapeDataString(JsonConvert.SerializeObject(startKey)), Uri.EscapeDataString(JsonConvert.SerializeObject(endKey)));
            }
            else if (startKey != null)
            {
                keyString = string.Format("?startkey={0}&group=true", Uri.EscapeDataString(JsonConvert.SerializeObject(startKey)));
            }
            else if (endKey != null)
            {
                keyString = string.Format("?endkey={0}&group=true", Uri.EscapeDataString(JsonConvert.SerializeObject(endKey)));
            }
            var url = Server + db + "/_design/" + designDocumentName + "/_view/" + viewName + keyString;
            dynamic queryResult;
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E)";
                string str = wc.DownloadString(url);
                queryResult = JsonConvert.DeserializeObject<ExpandoObject>(str);
            }
            IList<object> a = queryResult.rows;
            var result = new MultiValueDictionary();
            foreach (dynamic data in a)
            {
                result.Add(data.key == null ? QueryNullResult : data.key.ToString(), data.value);
            }
            return result;
        }
        catch (Exception e)
        {
            MessageBox.Show("Error in WebClient: " + e.ToString());
            throw new QueryException();
        }
    }
}

    
}
