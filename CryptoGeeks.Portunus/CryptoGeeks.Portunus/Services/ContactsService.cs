using CryptoGeeks.Portunus.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CryptoGeeks.Portunus.Services
{
    public class ContactsService
    {
        public async Task<int> GetContactsCount()
        {
            return await Task.FromResult(1);
        }

        public async Task<List<Contact>> GetContactsForUser()
        {
            return await Task.FromResult(new List<Contact>()
            {
                new Contact()
                {
                     DisplayName = "Bob",
                      Id = 1
                }
            });
        }
    }
}
