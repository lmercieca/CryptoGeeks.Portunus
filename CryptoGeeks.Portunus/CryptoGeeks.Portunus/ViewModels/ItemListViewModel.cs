using CryptoGeeks.API;
using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Api;
using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.MultiSelectListView;

namespace CryptoGeeks.Portunus.ViewModels
{
    public class ItemListViewModel : BaseViewModel
    {
        public ObservableCollection<Key> Keys { get; set; }

        public MultiSelectObservableCollection<ContactViewModel> Contacts { get; set; }

        public ObservableCollection<Fragment> Fragments { get; set; }

        public User currentUser;

        public List<ContactViewModel> BaseContactList { get; set; }

        public ItemListViewModel()
        {           
            this.Keys = new ObservableCollection<Key>();
            this.Contacts = new MultiSelectObservableCollection<ContactViewModel>();
            this.Fragments = new ObservableCollection<Fragment>();           
        }

        public async Task<User> RefreshData()
        {
            NameValueCollection parameters = new NameValueCollection();
            SecureStorage secureStorage = new SecureStorage();

            string displayName = secureStorage.GetFromSecureStorage(Constants.DisplayName);

            parameters.Add("displayName", displayName);
            currentUser = await new EntityService<User>().Get(SettingsService.GetUserDetailsByName(), parameters);

            return currentUser;
        }

        public async Task

       
    }
}
