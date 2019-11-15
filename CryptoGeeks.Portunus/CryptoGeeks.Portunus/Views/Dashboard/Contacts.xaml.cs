using CryptoGeeks.Portunus.Comm;
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
    public partial class Contacts : ContentPage
    {
        ObservableCollection<GetContactsForUser_Result> contacts;


        private bool _refreshing;
        public bool IsRefreshing
        {
            get { return _refreshing; }
            set { this._refreshing = value; OnPropertyChanged("IsRefreshing"); }
        }


        public async Task<ObservableCollection<GetContactsForUser_Result>> GetContacts()
        {
            EntityService<ObservableCollection<GetContactsForUser_Result>> entityService = new EntityService<ObservableCollection<GetContactsForUser_Result>>();
            NameValueCollection parameters = new NameValueCollection();

            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("userId", secureStorage.GetFromSecureStorage(Constants.UserId));
            ObservableCollection<GetContactsForUser_Result> result = await entityService.Get(SettingsService.GetAllForUserUrl(), parameters);

            return result;
        }

        public async Task<string> BindData()
        {
            BindingContext = null;
            KeysListView.ItemsSource = null;

            contacts = await GetContacts();
            BindingContext = contacts;
            KeysListView.ItemsSource = contacts; ;

            return await Task.FromResult("");

        }
        public Contacts()
        {
            try
            {
                InitializeComponent();

                this.Appearing += Contacts_Appearing;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void Contacts_Appearing(object sender, EventArgs e)
        {
            await BindData();
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushModalAsync(new NavigationPage(new AddContact.AddContact()));
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
                //Key k = e.SelectedItem as Key;

                //await Navigation.PushModalAsync(new NavigationPage(new KeyDetails(k)), true);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                KeysListView.BeginRefresh();

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    contacts = await GetContacts();
                    KeysListView.ItemsSource = contacts;
                }
                else
                {
                    // ContactsListView.ItemsSource = _container.Employees.Where(i => i.Name.Contains(e.NewTextValue));                
                    ObservableCollection<GetContactsForUser_Result> result = contacts.Where(i => i.DisplayName.ToLower().Contains(e.NewTextValue.ToLower())).ToObservableCollection();
                    KeysListView.ItemsSource = result;
                }

                KeysListView.EndRefresh();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ImgBtnRemove_Clicked(object sender, EventArgs e)
        {
            KeysListView.BeginRefresh();


            GetContactsForUser_Result cnt = ((ImageButton)sender).BindingContext as GetContactsForUser_Result;

            EntityService<GetContactsForUser_Result> service = new EntityService<GetContactsForUser_Result>();
            service.Delete(SettingsService.DeleteContactUrl(), cnt.ID);

            contacts = KeysListView.ItemsSource as ObservableCollection<GetContactsForUser_Result>;
            contacts.Remove(cnt);
         

         
            KeysListView.ItemsSource = contacts; ;
           // BindData();
            KeysListView.EndRefresh();
        }

        private void BtnAddContacts_Clicked(object sender, EventArgs e)
        {

            Navigation.PushModalAsync(new NavigationPage(new AddContact.AddContact()));
        }
    }
}