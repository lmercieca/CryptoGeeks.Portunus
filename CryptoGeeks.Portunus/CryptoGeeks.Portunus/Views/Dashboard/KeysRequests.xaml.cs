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
    public partial class KeysRequests : ContentPage, INotifyPropertyChanged
    {
        KeysService keysService = new KeysService();
        List<GetKeyRequests_Result> keys;

        private bool _refreshing;
        public bool IsRefreshing
        {
            get { return _refreshing; }
            set { this._refreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

        private async Task<List<GetKeyRequests_Result>> LoadUsersData()
        {

            EntityService<List<GetKeyRequests_Result>> entityService = new EntityService<List<GetKeyRequests_Result>>();
            NameValueCollection parameters = new NameValueCollection();
            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("userId", secureStorage.GetFromSecureStorage(Constants.UserId));

            return await entityService.Get(SettingsService.GetKeyRequestsForUserUrl(), parameters);

        }

        public async void BindData()
        {
            BindingContext = null;
            KeysListView.ItemsSource = null;

            keys = await LoadUsersData();
            BindingContext = keys;
            KeysListView.ItemsSource = keys; 




        }

        public KeysRequests()
        {
            try
            {
                InitializeComponent();
                keys = new List<GetKeyRequests_Result>();
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
                GetKeyRequests_Result k = e.SelectedItem as GetKeyRequests_Result;

                EntityService<Key> keyService = new EntityService<Key>();

                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("id", k.Id.ToString());


               Key key =  await  keyService.Get(SettingsService.GetKeyUrl(), parameters);

                await Navigation.PushModalAsync(new NavigationPage(new KeyDetails(key)), true);


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


                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    KeysListView.ItemsSource = await keysService.GetKeysForUser();
                }
                else
                {
                    // ContactsListView.ItemsSource = _container.Employees.Where(i => i.Name.Contains(e.NewTextValue));                
                    var result = keys.Where(i => i.DisplayName.ToLower().Contains(e.NewTextValue.ToLower()));
                    KeysListView.ItemsSource = result;


                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void BtnExport_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new Export()), true);

        }

        public void FileWriter(string data) // Code to generate a text file
        {

            var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "keys.ptn");

            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.Write(data);
                sw.Close();
            }

        }


        public async Task SendEmail(string subject, string body)
        {
            try
            {
                var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                var filename = Path.Combine(documents, "keys.ptn");


                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    Attachments = new List<EmailAttachment>()
                    {
                        new EmailAttachment(filename)
                    }
                };

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device  
            }
            catch (Exception ex)
            {
                // Some other exception occurred  
            }
        }

        private async void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            BindData();
        }
    }
}