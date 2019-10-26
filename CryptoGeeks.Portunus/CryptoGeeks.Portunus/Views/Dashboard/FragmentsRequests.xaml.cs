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
    public partial class FragmentsRequests : ContentPage, INotifyPropertyChanged
    {
        KeysService keysService = new KeysService();
        ObservableCollection<GetKeyFragmentRequests_Result> keyRequests;

        private bool _refreshing;
        public bool IsRefreshing
        {
            get { return _refreshing; }
            set { this._refreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

        private async Task<ObservableCollection<GetKeyFragmentRequests_Result>> LoadUsersData()
        {

            EntityService<ObservableCollection<GetKeyFragmentRequests_Result>> entityService = new EntityService<ObservableCollection<GetKeyFragmentRequests_Result>>();
            NameValueCollection parameters = new NameValueCollection();
            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("userId", secureStorage.GetFromSecureStorage(Constants.UserId));

            return await entityService.Get(SettingsService.GetPendingFragmentsForUserUrl(), parameters);

        }

        public async void BindData()
        {
            BindingContext = null;
            KeysListView.ItemsSource = null;

            keyRequests = await LoadUsersData();
            BindingContext = keyRequests;
            KeysListView.ItemsSource = keyRequests; ;

        }

        public FragmentsRequests()
        {
            try
            {
                InitializeComponent();
                keyRequests = new ObservableCollection<GetKeyFragmentRequests_Result>();
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

        

        private async void KeysListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                GetKeyFragmentRequests_Result k = e.SelectedItem as GetKeyFragmentRequests_Result;

                EntityService<Fragment> fragService = new EntityService<Fragment>();
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("fragmentId", k.FragmentId.ToString());
                await fragService.Get(SettingsService.MarkFragmentAsSent(), parameters);

                BindData();
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