using CryptoGeeks.Portunus.Models;
using CryptoGeeks.Portunus.ViewModels;
using CryptoGeeks.Portunus.Views.AddKey;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Keys : ContentPage
    {
        ItemListViewModel itemListViewModel;

        public Keys()
        {
            try
            {
                InitializeComponent();


                itemListViewModel = new ItemListViewModel();
                BindingContext = itemListViewModel;
                this.Appearing += KeyList_Appearing;
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
                itemListViewModel = new ItemListViewModel();

                await LoadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<string> LoadData()
        {
            try
            {

                await itemListViewModel.RefreshKeys();


                Device.BeginInvokeOnMainThread(() =>
                {
                    KeysListView.BeginRefresh();

                    KeysListView.ItemsSource = null;
                    KeysListView.ItemsSource = itemListViewModel.Keys;
                    KeysListView.EndRefresh();


                });



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult("");
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
                Key k = e.SelectedItem as Key;

                await Navigation.PushModalAsync(new NavigationPage(new KeyDetails(k)), true);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                KeysListView.BeginRefresh();

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    KeysListView.ItemsSource = itemListViewModel.Keys;
                }
                else
                {
                    // ContactsListView.ItemsSource = _container.Employees.Where(i => i.Name.Contains(e.NewTextValue));                
                    var result = itemListViewModel.Keys.Where(i => i.DisplayName.ToLower().Contains(e.NewTextValue.ToLower()));
                    KeysListView.ItemsSource = result;


                }

                KeysListView.EndRefresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void BtnExport_Clicked(object sender, EventArgs e)
        {
            FileWriter("This is a test");
            await SendEmail("Test", "This is a test");
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
    }
}