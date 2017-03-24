using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class Product : KillBillObject
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public List<Plan> Plans { get; set; }
        public List<string> Included { get; set; }
        public List<string> Available { get; set; }
    }
}