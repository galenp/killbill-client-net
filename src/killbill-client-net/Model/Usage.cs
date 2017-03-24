using System.Collections.Generic;
using System.Net;

namespace KillBill.Client.Net.Model
{
    public class Usage
    {
        public string BillingPeriod { get; set; }
        public List<Tier> Tiers { get; set; }
        
    }
}