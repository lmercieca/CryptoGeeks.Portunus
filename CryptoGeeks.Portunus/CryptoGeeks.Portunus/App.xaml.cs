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
using CryptoGeeks.Portunus.Views.AddContact;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Api;
using System.Threading.Tasks;
using Xamarin.Essentials;
using SecureStorage = CryptoGeeks.Common.SecureStorage;
using Matcha.BackgroundService;
using System.Threading;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CryptoGeeks.Portunus
{
    //From is coming the same as the server ip

    public partial class App : Application
    {
        SecureStorage secureStorage = new SecureStorage();

        public App()
        {
            InitializeComponent();


            ExperimentalFeatures.Enable(ExperimentalFeatures.EmailAttachments);

            try
            {
                PortunusResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }


           
            MainPage = new NavigationPage(new CryptoGeeks.Portunus.Landing());

            
            //MainPage = new NavigationPage(new CryptoGeeks.Portunus.Views.Dashboard.Comm());
            //MainPage = new NavigationPage(new CryptoGeeks.Portunus.Views.Dashboard.Dashboard());

            /*
            if (!string.IsNullOrEmpty(displayName))
                MainPage = new NavigationPage(new Keys());
            else
             */
           //MainPage = new NavigationPage(new Register());
               

        }

        protected override void OnStart()
        {

            try
            {
                AppCenter.Start("android=d736b270-a39e-48b4-a624-e2c2f29755be;" +
                "uwp=8f8d0010-e66b-4b71-9807-0f284d303e4d;" +
                "ios=861852a1-5d74-4e18-88f5-73b81c097318",
                typeof(Analytics), typeof(Crashes));

                BackgroundAggregatorService.Add(() => new PingService(10));
                BackgroundAggregatorService.Add(() => new TCPReceiveService(100));

                //Start the background service
                //BackgroundAggregatorService.StartBackgroundService();
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
