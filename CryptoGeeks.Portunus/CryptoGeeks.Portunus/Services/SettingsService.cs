using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Services
{
    public static class SettingsService
    {
        public static string GetAPIBaseUrl()
        {
            return "https://portunus.azurewebsites.net/api";
            // return "http://localhost:59423/api";
            // return "http://10.0.2.2:59423/api";

        }

        #region Users
        public static string GetAllUsersUrl()
        {
            return GetAPIBaseUrl() + "/Users/GetUsers";
        }

        public static string GetUserByName()
        {
            return GetAPIBaseUrl() + "/Users/GetUserByName";
        }


        public static string GetNewUser()
        {
            return GetAPIBaseUrl() + "/Users/GetNewUser";
        }


        

        public static string GetDashboard()
        {
            return GetAPIBaseUrl() + "/Users/GetDashboard";
        }


        public static string GetUserDetailsByName()
        {
            return GetAPIBaseUrl() + "/Users/GetUserDetailsByName";
        }

        public static string GetUserUrl()
        {
            return GetAPIBaseUrl() + "/Users/GetUser";
        }

        public static string PostUserUrl()
        {
            return GetAPIBaseUrl() + "/Users/PostUser";
        }

        public static string DeleteUserUrl()
        {
            return GetAPIBaseUrl() + "/Users/DeleteUser";
        }


        


        public static string GetNewDisplayName()
        {
            return GetAPIBaseUrl() + "/Users/GetNewUserName";
        }


        #endregion

        #region Keys
        public static string GetKeyUrl()
        {
            return GetAPIBaseUrl() + "/Keys/GetKey";
        }

        public static string GetKeysForUser()
        {
            return GetAPIBaseUrl() + "/Keys/GetKeysForUser";
        }

        public static string PostKeyUrl()
        {
            return GetAPIBaseUrl() + "/Keys/PostKey";
        }

        public static string DeleteKeyUrl()
        {
            return GetAPIBaseUrl() + "/Keys/DeleteKey";
        }
        #endregion

        #region Contacts
        public static string GetAllForUserUrl()
        {
            return GetAPIBaseUrl() + "/Contacts/GetContactsForUser";
        }

        public static string GetContactUrl()
        {
            return GetAPIBaseUrl() + "/Contacts/GetContact";
        }

        public static string GetContactsForUserUrl()
        {
            return GetAPIBaseUrl() + "/Contacts/GetContactsForUser";
        }

        public static string GetContactsCountForUser()
        {
            return GetAPIBaseUrl() + "/Contacts/GetContactsCountForUser";
        }


        
        public static string PostContactUrl()
        {
            return GetAPIBaseUrl() + "/Contacts/PostContact";
        }

        public static string DeleteContactUrl()
        {
            return GetAPIBaseUrl() + "/Contacts/DeleteContact";
        }

        public static string GetAvailableContactsForUser()
        {
            return GetAPIBaseUrl() + "/Contacts/GetAvailableContactsForUser";
        }

        #endregion

        #region Countries
        public static string GetCountryUrl()
        {
            return GetAPIBaseUrl() + "/Countries/GetCountry";
        }

        public static string GetAllCountries()
        {
            return GetAPIBaseUrl() + "/Countries/GetCountries";
        }

        public static string SearchCountries()
        {
            return GetAPIBaseUrl() + "/Countries/GetCountryByName";
        }
        #endregion

        #region Fragments
        public static string GetAllFragmentsForKeyUrl()
        {
            return GetAPIBaseUrl() + "/Fragments/GetFragmentsForKey";
        }

        public static string PostFragmentUrl()
        {
            return GetAPIBaseUrl() + "/Fragments/PostFragment";
        }

        public static string GetAllFragmentsForUserUrl()
        {
            return GetAPIBaseUrl() + "/Fragments/GetFragmentsForUser";
        }
        #endregion

        #region KeyRequests
        public static string GetKeyRequestUrl()
        {
            return GetAPIBaseUrl() + "/KeyRequests/GetKeyRequest";
        }

        public static string GetKeyRequestsForKeyUrl()
        {
            return GetAPIBaseUrl() + "/KeyRequests/GetKeyRequestsForKey";
        }

        public static string GetKeyRequestsForUserUrl()
        {
            return GetAPIBaseUrl() + "/KeyRequests/GetKeyRequestsForUser";
        }


        public static string GetPendingFragmentsForUserUrl()
        {
            return GetAPIBaseUrl() + "/KeyRequests/GetFragmentsForUser";
        }

        public static string GetFragmentsForUserUrl()
        {
            return GetAPIBaseUrl() + "/KeyRequests/GetFragmentsForUser";
        }

        public static string MarkFragmentAsSent()
        {
            return GetAPIBaseUrl() + "/KeyRequests/GetFragmentAsSent";
        }

        public static string MarkReqyestComplete()
        {
            return GetAPIBaseUrl() + "/KeyRequests/GetMarkReqyestComplete";
        }

        

        public static string PostKeyRequestUrl()
        {
            return GetAPIBaseUrl() + "/KeyRequests/PostKeyRequest";
        }

        public static string DeleteKeyRequestUrl()
        {
            return GetAPIBaseUrl() + "/KeyRequests/DeleteKeyRequest";
        }
        #endregion

        #region Pings
     
        public static string PostPing()
        {
            return GetAPIBaseUrl() + "/Pings/PostPing";
        }

        #endregion

        #region UserKeyFragment
        public static string GetAllUserKeyFragmentUrl()
        {
            return GetAPIBaseUrl() + "/UserKeyFragment/GetUserKeyFragment";
        }

        public static string GetUserKeyFragments()
        {
            return GetAPIBaseUrl() + "/UserKeyFragment/GetUserKeyFragments";
        }

        public static string GetUserOwnerFragments()
        {
            return GetAPIBaseUrl() + "/UserKeyFragment/GetUserOwnerFragments";
        }

        public static string GetUserRecepientFragments()
        {
            return GetAPIBaseUrl() + "/UserKeyFragment/GetUserRecepientFragments";
        }

        public static string GetUserKeyFragmentUrl()
        {
            return GetAPIBaseUrl() + "/UserKeyFragment/GetUserKeyFragment";
        }

        public static string PostUserKeyFragmentUrl()
        {
            return GetAPIBaseUrl() + "/UserKeyFragment/PostUserKeyFragment";
        }

        public static string DeleteUserKeyFragmentUrl()
        {
            return GetAPIBaseUrl() + "/UserKeyFragment/DeleteUserKeyFragment";
        }


        #endregion

        #region Pings

        public static string GetUserStatus()
        {
            return GetAPIBaseUrl() + "/UserStatusCompact/GetUserStatus";
        }

        #endregion
    }
}
