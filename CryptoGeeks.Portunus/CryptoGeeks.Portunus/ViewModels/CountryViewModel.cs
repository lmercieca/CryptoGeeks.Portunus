using CryptoGeeks.Portunus.Api.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CryptoGeeks.Portunus.Api.Model
{

    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso2 { get; set; }
        public string CallingCode { get; set; }
    }

    public class CountryViewModel : INotifyPropertyChanged
    {
        private Country country;


        public Country Country
        {
            get { return country; }
        }

        public string Name
        {
            get { return country.Name; }
            set
            {
                country.Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string CallingCode
        {
            get { return country.CallingCode; }
            set
            {
                country.CallingCode = value;
                NotifyPropertyChanged("CallingCode");
            }
        }

        public CountryViewModel(Country country)
        {
            this.country = country;
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

