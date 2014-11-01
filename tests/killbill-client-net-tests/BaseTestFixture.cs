using System;
using KillBill.Client.Net.HttpClients;

namespace KillBill.Client.Net.Tests
{
    public class BaseTestFixture
    {
        protected readonly Guid AccountId = new Guid("ddacacf1-f81a-4b83-a7f1-85fd2e13902e");
        protected readonly Guid BundleId = new Guid("7a527d56-f40b-4830-8ac5-7e8c85a2348d");
        protected readonly KillBillClient Client;

        protected readonly string CreatedBy = "Testing User";
        protected readonly string Reason = "KillBill Api Test";
        protected Random Random = new Random();

        public BaseTestFixture()
        {
            IKbHttpClient httpClient = new HttpClientBasicAuth();
            Client = new KillBillClient(httpClient);
        }
    }
}