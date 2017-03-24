using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class TieredBlock
    {
        public string Unit { get; set; }
        public string Size { get; set; }
        public string Max { get; set; }
        public List<Price> Prices { get; set; }
    }
}