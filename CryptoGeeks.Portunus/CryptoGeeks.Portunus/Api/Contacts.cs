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
using CryptoGeeks.Portunus.Models;
using Refit;
using Xamarin.Forms.MultiSelectListView;
using System.Web;

namespace CryptoGeeks.Portunus.Api
{

    public class ContactService
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


        public async Task<MultiSelectObservableCollection<ContactViewModel>> DeserializeOptimizedFromStreamCallAsync(CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Constants.CountryURL))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<MultiSelectObservableCollection<ContactViewModel>>(stream);

                var content = await StreamToStringAsync(stream);
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }


        public async Task<MultiSelectObservableCollection<ContactViewModel>> RefreshDataAsync()
        {
            var httpClient = new HttpClient();
            string text = await httpClient.GetStringAsync(Constants.ContactsURL).ConfigureAwait(false);
            List<Contact> results = JsonConvert.DeserializeObject<List<Contact>>(text);
            var query = from x in results select new ContactViewModel(x);
            var Items = new MultiSelectObservableCollection<ContactViewModel>(query.ToList());

            return Items;
        }


        public async Task<bool> DisplayNameExists(string displayName)
        {
            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            {
                ClientHandler.AllowAutoRedirect = true;
                ClientHandler.UseDefaultCredentials = true;


                using (HttpClient Client = new HttpClient(ClientHandler))
                {

                    var builder = new UriBuilder(Constants.GetUserByNameURL);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["displayname"] = displayName;
                    builder.Query = query.ToString();
                    string url = builder.ToString();


                    using (HttpResponseMessage ResponseMessage = await Client.GetAsync(url))
                    {
                        using (HttpContent Content = ResponseMessage.Content)
                        {
                            string result = await Content.ReadAsStringAsync();


                            bool exist = JsonConvert.DeserializeObject<bool>(result);
                            return exist;
                        }
                    }
                }
            }
        }

        public async Task<int> AddDisplayName(string displayName)
        {
            int id = -1;

            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            {
                ClientHandler.AllowAutoRedirect = true;
                ClientHandler.UseDefaultCredentials = true;


                using (HttpClient Client = new HttpClient(ClientHandler))
                {

                    var builder = new UriBuilder(Constants.AddDisplayNameURL);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    query["displayname"] = displayName;
                    builder.Query = query.ToString();
                    string url = builder.ToString();


                    using (HttpResponseMessage ResponseMessage = await Client.GetAsync(url))
                    {
                        if ( ResponseMessage.IsSuccessStatusCode)
                        {
                            id = int.Parse(ResponseMessage.Content.ReadAsStringAsync().Result);
                        }

                    }
                }
            }

            return await Task.FromResult(id);
        }
    }
}
