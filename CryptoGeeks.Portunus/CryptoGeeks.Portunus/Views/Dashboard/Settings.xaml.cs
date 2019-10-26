using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Api;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Views.ExportImport;
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
    public partial class Settings : ContentPage
    {
        SecureStorage secureStorage = new SecureStorage();
        ContactService cs = new ContactService();


        public Settings()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {
                DisplayAlert("",ex.Message,"Ok");
            }
        }

        private async void BtnImport_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new ImportData());

        }

        private async void BtnExport_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new Export());
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
           /*
            if (DisplayName.Text.Trim().Length == 0)
            {
                await DisplayAlert("Validation error", "Display name cannot be left empty", "OK");
            }
            else
            {
                if (await DisplayNameExists(DisplayName.Text))
                {
                    await DisplayAlert("Registration validation error", "Display name already exists", "OK");
                }
                else
                {

                    await cs.UpdateDisplayName(secureStorage.GetFromSecureStorage(Constants.UserId), DisplayName.Text);

                    secureStorage.StoreInSecureStorage(Constants.DisplayName, DisplayName.Text);
                    
                    await Navigation.PushAsync(new CryptoGeeks.Portunus.Views.Dashboard.Dashboard());
                }
            }*/
        }

        private async Task<bool> DisplayNameExists(string displayName)
        {
            bool result = false;
            result = await cs.DisplayNameExists(displayName);

            return result;
        }

    }
}