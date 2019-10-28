using CryptoGeeks.API;
using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Services;
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

        string displayName;
        User currentUser;

        public Dashboard()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            btnKeys.Clicked += BtnKeys_Clicked;
            btnRequests.Clicked += BtnRequests_Clicked;
            btnConatcts.Clicked += BtnConatcts_Clicked;
            btnSettings.Clicked += BtnSettings_Clicked;
            btnFragments.Clicked += BtnFragments_Clicked;
            btnFragRequests.Clicked += BtnFragRequests_Clicked;
            this.Appearing += Dashboard_Appearing;
            displayName = secureStorage.GetFromSecureStorage(Constants.DisplayName);
            if (String.IsNullOrEmpty(displayName))
                this.Navigation.PushAsync(new Register());


        
        }


        private async void Dashboard_Appearing(object sender, EventArgs e)
        {
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("displayName", displayName);
            currentUser = await new EntityService<User>().Get(SettingsService.GetUserDetailsByName(), parameters);

            btnKeys.Text = "Keys " + Environment.NewLine + "(" + currentUser.Keys.Count() + ")";
            btnRequests.Text = "Key Requests " + Environment.NewLine + "(" + currentUser.Keys.Select(x => x.KeyRequests).Count() + ")";
            btnConatcts.Text = "Contacts " + Environment.NewLine + "(" + currentUser.Contacts.Count() + ")";
            btnFragments.Text = "Fragments " + Environment.NewLine + "(" + currentUser.UserKeyFragments.Count() + ")";
            btnFragRequests.Text = "Fragment Requests " + Environment.NewLine + "(" + currentUser.Fragments.Where(F=>F.SentToOwner==false).Count() + ")";

            uint timeout = 550;

            if (currentUser.Contacts.Count() == 0)
            {
                new Thread(async () =>
                {
                    while (true)
                    {
                        await btnConatcts.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnConatcts.ScaleTo(1, timeout, Easing.CubicInOut);
                        await btnConatcts.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnConatcts.ScaleTo(1, timeout, Easing.CubicInOut);
                        await btnConatcts.ScaleTo(1.05, timeout, Easing.CubicInOut);
                        await btnConatcts.ScaleTo(1, timeout, Easing.CubicInOut);
                    }
                }).Start();
            }



            if (currentUser.Fragments.Where(F => F.SentToOwner == false).Count() > 0)
            {
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


            bool highlightKeyReq = false;

            foreach (Key k in currentUser.Keys)
            {
                bool fragmentsReturned = k.Fragments.Where(f => f.SentToOwner == true).Count() >= k.RecoverNo; ;
                bool hasKeyReq = k.KeyRequests.Count() > 0;

                if (fragmentsReturned && hasKeyReq)
                    highlightKeyReq = true;

            }


            if (highlightKeyReq)
            {
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

        private void BtnFragments_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Fragments());
        }


        private void BtnFragRequests_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new FragmentsRequests());
        }


        private void BtnKeys_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Keys(currentUser));
        }

        private void BtnConatcts_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Contacts());
        }

        private void BtnRequests_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new KeysRequests());
        }

        private void BtnSettings_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Settings());
        }
    }
}