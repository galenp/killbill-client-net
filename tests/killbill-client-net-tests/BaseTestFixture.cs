using System;

namespace KillBill.Client.Net.Tests
{
    public class BaseTestFixture
    {
        protected static readonly Guid AccountId = new Guid("1d484b0a-5e7b-4ee8-b5d6-a6893db879a5");


        protected readonly Guid BundleId = new Guid("554fe9de-b9e9-4321-ab71-791151944a91");
        protected readonly KillBillClient Client;

        protected readonly string CreatedBy = "Testing User";
        protected readonly string Reason = "KillBill Api Test";
        protected Random Random = new Random();
        protected readonly RequestOptions Options;

        public BaseTestFixture()
        {
            IKbHttpClient httpClient = new KillBillHttpClient();
            Client = new KillBillClient(httpClient);

            Options = RequestOptions.Builder()
                                            .WithRequestId(Guid.NewGuid().ToString())
                                            .WithCreatedBy(CreatedBy)
                                            .WithReason(Reason)
                                            .WithUser(KbConfig.HttpUser)
                                            .WithPassword(KbConfig.HttpPassword)
                                            .WithTenantApiKey(KbConfig.ApiKey)
                                            .WithTenantApiSecret(KbConfig.ApiSecret)
                                            .WithComment("kill-bill-net-tests")
                                            .Build();
        }
    }
}