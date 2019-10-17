using CryptoGeeks.Portunus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        KeysService keysService = new KeysService();
        ContactsService contactsService = new ContactsService();

        public Dashboard()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            btnKeys.Clicked += BtnKeys_Clicked;
            btnRequests.Clicked += BtnRequests_Clicked;
            btnConatcts.Clicked += BtnConatcts_Clicked;
            btnSettings.Clicked += BtnSettings_Clicked;

            UpdateKeys();
            UpdateFragments();
            UpdateContacts();           
           
        }


        private async void UpdateKeys()
        {
            int noOfKeys = await keysService.GetKeysCount();
            btnKeys.Text = "Keys " + Environment.NewLine + "(" + noOfKeys +")";
        }

        private async void UpdateFragments()
        {
            int noOfFrags = await keysService.GetFragmentsCount();
            btnRequests.Text = "Fragments " + Environment.NewLine + "(" + noOfFrags + ")";
        }

        private async void UpdateContacts()
        {
            int noOfContacts = await contactsService.GetContactsCount();
            btnConatcts.Text = "Contacts " + Environment.NewLine + "(" + noOfContacts + ")";
        }


        private void BtnKeys_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Keys());
        }

        private void BtnConatcts_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Contacts());
        }

        private void BtnRequests_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnSettings_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Settings());
        }
    }
}