using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class AccountTimeline : KillBillObject
    {
        private Account Account { get; set; }
        private List<Bundle> Bundles { get; set; }
        private List<Invoice> Invoices { get; set; }
        //private List<InvoicePayment> Payments {get;set;}
    }
}