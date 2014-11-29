using System;
using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class PaymentTransaction
    {
        public Guid? TransactionId { get; set; }
        public string TransactionExternalKey { get; set; }
        public Guid? PaymentId { get; set; } 
        public string PaymentExternalKey { get; set; }
        public string TransactionType { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string GatewayErrorCode { get; set; }
        public string GatewayErrorMsg { get; set; }
        public string FirstPaymentReferenceId { get; set; }
        public string SecondPaymentReferenceId { get; set; }
        public List<PluginProperty> Properties { get; set; }
    }
}