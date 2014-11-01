using System;
using KillBill.Client.Net.JSON;
using Newtonsoft.Json;

namespace KillBill.Client.Net.Model
{
    public class InvoiceItem : KillBillObject
    {
        public Guid? InvoiceItemId { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? LinkedInvoiceItemId { get; set; }
        public Guid AccountId { get; set; }
        public Guid? BundleId { get; set; }
        public Guid? SubscriptionId { get; set; }
        public string PlanName { get; set; }
        public string PhaseName { get; set; }
        public string UsageName { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        [JsonConverter(typeof(ShortDateTimeConverter))]
        public DateTime? StartDate { get; set; }
        
        [JsonConverter(typeof(ShortDateTimeConverter))]
        public DateTime? EndDate { get; set; }
        public Double Amount { get; set; }
        public string Currency { get; set; }

        
    }
}