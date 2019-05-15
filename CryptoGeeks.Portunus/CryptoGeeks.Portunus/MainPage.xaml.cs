using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CryptoGeeks.Portunus
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }


        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                List<string> countries = new List<string>() { "Malta", "Gozo" };
                Country.ItemsSource = countries;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }
    }
}
