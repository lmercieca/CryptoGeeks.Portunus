using CryptoGeeks.Portunus.Helpers;
//using CryptoGeeks.Portunus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode;

namespace CryptoGeeks.Portunus.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportData : ContentPage
    {
        public ImportData()
        {
            InitializeComponent();
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }


        private async void BtnExport_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text.Trim().Length == 0)
                {
                    await DisplayAlert("Export", "Password cannot be left empty", "OK");
                    return;
                }
                string data = txtData.Text;
                CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();

                SecurityHelper securityHelper = new SecurityHelper();
               string result = 
                    securityHelper.Decrypt(data, txtPassword.Text);


                string[] personalData = result.Split('~');

                secureStorage.StoreInSecureStorage(Constants.UserId, personalData[0]);
                secureStorage.StoreInSecureStorage(Constants.DisplayName, personalData[1]);


                await this.Navigation.PushModalAsync(new NavigationPage(new Portunus.Views.Dashboard.Dashboard()));
            }
            catch(Exception ex)
            {
                await DisplayAlert("Export Issue", ex.Message, "OK");

            }

        }




    }
}