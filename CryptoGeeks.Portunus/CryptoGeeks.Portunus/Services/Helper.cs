using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace CryptoGeeks.Portunus.Services
{
    public static class ExtensionMethod
    {
        public static ObservableCollection<T> ToObservableCollection<T>
      (this IEnumerable<T> en)
        {
            return new ObservableCollection<T>(en);
        }
    }

    public class Helper
    {
        public string GetCompleteUrl(string baseUrl, NameValueCollection parameters)
        {
            StringBuilder queryString = new StringBuilder();

            if (parameters == null || parameters.Count ==0)
                return baseUrl;

            foreach (string key in parameters)
            {
                queryString.Append(key + "=" + parameters[key] + "&");
            }

            string paramString = queryString.ToString().Remove(queryString.Length - 1, 1);
            return baseUrl + "?" + Uri.EscapeUriString(paramString);
        }

    }
}
