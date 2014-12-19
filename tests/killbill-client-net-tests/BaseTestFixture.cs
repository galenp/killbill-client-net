using System;
using KillBill.Client.Net.HttpClients;

namespace KillBill.Client.Net.Tests
{
    public class BaseTestFixture
    {
        protected readonly Guid AccountId = new Guid("7c9c1ca7-e835-4ac0-a615-ceda6ed2567b");
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