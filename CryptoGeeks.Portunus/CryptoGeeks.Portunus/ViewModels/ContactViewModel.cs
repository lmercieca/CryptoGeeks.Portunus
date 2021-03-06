﻿using CryptoGeeks.API;
using CryptoGeeks.Portunus.Api.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptoGeeks.Portunus.Api.Model
{

    public class ContactViewModel : INotifyPropertyChanged
    {
        private Contact contact;


        public Contact Contact
        {
            get { return contact; }
        }


        public ContactViewModel(Contact contact)
        {
            this.contact = contact;
        }

        public bool Selected { get; set; }

        public string DisplayName { get { return this.contact.User.DisplayName; } }
        public int Id { get { return this.contact.User.Id; } }


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

