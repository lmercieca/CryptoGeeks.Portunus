using CryptoGeeks.Portunus.Api.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptoGeeks.Portunus.Api.Model
{

    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ContactViewModel : INotifyPropertyChanged
    {
        private Contact contact;


        public Contact Contact
        {
            get { return contact; }
        }

        public string Name
        {
            get { return contact.Name; }
            set
            {
                contact.Name = value;
                NotifyPropertyChanged("Name");
            }
        }

      

        public ContactViewModel(Contact contact)
        {
            this.contact = contact;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}

