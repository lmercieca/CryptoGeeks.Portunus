using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Api;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.Registration
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Register : ContentPage
	{
        SecureStorage secureStorage = new SecureStorage();
        ContactService cs = new ContactService();

        public Register ()
		{
			InitializeComponent ();

        

        }
        private async Task<bool> DisplayNameExists(string displayName)
        {
            bool result = false;
            result = await cs.DisplayNameExists(displayName);
            
            return result;
        }

        private async void BtnAgree_Clicked(object sender, EventArgs e)
        {
            btnAgree.IsEnabled = false;

            if (!cbAgreement.IsToggled)
                await DisplayAlert("Registration validation error", "Please accept the terms and conditions before proceeding", "OK");
            else
            {
                if (DisplayName.Text.Trim().Length == 0)
                {
                    await DisplayAlert("Registration validation error", "Display name cannot be left empty", "OK");
                }
                else
                {
                    if (await DisplayNameExists(DisplayName.Text))
                    {
                        await DisplayAlert("Registration validation error", "Display name already exists", "OK");
                    }
                    else
                    {
                        int id = await cs.AddDisplayName(DisplayName.Text);

                        secureStorage.StoreInSecureStorage(Constants.DisplayName, DisplayName.Text);
                        secureStorage.StoreInSecureStorage(Constants.UserId, id.ToString());

                        await Navigation.PushModalAsync(new CryptoGeeks.Portunus.Views.Dashboard.Dashboard());
                    }
                }
            }

            btnAgree.IsEnabled = true;
        }
    }
}