using System;
using KillBill.Client.Net.HttpClients;

namespace KillBill.Client.Net.Tests
{
    public class BaseTestFixture
    {
        protected readonly Guid TenantId = new Guid("e692b901-ded2-4cb7-a060-e3fd3a60608d");
        protected readonly Guid AccountId = new Guid("0c14f834-c265-4e55-a4e4-cb12d06b4276");
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