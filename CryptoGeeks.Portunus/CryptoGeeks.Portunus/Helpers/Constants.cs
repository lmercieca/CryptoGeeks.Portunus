using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Helpers
{
    public class Constants
    {

        public const string DisplayName = "displayname";
        public const string UserId = "userid";
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
        public static string UpdateDisplayName = BaseUrl + "/users/GetNewDisplayName";

        public static string AddContact = BaseUrl + "/keys/PostKey";
        public static string RemoveContact = BaseUrl + "/Contacts/DeleteContact";


        public static string AddKeyURL = BaseUrl + "/keys/PostKey";
        public static string GetKeyURL = BaseUrl + "/keys/GetKeysForUser";
        public static string RemoveKeyURL = BaseUrl + "/keys/DeleteKey";

        public static string AddKeyRequest = BaseUrl + "/KeyRequests/PostKeyRequest";
        public static string GetKeyRequest = BaseUrl + "/KeyRequests/GetKeyRequest";

        public static string AddKeyFragmentRequest = BaseUrl + "/UserKeyFragments/PostUserKeyFragment";
        public static string GetKeyFragmentRequest = BaseUrl + "/UserKeyFragments/GetUserKeyFragment";


    }
}
