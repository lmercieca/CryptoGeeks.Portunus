using CryptoGeeks.Portunus.Api;
using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.MultiSelectListView;

namespace CryptoGeeks.Portunus.ViewModels
{
    public class ItemListViewModel : BaseViewModel
    {
        public ObservableCollection<Key> Keys { get; set; }
        public MultiSelectObservableCollection<ContactViewModel> Contacts { get; set; }
        public MultiSelectObservableCollection<Fragment> Fragments { get; set; }


        public ItemListViewModel()
        {

           
            this.Keys = new ObservableCollection<Key>();
            this.Contacts = new MultiSelectObservableCollection<ContactViewModel>();
            this.Fragments = new MultiSelectObservableCollection<Fragment>();

            Contact contact = new  Contact() {  DisplayName = "Loading.." };
            ContactViewModel cvm = new ContactViewModel(contact);

            Fragment fragment1 = new Fragment() { Data="Fragment 1", Owner= contact};
            Fragment fragment2 = new Fragment() { Data = "Fragment 2", Owner = contact };
            Fragment fragment3 = new Fragment() { Data = "Fragment 1", Owner = contact };

            Key key = new Key()
            {
                CreatedDate = DateTime.Now,
                Fragments = new List<Fragment>() { fragment1, fragment2, fragment3 },
                RequiredFragments = 2,
                DisplayName = "Loading"
                
            };

            //Just for tesing
            this.Keys.Add(key);
            this.Contacts.Add(cvm);

           
        }

        public async Task<string> RefreshData()
        {
            ContactService contactService = new ContactService();
            var res = await contactService.RefreshDataAsync();

            this.Contacts = res;

            return await Task.FromResult("");
        }

        public async Task<string> RefreshKeys()
        {
            Contact contact = new Contact() { DisplayName = "Loading.." };


            Fragment fragment1 = new Fragment() { Data = "Fragment 1", Owner = contact };
            Fragment fragment2 = new Fragment() { Data = "Fragment 2", Owner = contact };
            Fragment fragment3 = new Fragment() { Data = "Fragment 1", Owner = contact };

            Key key = new Key()
            {
                CreatedDate = DateTime.Now,
                Fragments = new List<Fragment>() { fragment1, fragment2, fragment3 },
                RequiredFragments = 2,
                DisplayName = "Added"

            };

            //Just for tesing
            this.Keys.Add(key);


            return await Task.FromResult("");
        }
    }
}
