using System;

namespace KillBill.Client.Net.Model
{
    public class InvoiceEmail : KillBillObject
    {
        public Guid AccountId { get; set; }
        public bool IsNotifiedForInvoices { get; set; }
    }
}