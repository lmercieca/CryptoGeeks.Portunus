using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CryptoGeeks.Portunus.Models;

namespace CryptoGeeks.Portunus.Services
{
    public class KeysService
    {
        public async Task<int> GetKeysCount()
        {
            return await Task.FromResult(1);
        }

        public async Task<int> GetFragmentsCount()
        {
            return await Task.FromResult(1);
        }

        public async Task<List<Key>> GetKeysForUser()
        {
            return await Task.FromResult(new List<Key>()
            {
                new Key()
                {
                     CreatedDate = DateTime.Today,
                      Data = "123",
                       DisplayName = "Loui",
                        Identifier = Guid.NewGuid().ToString(),
                         RequiredFragments = 3


                }
            });
        }
    }
}
