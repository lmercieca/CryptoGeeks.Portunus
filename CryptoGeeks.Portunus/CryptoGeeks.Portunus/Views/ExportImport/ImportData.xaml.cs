using CryptoGeeks.Portunus.Helpers;
//using CryptoGeeks.Portunus.Models;
using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImportData : ContentPage
    {
        FileData filePath = null;

        public ImportData()
        {
            InitializeComponent();

            this.Appearing += ImportData_Appearing;
        }

        private void ImportData_Appearing(object sender, EventArgs e)
        {
            //filePath = null;
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }


        private async void BtnExport_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text == null || txtPassword.Text.Trim().Length == 0)

                {
                    await DisplayAlert("Export", "Password cannot be left empty", "OK");
                    return;
                }

                if (filePath == null)
                 {
                     await DisplayAlert("Export", "Please select the file to import", "OK");
                     return;
                 }


                string data =  System.Text.Encoding.UTF8.GetString(filePath.DataArray);

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
                await DisplayAlert("There was an error while importing the file. Please ensure the file is correct.", ex.Message, "OK");

            }

        }

        private async  void BtnGetFile_Clicked(object sender, EventArgs e)
        {
            try
            {
                string[] allowedExtensions = new string[] { "ptn" };
                filePath = await CrossFilePicker.Current.PickFile(allowedExtensions);

                if (filePath != null)
                    btnGetFile.Text = "Change file";
            }catch(Exception ex)
            {
                await DisplayAlert("There was an error while importing the file. Please ensure the file is correct.", ex.Message, "OK");


            }
        }
    }
}