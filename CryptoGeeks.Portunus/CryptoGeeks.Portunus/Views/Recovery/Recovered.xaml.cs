using CryptoGeeks.API;
using CryptoGeeks.Portunus.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.Recovery
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Recovered : ContentPage
	{
        private Key key;


        public Recovered(Key key)
        {
            this.key = key;

            BindingContext = key;
            InitializeComponent();

            txtData.Text = key.Data;

        }

        public Recovered ()
		{
			InitializeComponent ();
		}

        private async void BtnDone_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new CryptoGeeks.Portunus.Views.Dashboard.Dashboard()), true);

        }
    }
}