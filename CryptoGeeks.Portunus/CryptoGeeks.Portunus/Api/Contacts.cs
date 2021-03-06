﻿using CryptoGeeks.Portunus.Api.Model;
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
using CryptoGeeks.API;
using CryptoGeeks.Portunus.Services;
using System.Collections.Specialized;

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

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("displayName", displayName);


            bool exist = await new EntityService<bool>().Get(Constants.GetUserByNameURL, parameters);
            return exist;
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

        public async Task<string> UpdateDisplayName(string id, string displayName)
        {

            EntityService<User> userService = new EntityService<User>();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("displayName", displayName);
            parameters.Add("userId", id);


            await new EntityService<User>().GetWithNoReturn(SettingsService.GetNewDisplayName(), parameters);

            
            return await Task.FromResult("");
        }
    }
}
