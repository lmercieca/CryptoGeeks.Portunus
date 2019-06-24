using CryptoGeeks.Portunus.Api.Model;
using CryptoGeeks.Portunus.Models;
using CryptoGeeks.Portunus.ViewModels;
using CryptoGeeks.Portunus.Views.AddKey;
using CryptoGeeks.Portunus.Views.ExportImport;
using CryptoGeeks.Portunus.Views.Recovery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.MultiSelectListView;
using Xamarin.Forms.Xaml;

namespace CryptoGeeks.Portunus.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Recover : ContentPage
    {

        private MultiSelectObservableCollection<Fragment> fragments = new MultiSelectObservableCollection<Fragment>();


        private Key key;


        public Recover(Key key)
        {
            this.key = key;

            BindingContext = key;
            InitializeComponent();

            foreach (Fragment f in key.Fragments)
            {
                SelectableItem<Fragment> fr = new SelectableItem<Fragment>(f);
                fr.IsSelected = false;
                fragments.Add(fr);
            }

            ContactsListView.BindingContext = fragments;
            ContactsListView.ItemsSource = fragments;

            this.Appearing += AddContact_Appearing;


            btnDone.IsVisible = false;

        }

        private async void AddContact_Appearing(object sender, EventArgs e)
        {
            base.OnAppearing();




        }



        private async Task<bool> LoadData()
        {
           
         
          

            Device.BeginInvokeOnMainThread(() =>
            {

                var f = fragments.Where(c => !c.IsSelected).FirstOrDefault();
                {

                    if (f != null)
                        f.IsSelected = true;
                }
                Thread.Sleep(TimeSpan.FromSeconds(RandomNumber(3, 5)));


                Console.WriteLine("Entered main thread at " + DateTime.Now.ToString("hh:mm:ss"));

                ContactsListView.BeginRefresh();

                ContactsListView.ItemsSource = null;
                ContactsListView.ItemsSource = fragments;
                ContactsListView.EndRefresh();
                Console.WriteLine("Done main thread at " + DateTime.Now.ToString("hh:mm:ss"));


                if (fragments.Where(c => c.IsSelected).Count() >= key.RequiredFragments)
                {

                    btnStart.IsVisible = false;
                    btnDone.IsVisible = true;
                }
            });

            return await Task.FromResult(true);

        }

        private void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ContactViewModel model = e.SelectedItem as ContactViewModel;
            model.Selected = !model.Selected;
        }

        private async void BtnDone_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new Recovered(key)));
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void BtnStart_Clicked(object sender, EventArgs e)
        {
            List<Task> allTasks = new List<Task>();
            int cnt = 0;

            foreach (SelectableItem<Fragment> f in fragments)
            {
                Console.WriteLine("Loop started at " + DateTime.Now.ToString("hh:mm:ss"));

                Task x = Task.Run(async () =>
              {
                  Console.WriteLine("Entered tas at " + DateTime.Now.ToString("hh:mm:ss"));

                  //await Task.Delay(TimeSpan.FromSeconds(RandomNumber(3, 7)));
                 // f.IsSelected = true;

                  Console.WriteLine("Load Data call at " + DateTime.Now.ToString("hh:mm:ss"));
                  await LoadData();
                  Console.WriteLine("Load Data called at " + DateTime.Now.ToString("hh:mm:ss"));
                  

              });

                allTasks.Add(x);
            }

            Console.WriteLine("wait all called at " + DateTime.Now.ToString("hh:mm:ss"));
            Task.WaitAll(allTasks.ToArray());
            Console.WriteLine("wait all finished at " + DateTime.Now.ToString("hh:mm:ss"));






        }


    }
}