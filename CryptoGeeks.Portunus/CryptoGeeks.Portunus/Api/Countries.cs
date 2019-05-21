using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Api
{
    public class Countries
    {
        HttpClient _client;

        public async Task<List<CountryView>> RefreshDataAsync()
        {

            var uri = new Uri(string.Format(Constants.CountryURL, string.Empty));
            var response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Items = JsonConvert.DeserializeObject<List<CountryView>>(content);

                return Items;
            }

            return new List<CountryView>();
        }
    }
}
