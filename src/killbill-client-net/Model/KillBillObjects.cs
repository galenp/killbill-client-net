using System.Collections.Generic;

namespace KillBill.Client.Net.Model
{
    public class KillBillObjects<T> : List<T> where T : KillBillObject
    {
        public int PaginationCurrentOffset { get; set; }
        public int PaginationNextOffset { get; set; }
        public int PaginationTotalNbRecords { get; set; }
        public int PaginationMaxNbRecords { get; set; }
        public string PaginationNextPageUri { get; set; }

    }
}