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

        public async Task<string> BindData()
        {
            BindingContext = null;
            ContactsListView.ItemsSource = null;

            contacts = await LoadUsersData();
            BindingContext = contacts;
            ContactsListView.ItemsSource = contacts; ;

            return await Task.FromResult("");
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

                /*                if (contacts.Count() == 0)
                                    btnDone.IsVisible = false;
                                else

                                    btnDone.IsVisible = true;*/
            }
            else
            {
                // ContactsListView.ItemsSource = _container.Employees.Where(i => i.Name.Contains(e.NewTextValue));                
                var result = contacts.Where(i => i.DisplayName.ToLower().Contains(e.NewTextValue.ToLower())).ToObservableCollection();

                ContactsListView.ItemsSource = result;
                /*     if (result.Count() == 0)
                     btnDone.IsVisible = false;
                 else
                     btnDone.IsVisible = true;*/
            }

            ContactsListView.EndRefresh();

        }

        private async void ImgBtnAdd_Clicked(object sender, EventArgs e)
        {
            try
            {
                ContactsListView.BeginRefresh();

                GetAvailableContactsForUser_Result cnt = ((ImageButton)sender).BindingContext as GetAvailableContactsForUser_Result;

                ContactsListView.BeginRefresh();

                contacts = ContactsListView.ItemsSource as ObservableCollection<GetAvailableContactsForUser_Result>;

                contacts.Remove(cnt);
                //  ContactsListView.EndRefresh();

                ContactsListView.ItemsSource = contacts; ;


                await AddNewContact(cnt);



                //await BindData();
                ContactsListView.EndRefresh();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task<string> AddNewContact(GetAvailableContactsForUser_Result cnt)
        {

            EntityService<Contact> entityService = new EntityService<Contact>();
            SecureStorage secureStorage = new SecureStorage();

            Contact contact = new Contact()
            {
                UserID = int.Parse(secureStorage.GetFromSecureStorage(Constants.UserId)),
                ContactID = cnt.UserId
            };

            await entityService.Add(SettingsService.PostContactUrl(), contact);


            return await Task.FromResult("");
        }

        private void BtnDone_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void ContactsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                GetAvailableContactsForUser_Result cnt = e.SelectedItem as GetAvailableContactsForUser_Result;

                await AddNewContact(cnt);


                await BindData();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }


}