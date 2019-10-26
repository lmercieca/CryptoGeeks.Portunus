using CryptoGeeks.API;
using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Services;
using CryptoGeeks.Portunus.Services.POCO;
using CryptoGeeks.Portunus.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CryptoGeeks.Portunus.Views.AddContact
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddContact : ContentPage
    {
        ObservableCollection<GetAvailableContactsForUser_Result> contacts;

        public AddContact()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            BindData();

            btnDone.IsVisible = false;
        }

        private async Task<ObservableCollection<GetAvailableContactsForUser_Result>> LoadUsersData()
        {

            EntityService<ObservableCollection<GetAvailableContactsForUser_Result>> entityService = new EntityService<ObservableCollection<GetAvailableContactsForUser_Result>>();
            NameValueCollection parameters = new NameValueCollection();
            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("userId", secureStorage.GetFromSecureStorage(Constants.UserId));

            return await entityService.Get(SettingsService.GetAvailableContactsForUser(), parameters);

        }

        public async void BindData()
        {
            BindingContext = null;
            ContactsListView.ItemsSource = null;

            contacts = await LoadUsersData();
            BindingContext = contacts;
            ContactsListView.ItemsSource = contacts; ;

        }




        private void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ContactViewModel model = e.SelectedItem as ContactViewModel;
            model.Selected = !model.Selected;
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ContactsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                ContactsListView.ItemsSource = contacts;

                if (contacts.Count() == 0)
                    btnDone.IsVisible = false;
                else

                    btnDone.IsVisible = true;
            }
            else
            {
                // ContactsListView.ItemsSource = _container.Employees.Where(i => i.Name.Contains(e.NewTextValue));                
                var result = contacts.Where(i => i.DisplayName.ToLower().Contains(e.NewTextValue.ToLower()));
                ContactsListView.ItemsSource = result;

                if (result.Count() == 0)
                    btnDone.IsVisible = false;
                else
                    btnDone.IsVisible = true;

            }

            ContactsListView.EndRefresh();
        }

        private async void ImgBtnAdd_Clicked(object sender, EventArgs e)
        {
            GetAvailableContactsForUser_Result cnt = ((ImageButton)sender).BindingContext as GetAvailableContactsForUser_Result;


            EntityService<Contact> entityService = new EntityService<Contact>();
            SecureStorage secureStorage = new SecureStorage();

            Contact contact = new Contact()
            {
                UserID = int.Parse(secureStorage.GetFromSecureStorage(Constants.UserId)),
                ContactID = cnt.UserId
            };
            
            await entityService.Add(SettingsService.PostContactUrl(), contact);

            BindData();

        }

        private void BtnDone_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new NavigationPage(new CryptoGeeks.Portunus.Views.Dashboard.Dashboard()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }


}