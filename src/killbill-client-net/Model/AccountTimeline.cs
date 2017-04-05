using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class AccountTimeline : KillBillObject
    {
        public Account Account { get; set; }
        public List<Bundle> Bundles { get; set; }
        public List<Invoice> Invoices { get; set; }
        public List<InvoicePayment> Payments {get;set;}
    }
}