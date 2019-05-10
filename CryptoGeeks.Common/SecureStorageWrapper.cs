using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace CryptoGeeks.Common
{
    public class SecureStorage
    {
        public enum SecureStorageKeys
        {
           IsUserRegistered,
           Name,
           Surname,
           DateOfBirth,
           Email,
           Country,
           CountryCode,
           Phone,
           SecurityQuestion,
           SecurityAnswer,
           Passcode
        }

        public string GetFromSecureStorage(string key)
        {
            var res = Xamarin.Essentials.SecureStorage.GetAsync(key).Result;
            return res;            
        }

        public async void StoreInSecureStorage(string key, string value)
        {
           await Xamarin.Essentials.SecureStorage.SetAsync(key, value);            
        }

        public bool KeyExist(string key)
        {
            string val = GetFromSecureStorage(key);

            return string.IsNullOrEmpty(val.Trim());
        }
    }

}
