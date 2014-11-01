using System;
using KillBill.Client.Net.JSON;
using Newtonsoft.Json;

namespace KillBill.Client.Net.Model
{
    public class EventBaseSubscription
    {
        public string BillingPeriod { get;  set; }
        [JsonConverter(typeof(ShortDateTimeConverter))]
        public DateTime? RequestedDate { get;  set; }
        public string Product { get;  set; }
        public string PriceList { get;  set; }
        public string EventType { get;  set; }
        public string Phase { get;  set; }
       
    }
}