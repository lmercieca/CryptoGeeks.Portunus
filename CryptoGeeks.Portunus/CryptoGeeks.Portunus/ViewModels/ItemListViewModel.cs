using CryptoGeeks.Portunus.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CryptoGeeks.Portunus.ViewModels
{
    public class ItemListViewModel : BaseViewModel
    {
        public ObservableCollection<Key> Keys { get; set; }
        public ObservableCollection<Contact> Contacts { get; set; }
        public ObservableCollection<Fragment> Fragments { get; set; }


        public ItemListViewModel()
        {
            this.Keys = new ObservableCollection<Key>();
            this.Contacts = new ObservableCollection<Contact>();
            this.Fragments = new ObservableCollection<Fragment>();

            Contact contact = new Contact() { DisplayName = "Loui" };

            Fragment fragment1 = new Fragment() { Data="Fragment 1", Owner= contact};
            Fragment fragment2 = new Fragment() { Data = "Fragment 2", Owner = contact };
            Fragment fragment3 = new Fragment() { Data = "Fragment 1", Owner = contact };

            Key key = new Key()
            {
                CreatedDate = DateTime.Now,
                Fragments = new List<Fragment>() { fragment1, fragment2, fragment3 }
            };

            //Just for tesing
            this.Keys.Add(key);
            this.Contacts.Add(contact);

           
        }
    }
}
