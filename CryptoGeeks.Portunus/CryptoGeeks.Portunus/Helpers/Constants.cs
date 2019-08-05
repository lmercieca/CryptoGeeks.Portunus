using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Helpers
{
    public class Constants
    {

        public const string DisplayName = "displayname";
        public static string OtherUsersFragmentsList = "otherPeopleFragmentsList";
        public static string KeysList = "keys";
        public static int DefaultServerPort = 11000;
        public static string Password = "salt";

        public static string ServerIP = "40.114.212.129";

        public static string BaseUrl = "https://portunus.azurewebsites.net/api";
        public static string CountryURL = BaseUrl +  "/countries";
        public static string ContactsURL = BaseUrl + "/users/GetUsers";
        public static string GetUserByNameURL = BaseUrl + "/users/GetUserByName";
        public static string AddDisplayNameURL = BaseUrl + "/users/AddDisplayName";

        public static string AddKeyURL = BaseUrl + "/keys/PostKey";
        public static string GetKeyURL = BaseUrl + "/keys/GetKeysForUser";



    }
}
