using CryptoGeeks.API;
using CryptoGeeks.Portunus.Helpers;
//using CryptoGeeks.Portunus.Models;
using CryptoGeeks.Portunus.Services;
using CryptoGeeks.Portunus.Services.POCO;
using CryptoGeeks.Portunus.ViewModels;
using CryptoGeeks.Portunus.Views.AddKey;
using CryptoGeeks.Portunus.Views.ExportImport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Fragments : ContentPage, INotifyPropertyChanged
    {
        KeysService keysService = new KeysService();
        ObservableCollection<Fragment> keyRequests;

        private bool _refreshing;
        public bool IsRefreshing
        {
            get { return _refreshing; }
            set { this._refreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

        private async Task<ObservableCollection<Fragment>> LoadUsersData()
        {

            EntityService<ObservableCollection<Fragment>> entityService = new EntityService<ObservableCollection<Fragment>>();
            NameValueCollection parameters = new NameValueCollection();
            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("userId", secureStorage.GetFromSecureStorage(Constants.UserId));

            return await entityService.Get(SettingsService.GetAllFragmentsForUserUrl(), parameters);

        }

        public async void BindData()
        {
            BindingContext = null;
            KeysListView.ItemsSource = null;

            keyRequests = await LoadUsersData();
            BindingContext = keyRequests;
            KeysListView.ItemsSource = keyRequests; ;

        }

        public Fragments()
        {
            try
            {
                InitializeComponent();
                keyRequests = new ObservableCollection<Fragment>();
                BindData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void KeyList_Appearing(object sender, EventArgs e)
        {
            try
            {
                base.OnAppearing();

                BindData();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushModalAsync(new NavigationPage(new AddEntry()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private async void KeysListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        private async void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            BindData();
        }
    }
}