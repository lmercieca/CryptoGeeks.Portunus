using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CryptoGeeks.Portunus.Controls;
using CryptoGeeks.Portunus.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(PortunusEntry), typeof(PortunusEntryRenderer))]
namespace CryptoGeeks.Portunus.Droid
{
 

    public class PortunusEntryRenderer : EntryRenderer
    {
        public PortunusEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundColor(global::Android.Graphics.Color.WhiteSmoke);
                Control.SetTextColor(Xamarin.Forms.Color.FromHex("#011E3B").ToAndroid());
            }
        }
    }
}