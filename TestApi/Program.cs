using CryptoGeeks.API;
using CryptoGeeks.Portunus.Services;
using CryptoGeeks.Portunus.Services.POCO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApi
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please enter what to test: ");
                Console.WriteLine("1 Fragments");
                Console.WriteLine("2 Get Fragments");
                string res = Console.ReadLine();

                switch (res)
                {
                    case "1":
                        TestFragment();
                        break;

                    case "2":
                        GetFragments();
                        break;
                }
            }

        }

        static async void GetFragments()
        {

            EntityService<ObservableCollection<GetFragmentsForUser_Result>> entityService = new EntityService<ObservableCollection<GetFragmentsForUser_Result>>();
            NameValueCollection parameters = new NameValueCollection();
            
            parameters.Add("userId", "69");

            ObservableCollection<GetFragmentsForUser_Result> res =  await entityService.Get(SettingsService.GetAllFragmentsForUserUrl(), parameters);


            foreach (GetFragmentsForUser_Result f in res)
            {
                Console.WriteLine(f.Data);
            }

        }

        static async void TestFragment()
        {
            Fragment frag = new Fragment()
            {
                FragmentHolder = "FragmentHolder",
                Data = "Data",
                SentToHolder = true,
                SentToOwner = false,
                Owner = 1

            };

            Key k = new Key()
            {
                Owner = "Owner",

                RecoverNo = 1,
                Key1 = "Owner",
                Data = "Data",
                Split = 2,
                User = 1,
                Fragments = new List<Fragment>() { frag}
            };


            //EntityService<Fragment> fragservice = new EntityService<Fragment>();
            //await fragservice.Add(SettingsService.PostFragmentUrl(), frag);

            EntityService<Key> keyService = new EntityService<Key>();
            keyService.Add(SettingsService.PostKeyUrl(), k);
        }
    }
}
