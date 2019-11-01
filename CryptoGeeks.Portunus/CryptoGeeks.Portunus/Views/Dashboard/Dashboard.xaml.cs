using CryptoGeeks.API;
using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Services;
using CryptoGeeks.Portunus.Services.POCO;
using CryptoGeeks.Portunus.Views.Registration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
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
        SecureStorage secureStorage = new SecureStorage();

        string userId;

        GetDashboard_Result dashboard;

        string ContactsText = "Add contacts so they can store my fragments";
        string KeysText = "Add keys and send them to selected contacts";
        string RequestsText = "View my key retrieval requests";
        string FragRequestsText = "View the fragment users are requesting";
        string FragmentsText = "View fragments I am storing";
        string SettingsText = "Import or export settings";

        public Dashboard()
        {
            InitializeComponent();

            try
            {
                NavigationPage.SetHasNavigationBar(this, false);

                btnKeys.Clicked += BtnKeys_Clicked;
                btnRequests.Clicked += BtnRequests_Clicked;
                btnConatcts.Clicked += BtnConatcts_Clicked;
                btnSettings.Clicked += BtnSettings_Clicked;
                btnFragments.Clicked += BtnFragments_Clicked;
                btnFragRequests.Clicked += BtnFragRequests_Clicked;
                btnRefresh.Clicked += btnRefresh_Clicked;

                this.Appearing += Dashboard_Appearing;
                userId = secureStorage.GetFromSecureStorage(Constants.UserId);

                displayName.Text = "Hello " + secureStorage.GetFromSecureStorage(Constants.DisplayName) + ", what would you like to do?";
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Yo, got you");
            }

        }

        private async void LoadDetails()
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("userId", userId);
            dashboard = await new EntityService<GetDashboard_Result>().Get(SettingsService.GetDashboard(), parameters);

            btnKeys.Text = KeysText + Environment.NewLine + "(" + dashboard.Keys.ToString() + " keys present)";
            btnRequests.Text = RequestsText + Environment.NewLine + "(" + dashboard.KeyRequests.ToString() + " key return requests)";
            btnConatcts.Text = ContactsText + Environment.NewLine + "(" + dashboard.Contacts.ToString() + " contacts stored)";
            btnFragments.Text = FragmentsText + Environment.NewLine + "(" + dashboard.Fragments.ToString() + " fragments stored)";
            btnFragRequests.Text = FragRequestsText + Environment.NewLine + "(" + dashboard.FragmentRequests.ToString() + " fragments return request)";

            uint timeout = 1000;

            btnRequests.IsVisible = dashboard.KeyRequests > 0;
            btnFragments.IsVisible = dashboard.Fragments > 0;
            btnFragRequests.IsVisible = dashboard.FragmentRequests > 0;



            if (dashboard.Contacts == 0)
            {
                btnConatcts.BackgroundColor = Color.FromHex("#FCA809");

                new Thread(async () =>
                {
                    while (true)
                    {
                        await btnConatcts.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnConatcts.ScaleTo(1, timeout, Easing.CubicInOut);
                    }
                }).Start();
            }



            if (dashboard.Fragments > 0)
            {

                btnFragRequests.BackgroundColor = Color.FromHex("#FCA809");
                new Thread(async () =>
                {
                    while (true)
                    {
                        await btnFragRequests.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnFragRequests.ScaleTo(1, timeout, Easing.CubicInOut);
                        await btnFragRequests.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnFragRequests.ScaleTo(1, timeout, Easing.CubicInOut);
                        await btnFragRequests.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnFragRequests.ScaleTo(1, timeout, Easing.CubicInOut);
                    }
                }).Start();
            }


            if (dashboard.KeyRequests > 0)
            {

                btnRequests.BackgroundColor = Color.FromHex("#FCA809");
                new Thread(async () =>
                {
                    while (true)
                    {
                        await btnRequests.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnRequests.ScaleTo(1, timeout, Easing.CubicInOut);
                        await btnRequests.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnRequests.ScaleTo(1, timeout, Easing.CubicInOut);
                        await btnRequests.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnRequests.ScaleTo(1, timeout, Easing.CubicInOut);
                    }
                }).Start();
            }

        }

        private async void Dashboard_Appearing(object sender, EventArgs e)
        {
            try
            {
                NavigationPage.SetHasNavigationBar(this, false);


                LoadDetails();
            }
            catch (Exception ex)
            {
               await  DisplayAlert("Bubu", ex.Message, "Yo, got you");
            }

        }

        private void BtnFragments_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new NavigationPage(new Fragments()));
        }


        private void BtnFragRequests_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new NavigationPage(new FragmentsRequests()));
        }


        private void BtnKeys_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new NavigationPage(new Keys()));
        }

        private void BtnConatcts_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new NavigationPage(new Contacts()));
        }

        private void BtnRequests_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new NavigationPage(new KeysRequests()));
        }

        private void BtnSettings_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new NavigationPage(new Settings()));
        }


        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            LoadDetails();
        }
    }
}