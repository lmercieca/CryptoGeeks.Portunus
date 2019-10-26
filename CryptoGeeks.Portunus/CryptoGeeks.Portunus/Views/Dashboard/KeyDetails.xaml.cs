using CryptoGeeks.API;
using CryptoGeeks.Portunus.Services;
using Plugin.Clipboard;
//using CryptoGeeks.Portunus.Models;
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
    public partial class KeyDetails : ContentPage
    {
        Key key;
       
        public KeyDetails()
        {
            InitializeComponent();
        }


        public KeyDetails(Key key)
        {
            this.key = key;

             InitializeComponent();

            BindingContext = this.key;

            FragmentsListView.HeightRequest = (key.Fragments.Count * 100)/ 2;

            bool pendingKeys = key.Fragments.Where(x => x.SentToOwner == false).Count() >= key.RecoverNo;

            if (!pendingKeys)
                btnRecover.Text = "View";
            else
                btnRecover.Text = "Recover";

        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void BtnRecover_Clicked(object sender, EventArgs e)
        {
            bool pendingKeys = key.Fragments.Where(x => x.SentToOwner == false).Count() >= key.RecoverNo;

            if (!pendingKeys)
            {
                bool copy = await DisplayAlert("Key data", key.Data, "Copy", "Close");
                if (copy)
                    CrossClipboard.Current.SetText(key.Data);
            }
            else
            {
                KeyRequest kr = new KeyRequest();
                kr.KeyID = key.Id;
                kr.RequestDate = DateTime.Now;

                EntityService<KeyRequest> keyRequestService = new EntityService<KeyRequest>();
                await keyRequestService.Add(SettingsService.PostKeyRequestUrl(), kr);

                await this.Navigation.PushAsync(new Dashboard());
            }
        }
    }
}