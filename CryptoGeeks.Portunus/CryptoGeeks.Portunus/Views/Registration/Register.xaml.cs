using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Api;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Services;
using CryptoGeeks.Portunus.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.Registration
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Register : ContentPage
	{
        Common.SecureStorage secureStorage = new Common.SecureStorage();
        ContactService cs = new ContactService();

        public Register ()
		{
			InitializeComponent ();

            this.BindingContext = new ViewModel();

        }
        private async Task<bool> DisplayNameExists(string displayName)
        {
            bool result = false;
            result = await cs.DisplayNameExists(displayName);
            
            return result;
        }


        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Device.OpenUri(new System.Uri(url));
        });


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
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("displayName", DisplayName.Text);
                        

                        int id = await new EntityService<int>().Get(SettingsService.GetNewUser(), parameters);

                        secureStorage.StoreInSecureStorage(Constants.DisplayName, DisplayName.Text);
                        secureStorage.StoreInSecureStorage(Constants.UserId, id.ToString());

                        await Navigation.PushModalAsync(new NavigationPage(new CryptoGeeks.Portunus.Views.Dashboard.Dashboard()));
                    }
                }
            }

            btnAgree.IsEnabled = true;
        }

        private void BtnViewContract_Clicked(object sender, EventArgs e)
        {
            string url = "https://www.portunus.ai/terms";
            Browser.OpenAsync(new System.Uri(url), BrowserLaunchMode.SystemPreferred);
        }
    }

    public class ViewModel
    {
        public ICommand ClickCommand => new Command<string>((url) =>  
        {

            Browser.OpenAsync(new System.Uri(url), BrowserLaunchMode.SystemPreferred);

        });
    }
}