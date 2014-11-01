using System;
using KillBill.Client.Net.JSON;
using Newtonsoft.Json;

namespace KillBill.Client.Net.Model
{
    public class EventSubscription : EventBaseSubscription
    {
        public Guid EventId { get;  set; }

        [JsonConverter(typeof(ShortDateTimeConverter))]
        public DateTime? EffectiveDate { get;  set; }
      
    }
}