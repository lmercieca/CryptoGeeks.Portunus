using CryptoGeeks.Portunus.Api;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Diagnostics;
using System.Threading;

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

                Countries countriesApi = new Countries();

                var apiResult = countriesApi.RefreshDataAsync().Result;

                Country.ItemsSource = apiResult;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }
    }
}
