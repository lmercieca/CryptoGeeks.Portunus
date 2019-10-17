using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace CryptoGeeks.Portunus.Services
{
    public class Helper
    {
        public string GetCompleteUrl(string baseUrl, NameValueCollection parameters)
        {
            StringBuilder queryString = new StringBuilder();

            foreach (string key in parameters)
            {
                queryString.Append(key + "=" + parameters[key]);
            }

            return baseUrl + "?" + System.Web.HttpUtility.HtmlEncode(queryString);
        }
    }
}
