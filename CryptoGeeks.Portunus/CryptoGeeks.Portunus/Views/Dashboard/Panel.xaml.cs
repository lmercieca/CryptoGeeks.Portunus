using CryptoGeeks.Portunus.ViewModels;
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
    public partial class Panel : TabbedPage
    {
        ItemListViewModel itemListViewModel;


        public Panel ()
        {
            InitializeComponent();
            itemListViewModel = new ItemListViewModel();
            BindingContext = itemListViewModel;
        }

        private void BtnAddContact_Clicked(object sender, EventArgs e)
        {

        }

        void OnAddContactTapped(object sender, EventArgs args)
        {
            try
            {
                //Code to execute on tapped event
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void OnAddKeyTapped(object sender, EventArgs args)
        {
            try
            {
                //Code to execute on tapped event
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void OnRecoverKeyTapped(object sender, EventArgs args)
        {
            try
            {
                //Code to execute on tapped event
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

    }
}