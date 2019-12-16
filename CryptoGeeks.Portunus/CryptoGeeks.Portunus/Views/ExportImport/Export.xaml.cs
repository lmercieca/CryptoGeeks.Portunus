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


namespace CryptoGeeks.Portunus.Views.ExportImport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Export : ContentPage
    {
        public Export()
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
                if (txtPassword.Text == null || txtPassword.Text.Trim().Length == 0)
                {
                    await DisplayAlert("Export", "Password cannot be left empty", "OK");
                    return;
                }

                CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();

                string fragmentsJson = secureStorage.GetFromSecureStorage(Constants.UserId);
                string keysJson = secureStorage.GetFromSecureStorage(Constants.DisplayName);

                SecurityHelper securityHelper = new SecurityHelper();
                string secureData = securityHelper.Encrypt(fragmentsJson + "~" + keysJson, txtPassword.Text);
                FileWriter(secureData);

                await SendEmail("Portunus - Details export", "Please find attached an export of your current keys and fragments. Store it in a secure manner, remember with great power comes great responsibility (spiderman)!");


                await Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                await DisplayAlert("There was an issue while exporting the file.", ex.Message, "OK");

            }

        }




        public void FileWriter(string data) // Code to generate a text file
        {

            var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filename = Path.Combine(documents, "keys.ptn");

            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.Write(data);
                sw.Close();
            }

        }


        public async Task SendEmail(string subject, string body)
        {
            try
            {
                var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create);
                var filename = Path.Combine(documents, "keys.ptn");


                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    Attachments = new List<EmailAttachment>()
                    {
                        new EmailAttachment(filename)
                    }
                };

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device  
            }
            catch (Exception ex)
            {
                // Some other exception occurred  
            }
        }
    }
}