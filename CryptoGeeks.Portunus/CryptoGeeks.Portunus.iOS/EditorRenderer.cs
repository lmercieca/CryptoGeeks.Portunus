using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoGeeks.Portunus.Controls;
using CryptoGeeks.Portunus.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PortunusEntry), typeof(PortunusEntryRenderer))]
namespace CryptoGeeks.Portunus.iOS
{
    public class PortunusEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                Control.BackgroundColor = UIColor.White;
                Control.TextColor = UIColor.FromRGB(0, 120, 230);
                Control.BorderStyle = UITextBorderStyle.Line;
            }
        }
    }
}
