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


        public async Task<ObservableCollection<ContactViewModel>> DeserializeOptimizedFromStreamCallAsync(CancellationToken cancellationToken)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, Constants.CountryURL))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<ObservableCollection<ContactViewModel>>(stream);

                var content = await StreamToStringAsync(stream);
                throw new ApiException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = content
                };
            }
        }


        public async Task<ObservableCollection<ContactViewModel>> RefreshDataAsync()
        {
            HttpClient _client = new HttpClient();


            var uri = new Uri(string.Format(Constants.CountryURL, string.Empty));
            var response = await _client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                List<Contact> result = JsonConvert.DeserializeObject<List<Contact>>(content);

                var query = from x in result select new ContactViewModel(x);

                var Items = new ObservableCollection<ContactViewModel>(query.ToList());

                return Items;
            }

            return new ObservableCollection<ContactViewModel>();
        }
    }
}
