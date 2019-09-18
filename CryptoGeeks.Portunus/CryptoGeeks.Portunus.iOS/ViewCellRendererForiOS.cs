using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoGeeks.Portunus.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellRendererForiOS))]
namespace CryptoGeeks.Portunus.iOS
{

    public class ViewCellRendererForiOS : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            return cell;
        }
    }
}