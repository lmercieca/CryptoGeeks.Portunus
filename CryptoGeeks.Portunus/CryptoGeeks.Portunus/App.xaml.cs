using Plugin.Multilingual;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using CryptoGeeks.Portunus.Views.Registration;
using CryptoGeeks.Portunus.Views.Dashboard;
using CryptoGeeks.Common;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CryptoGeeks.Portunus
{
    public partial class App : Application
    {
        SecureStorage secureStorage = new SecureStorage();

        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            
            try
            {
                PortunusResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }

            string displayName = secureStorage.GetFromSecureStorage("displayname");

            if (!string.IsNullOrEmpty(displayName))
                MainPage = new Panel();
            else
                MainPage = new Register();

        }

        protected override void OnStart()
        {

            try
            {
                AppCenter.Start("android=d736b270-a39e-48b4-a624-e2c2f29755be;" +
                "uwp=8f8d0010-e66b-4b71-9807-0f284d303e4d;" +
                "ios=861852a1-5d74-4e18-88f5-73b81c097318",
                typeof(Analytics), typeof(Crashes));
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
