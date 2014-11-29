using System;
using KillBill.Client.Net.JSON;
using Newtonsoft.Json;

namespace KillBill.Client.Net.Model
{
    public class PaymentMethod : KillBillObject
    {
       
        public Guid? PaymentMethodId { get; set; }
        public string ExternalKey { get; set; }
        public Guid AccountId { get; set; }
        public bool IsDefault { get; set; }
        public string PluginName { get; set; }
        public PaymentMethodPluginDetail PluginInfo { get; set; }
         
    }
}