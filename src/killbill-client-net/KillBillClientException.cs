using System;

namespace KillBill.Client.Net
{
    public class KillBillClientException : Exception
    {
        public KillBillClientException()
        {
        }

        public KillBillClientException(string message) : base(message)
        {
        }

        public KillBillClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}