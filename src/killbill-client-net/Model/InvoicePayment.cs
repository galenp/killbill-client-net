using System;

namespace KillBill.Client.Net.Model
{
    public class InvoicePayment : Payment
    {
        public Guid TargetInvoiceId { get; set; }
    }
}