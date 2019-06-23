using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Helpers
{
    public class Constants
    {

        public const string DisplayName = "displayname";



        public static string BaseUrl = "https://portunusapi.azurewebsites.net/api";
        public static string CountryURL = BaseUrl +  "/countries";
        public static string ContactsURL = BaseUrl + "/users/GetUsers";
        public static string GetUserByNameURL = BaseUrl + "/users/GetUserByName";
        public static string AddDisplayNameURL = BaseUrl + "/users/AddDisplayName";
    }
}
