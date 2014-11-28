using System;
using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class SubscriptionUsageRecord : KillBillObject
    {
        public Guid SubscriptionId { get; set; }
        public List<UnitUsageRecord>
        {
            get;
            set;
        }
         
    }
}