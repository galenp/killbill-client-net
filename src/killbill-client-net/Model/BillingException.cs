using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KillBill.Client.Net.Model
{
    public class BillingException
    {
        public string ClassName { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string CauseClassName { get; set; }
        public string CauseMessage { get; set; }
        public List<StackTraceElement> StackTrace { get; set; }
    }
}
