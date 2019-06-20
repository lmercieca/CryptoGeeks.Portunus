using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Models
{
    public class Key
    {
        private List<Fragment> fragments = new List<Fragment>();

        public DateTime CreatedDate { get; set; }

        public String CreatedDateAsString { get { return CreatedDate.ToString("dd MMM yy"); } }

        public int NumberOfFragments { get { return fragments.Count; } }

        public List<Fragment> Fragments
        {
            get { return fragments; }
            set { fragments = value; }
        }
    }
}
