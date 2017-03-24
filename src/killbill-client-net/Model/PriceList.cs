using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class PriceList
    {
        public string Name { get; set; }
        public List<string> Plans { get; set; }
    }
}