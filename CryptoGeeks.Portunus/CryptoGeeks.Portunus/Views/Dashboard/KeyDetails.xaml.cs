using CryptoGeeks.Portunus.Models;
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
        private Key key; 

		public KeyDetails ()
		{
			InitializeComponent ();
		}


        public KeyDetails(Key key)
        {
            this.key = key;

            BindingContext = key;
            InitializeComponent();

            FragmentsListView.BindingContext = key;

        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void BtnRecover_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new Recover(key)), true);
        }
    }
}