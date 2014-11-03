using System;

namespace KillBill.Client.Net.Model
{
    public class Tenant : KillBillObject
    {
        public Guid TenantId { get; set; }
        public string ExternalKey { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; } 
    }
}