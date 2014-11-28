using System;
using KillBill.Client.Net.HttpClients;

namespace KillBill.Client.Net.Tests
{
    public class BaseTestFixture
    {
        protected readonly Guid AccountId = new Guid("d8d4d0b6-9dd7-478d-801b-013eb335ff27");
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