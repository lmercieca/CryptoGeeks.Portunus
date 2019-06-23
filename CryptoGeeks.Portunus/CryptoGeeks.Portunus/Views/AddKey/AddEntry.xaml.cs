using CryptoGeeks.Portunus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.AddKey
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEntry : TabbedPage
    {
        ItemListViewModel itemListViewModel;


        public AddEntry ()
        {
            InitializeComponent();

            itemListViewModel = new ItemListViewModel();

            this.Appearing += AddContact_Appearing;

            BindingContext = itemListViewModel;

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

        private void BtnNext_Clicked(object sender, EventArgs e)
        {
            this.CurrentPage = this.Children[1];
        }
    }
}