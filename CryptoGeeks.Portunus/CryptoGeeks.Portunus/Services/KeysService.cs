using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using CryptoGeeks.API;
using CryptoGeeks.Portunus.Helpers;

namespace CryptoGeeks.Portunus.Services
{
    public class KeysService
    {
        

        public async Task<List<Key>> GetKeysForUser()
        {
            EntityService<List<Key>> entityService = new EntityService<List<Key>>();
            NameValueCollection parameters = new NameValueCollection();

            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("UserId", secureStorage.GetFromSecureStorage(Constants.UserId));
            return await entityService.Get(SettingsService.GetKeysForUser(), parameters);

        }
    }
}
