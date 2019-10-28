using CryptoGeeks.API;
using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Api;
using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Services;
using CryptoGeeks.Portunus.Services.POCO;
//using CryptoGeeks.Portunus.Models;
using CryptoGeeks.Portunus.ViewModels;
using Moserware.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.AddKey
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEntry : TabbedPage
    {
        //ItemListViewModel itemListViewModel;
        MultiSelectObservableCollection<GetContactsForUser_Result> contacts;
        User currentUser;

        public AddEntry(User currentUser)
        {
            InitializeComponent();

            ContactService cs = new ContactService();
            this.currentUser = currentUser;

            //itemListViewModel = new ItemListViewModel();

            this.Appearing += AddContact_Appearing;

            //BindingContext = itemListViewModel;

        }

        private async void AddContact_Appearing(object sender, EventArgs e)
        {
            base.OnAppearing();

            await LoadData();
        }

        private async Task<string> LoadData()
        {
            // await itemListViewModel.RefreshData();
            ContactsService cs = new ContactsService();

            Device.BeginInvokeOnMainThread(async () =>
           {
               contacts = await cs.GetContactsForUser();

               ContactsListView.BeginRefresh();

               ContactsListView.ItemsSource = null;
               ContactsListView.ItemsSource = contacts;
               ContactsListView.EndRefresh();
           });


            return await Task.FromResult("");
        }


        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ContactsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                // EmployeeListView.ItemsSource = _container.Employees;
            }
            else
            {
                // EmployeeListView.ItemsSource = _container.Employees.Where(i => i.Name.Contains(e.NewTextValue));
            }

            ContactsListView.EndRefresh();
        }

        private async void BtnNext_Clicked(object sender, EventArgs e)
        {
            if (int.Parse(txtRecoverNo.Text) > currentUser.Contacts.Count())
            {
                if (await DisplayAlert("Add key", "You only have " + currentUser.Contacts.Count() + " contacts linked. Would you like to add more contacts?.", "Yes", "No"))
                {
                    await Navigation.PushModalAsync(new AddContact.AddContact());
                }
                else
                {
                    await Navigation.PopModalAsync(true);
                }
            }

            this.CurrentPage = this.Children[1];
        }

        private async void BtnDone_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (contacts.Where(x => x.IsSelected).Count() < int.Parse(txtRecoverNo.Text))
                {
                    await DisplayAlert("Add key", "You need to have more contacts than the minimum number of pieces to recover the message.", "OK");

                    return;
                }

                SecureStorage secureStorage = new SecureStorage();
                string displayName = secureStorage.GetFromSecureStorage(Constants.DisplayName);
                int userId = int.Parse(secureStorage.GetFromSecureStorage(Constants.UserId));
                List<SelectableItem<GetContactsForUser_Result>> selectedContacts = contacts.Where(x => x.IsSelected).ToList();

                ApiKey apiKey = new ApiKey()
                {
                    Owner = displayName,

                    RecoverNo = int.Parse(txtRecoverNo.Text),
                    Key1 = txtDisplayName.Text,
                    Data = txtKey.Text,
                    Split = selectedContacts.Count(),
                    User = userId,
                    Fragments = new List<ApiFragment>()
                };

               
                string[] frags = SecretSplitter.SplitMessage(apiKey.Data, apiKey.RecoverNo.Value, apiKey.Split);
                int cnt = 0;

                foreach (SelectableItem<GetContactsForUser_Result> selContact in selectedContacts)
                {
                    apiKey.Fragments.Add(new ApiFragment()
                    {
                        FragmentHolder = selContact.Data.DisplayName,
                        Data = frags[cnt],
                        SentToHolder = true,
                        SentToOwner = false,
                        Owner = selContact.Data.ID
                    });

                    cnt++;
                }

                KeyService keyService = new KeyService();
                await keyService.AddKey(apiKey);


                await DisplayAlert("Add key", "Key successfully added.", "OK");
                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Add key error", ex.Message, "OK");

            }
        }
    }
}