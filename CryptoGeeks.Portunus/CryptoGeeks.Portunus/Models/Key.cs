using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Models
{
    public class Key
    {

        public string DisplayName { get; set; }

        private List<Fragment> fragments = new List<Fragment>();

        public DateTime CreatedDate { get; set; }

        public String CreatedDateAsString { get { return CreatedDate.ToString("dd MMM yy"); } }

        public int NumberOfFragments { get { return fragments.Count; } }

        public int RequiredFragments { get; set; }

        public List<Fragment> Fragments
        {
            get { return fragments; }
            set { fragments = value; }
        }
    }
}
