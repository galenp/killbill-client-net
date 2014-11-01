using System;

namespace KillBill.Client.Net.Model
{
    public class AccountEmail : KillBillObject
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; }
    }
}