using CryptoGeeks.Portunus.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoGeeks.Portunus.Models
{
    public class Key
    {
        public Key() { }

        public Key(ApiKey key)
        {
            this.DisplayName = key.Owner;
            this.RequiredFragments = key.RecoverNo.Value;
            this.Data = key.Data;

            foreach ( ApiFragment frag in key.Fragments)
            {
                fragments.Add(new Fragment()
                {
                    Data = frag.Data,
                    Owner = new Contact() { DisplayName = frag.FragmentHolder }
                });
            }
        }

        public string Identifier { get; set; }

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

        public string Data { get; set; }
    }
}
