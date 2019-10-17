using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Services
{
    public class EntityService<T>
    {
        public async Task<T> Get(string url, NameValueCollection parameters)
        {
            string parsedUrl = new Helper().GetCompleteUrl(url, parameters);
            string response;

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage resp = await httpClient.GetAsync(parsedUrl).ConfigureAwait(false);
                response = await resp.Content.ReadAsStringAsync();
            }

            T res = JsonConvert.DeserializeObject<T>(response);
            return await Task.FromResult(res);
        }

        public async Task<T> Add(T obj)
        {
            string response;

            using (var httpClient = new HttpClient())
            {
                Uri parsedUrl = new Uri(SettingsService.PostUserUrl());
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
