using System;
using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class Payment : KillBillObject
    {
        public Guid AccountId { get; set; }
        public Guid? PaymentId { get; set; }
        public int PaymentNumber { get; set; }
        public string PaymentExternalKey { get; set; }
        public decimal AuthAmount { get; set; }
        public decimal CapturedAmount { get; set; }
        public decimal PurchasedAmount { get; set; }
        public decimal RefundedAmount { get; set; }
        public decimal CreditedAmount { get; set; }
        public string Currency { get; set; }
        public Guid PaymentMethodId { get; set; }
        public List<PaymentTransaction> Transactions { get; set; }
    }
}