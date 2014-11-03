using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class TenantKey : KillBillObject
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
    }
}