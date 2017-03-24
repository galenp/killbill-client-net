using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class Phase : KillBillObject
    {
        public string Type { get; set; }
        public List<Price> Prices { get; set; }
        public List<Price>  FixedPrices { get; set; }
        public Duration Duration { get; set; }
        public List<Usage> Usages { get; set; }
    }
}