using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.ViewModels;
using System;
using System.Collections.Generic;
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
        ItemListViewModel itemListViewModel;


        public AddContact()
        {
            InitializeComponent();

            itemListViewModel = new ItemListViewModel();

            this.Appearing += AddContact_Appearing;

            BindingContext = itemListViewModel;

            btnDone.IsVisible = false;
        }

        private async void AddContact_Appearing(object sender, EventArgs e)
        {
            base.OnAppearing();

            await LoadData();
        }

        private async Task<string> LoadData()
        {
            await itemListViewModel.RefreshData();


            Device.BeginInvokeOnMainThread(() =>
            {
                ContactsListView.BeginRefresh();

                ContactsListView.ItemsSource = null;
                ContactsListView.ItemsSource = itemListViewModel.Contacts;
                ContactsListView.EndRefresh();

                btnDone.IsVisible = true;

            });


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
                ContactsListView.ItemsSource = itemListViewModel.Contacts;

                if (itemListViewModel.Contacts.Count() == 0)
                    btnDone.IsVisible = false;
                else

                    btnDone.IsVisible = true;
            }
            else
            {
                // ContactsListView.ItemsSource = _container.Employees.Where(i => i.Name.Contains(e.NewTextValue));                
                var result = itemListViewModel.Contacts.Where(i => i.Data.DisplayName.ToLower().Contains(e.NewTextValue.ToLower()));
                ContactsListView.ItemsSource = result;

                if (result.Count() == 0)
                    btnDone.IsVisible = false;
                else
                    btnDone.IsVisible = true;

            }

            ContactsListView.EndRefresh();
        }
    }


}