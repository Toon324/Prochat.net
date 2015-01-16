using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Prochat.WebAccess
{
    public class Requests
    {

        public static HttpWebResponse SendWebRequest(string requestUrl)
        {
            HttpWebRequest request = (HttpWebRequest)
                             WebRequest.Create(requestUrl);

            return request.GetResponse() as HttpWebResponse;

        }

        //For when the return type needs a wrapper
        public static StreamReader GetJsonReader(string requestUrl)
        {
            return new StreamReader(SendWebRequest(requestUrl).GetResponseStream());
        }

    }
}