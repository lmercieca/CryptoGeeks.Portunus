using CryptoGeeks.API;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Services.POCO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.MultiSelectListView;

namespace CryptoGeeks.Portunus.Services
{
    public class ContactsService
    {
        public async Task<int> GetContactsCount()
        {
            return await Task.FromResult(1);
        }

        public async Task<MultiSelectObservableCollection<GetContactsForUser_Result>> GetContactsForUser()
        {
            EntityService<ObservableCollection<GetContactsForUser_Result>> entityService = new EntityService<ObservableCollection<GetContactsForUser_Result>>();
            NameValueCollection parameters = new NameValueCollection();

            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("userId", secureStorage.GetFromSecureStorage(Constants.UserId));
            ObservableCollection<GetContactsForUser_Result> result = await entityService.Get(SettingsService.GetAllForUserUrl(), parameters);

            MultiSelectObservableCollection<GetContactsForUser_Result> finalRes = new MultiSelectObservableCollection<GetContactsForUser_Result>();

            foreach(GetContactsForUser_Result re in result)
            {
                finalRes.Add(re);
            }

            return finalRes;


        }
    }
}
