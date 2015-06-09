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
    private string Server { get { return "http://52.10.252.48:5984/traffic/"; } }

        /// <summary>
        /// Query a view from our CouchDB Server, returning a Dictionary of keys to values!
        /// </summary>
        /// <param name="designDocumentName">Name of the Design that the View lives within.</param>
        /// <param name="viewName">Name of the View that you would like to query.</param>
        /// <param name="keys">Optional list of keys to query the view for.</param>
        /// <returns></returns>
    public MultiValueDictionary<object, object> Query(string designDocumentName, string viewName, IList<object> keys = null)
    {
        dynamic queryResult = InternalQuery(designDocumentName, viewName, keys);
        IList<object> a = queryResult.rows;
        var result = new MultiValueDictionary<object, object>();
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
