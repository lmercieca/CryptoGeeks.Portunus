using System;

namespace CryptoGeeks.Common
{
    public class Workflow
    {

        public bool isUserRegistered()
        {
            SecureStorage secureStorage = new SecureStorage();
            return secureStorage.KeyExist(SecureStorage.SecureStorageKeys.IsUserRegistered.ToString());
        }
    }
}
