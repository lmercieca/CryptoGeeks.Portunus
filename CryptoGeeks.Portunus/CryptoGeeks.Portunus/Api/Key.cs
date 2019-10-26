using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.IO;
using Refit;
using Xamarin.Forms.MultiSelectListView;
using System.Web;
using System.Net.Http.Headers;
using CryptoGeeks.API;

namespace CryptoGeeks.Portunus.Api
{

    public class KeyService
    {

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }



        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }


        public async Task<ObservableCollection<Key>> RefreshDataAsync(string displayName)
        {

            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            {
                ClientHandler.AllowAutoRedirect = true;
                ClientHandler.UseDefaultCredentials = true;


                using (HttpClient Client = new HttpClient(ClientHandler))
                {

                    var builder = new UriBuilder(Constants.GetKeyURL);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["displayname"] = displayName;
                    builder.Query = query.ToString();
                    string url = builder.ToString();


                    using (HttpResponseMessage ResponseMessage = await Client.GetAsync(url))
                    {
                        using (HttpContent Content = ResponseMessage.Content)
                        {
                            string result = await Content.ReadAsStringAsync();

                            ObservableCollection<Key> results = JsonConvert.DeserializeObject<ObservableCollection<Key>>(result);

                            var Items = results;

                            return Items;
                        }
                    }
                }
            }

            return new ObservableCollection<Key>();
        }


        public async Task<bool> AddKey(ApiKey key)
        {
            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            {
                ClientHandler.AllowAutoRedirect = true;
                ClientHandler.UseDefaultCredentials = true;
                

                using (HttpClient Client = new HttpClient(ClientHandler))
                {
                    var myContent = JsonConvert.SerializeObject(key);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                    await Client.PostAsync(Constants.AddKeyURL, byteContent);

                }
            }

            return await Task.FromResult(true);
        }
    }
}
