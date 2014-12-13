using System;
using System.Collections.Generic;
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
    }
}