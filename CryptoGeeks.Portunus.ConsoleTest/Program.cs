using CryptoGeeks.Portunus.Api;
using CryptoGeeks.Portunus.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var countries =  GetCountries();
            countries.Wait();

            var x = countries.Result;

        }

        public static async Task<List<Country>> GetCountries()
        {

            Countries countriesApi = new Countries();

            var countries = await countriesApi.RefreshDataAsync();

            return countries;
        }
    }
}
