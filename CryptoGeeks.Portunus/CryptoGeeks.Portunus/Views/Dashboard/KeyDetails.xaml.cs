using CryptoGeeks.API;
using CryptoGeeks.Portunus.Services;
using CryptoGeeks.Portunus.Services.POCO;
using Plugin.Clipboard;
//using CryptoGeeks.Portunus.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        GetKeysForUser_Result key;
        ObservableCollection<GetFragmentsForKey_Result> fragments = new ObservableCollection<GetFragmentsForKey_Result>();
        ObservableCollection<KeyRequest> keyRequests = new ObservableCollection<KeyRequest>();

        public KeyDetails()
        {
            InitializeComponent();
        }

        private async Task<ObservableCollection<GetFragmentsForKey_Result>> LoadKeyFragments()
        {
            EntityService<ObservableCollection<GetFragmentsForKey_Result>> entityService = new EntityService<ObservableCollection<GetFragmentsForKey_Result>>();
            NameValueCollection parameters = new NameValueCollection();
            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("keyId",key.Id.ToString());

            return await entityService.Get(SettingsService.GetAllFragmentsForKeyUrl(), parameters);
        }


        private async Task<ObservableCollection<KeyRequest>> LoadKeyRequest()
        {
            EntityService<ObservableCollection<KeyRequest>> entityService = new EntityService<ObservableCollection<KeyRequest>>();
            NameValueCollection parameters = new NameValueCollection();
            CryptoGeeks.Common.SecureStorage secureStorage = new CryptoGeeks.Common.SecureStorage();
            parameters.Add("keyId", key.Id.ToString());

            return await entityService.Get(SettingsService.GetKeyRequestsForKeyUrl(), parameters);
        }

        public async void BindData()
        {
            try
            {
                this.fragments = await LoadKeyFragments();
                this.keyRequests = await LoadKeyRequest();

                FragmentsListView.ItemsSource = this.fragments;
                FragmentsListView.HeightRequest = (this.fragments.Count() * 100) / 2;

                bool pendingKeys = this.fragments.Where(x => x.SentToOwner == false).Count() >= key.RecoverNo;

                if (!pendingKeys)
                    btnRecover.Text = "View";
                else
                    btnRecover.Text = "Recover";
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
        }

        public KeyDetails(GetKeysForUser_Result key)
        {
            this.key = key;
            BindData();

            InitializeComponent();

            BindingContext = this.key;

        

        }

       
        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void BtnRecover_Clicked(object sender, EventArgs e)
        {
            bool pendingKeys = this.fragments.Where(x => x.SentToOwner == false).Count() >= key.RecoverNo;

            if (!pendingKeys)
            {
                EntityService<KeyRequest> krServ = new EntityService<KeyRequest>();

                foreach (KeyRequest kr in this.keyRequests)
                {
                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("KeyId", kr.KeyID.ToString());
                    await krServ.Get(SettingsService.MarkReqyestComplete(), parameters);
                }

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

                await this.Navigation.PushModalAsync(new NavigationPage(new Dashboard()));
            }
        }
    }
}