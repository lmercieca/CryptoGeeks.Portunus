using CryptoGeeks.Common;
using CryptoGeeks.Portunus.Helpers;
using CryptoGeeks.Portunus.Views.Dashboard;
using CryptoGeeks.Portunus.Views.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Landing : ContentPage
    {
        SecureStorage secureStorage = new SecureStorage();

        public Landing()
        {
            InitializeComponent();

            this.Appearing += Landing_Appearing;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Landing_Appearing(object sender, EventArgs e)
        {
            uint timeout = 550;
            await LogoImage.ScaleTo(1.25, timeout, Easing.CubicInOut);
            await LogoImage.ScaleTo(1, timeout, Easing.CubicInOut);
            await LogoImage.TranslateTo(0, 15, timeout, Easing.Linear);

            await TitleImage.FadeTo(1, timeout);
            await TitleText.FadeTo(1, timeout);


            await Task.Delay(3000);


           string displayName = secureStorage.GetFromSecureStorage(Constants.DisplayName);

           if (!string.IsNullOrEmpty(displayName))
            {
                
                await this.Navigation.PushModalAsync(new NavigationPage(new Dashboard()));
            }
            else
                await this.Navigation.PushModalAsync(new NavigationPage(new Register()));
        }

    }
}