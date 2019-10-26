using CryptoGeeks.Portunus.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Services
{
    public class EntityService<T>
    {
        public async Task<string> GetWithNoReturn(string url, NameValueCollection parameters)
        {
            string parsedUrl = new Helper().GetCompleteUrl(url, parameters);
            
            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

                HttpResponseMessage resp = await httpClient.GetAsync(parsedUrl).ConfigureAwait(false);
            }

            return await Task.FromResult("");
        }


        public async Task<T> Get(string url, NameValueCollection parameters)
        {
            string parsedUrl = new Helper().GetCompleteUrl(url, parameters);
            string response;

            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

                HttpResponseMessage resp = await httpClient.GetAsync(parsedUrl).ConfigureAwait(false);
                response = await resp.Content.ReadAsStringAsync();
            }
            
            T res = JsonConvert.DeserializeObject<T>(response);
            return await Task.FromResult(res);
        }

        public async Task<T> GetWithUserId(string url)
        {
            NameValueCollection parameters = new NameValueCollection();

            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("UserId", secureStorage.GetFromSecureStorage(Constants.UserId));

            string parsedUrl = new Helper().GetCompleteUrl(url, parameters);
            string response;


            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

                HttpResponseMessage resp = await httpClient.GetAsync(parsedUrl).ConfigureAwait(false);
                response = await resp.Content.ReadAsStringAsync();
            }

            T res = JsonConvert.DeserializeObject<T>(response);
            return await Task.FromResult(res);
        }


        public async Task<T> GetWithUsername(string url)
        {
            NameValueCollection parameters = new NameValueCollection();
            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("DisplayName", secureStorage.GetFromSecureStorage(Constants.DisplayName));


            string parsedUrl = new Helper().GetCompleteUrl(url, parameters);
            string response;

            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

                HttpResponseMessage resp = await httpClient.GetAsync(parsedUrl).ConfigureAwait(false);
                response = await resp.Content.ReadAsStringAsync();
            }

            T res = JsonConvert.DeserializeObject<T>(response);
            return await Task.FromResult(res);
        }


        public async Task<T> Add(string url, T obj)
        {
            string response;

            using (var httpClient = new HttpClient())
            {
                Uri parsedUrl = new Uri(url);
                var payload = JsonConvert.SerializeObject(obj);

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                HttpResponseMessage resp = await httpClient.PostAsync(parsedUrl, content).ConfigureAwait(false);
                response = await resp.Content.ReadAsStringAsync();
            }

            T res = JsonConvert.DeserializeObject<T>(response);

            return await Task.FromResult(res);
        }

        public async void Delete(string url, int id)
        {

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("id", id.ToString());
            string parsedUrl = new Helper().GetCompleteUrl(url, parameters);
            string response;

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage resp = await httpClient.DeleteAsync(parsedUrl).ConfigureAwait(false);
                response = await resp.Content.ReadAsStringAsync();
            }
        }
    }
}
