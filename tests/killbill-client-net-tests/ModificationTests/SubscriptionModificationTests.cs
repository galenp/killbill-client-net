using System;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{
    [TestFixture]
    public class SubscriptionModificationTests : BaseTestFixture
    {

        [Test]
        public void Create_Subscription()
        {
            //Given
            var subscription =new Subscription
            {
                AccountId = AccountId,
                BundleId = Guid.NewGuid(),
                ExternalKey = DateTime.UtcNow.ToString("yyyyMMddhhmmss"),
                ProductName = "Standard",
                ProductCategory = "BASE",
                BillingPeriod = "MONTHLY",
                PriceList = "DEFAULT",
                StartDate = DateTime.Now
            };

            //When
            var bundle = Client.CreateSubscription(subscription, "test user", "testing bundle creation", "Create_Bundle()");

            //Then
            bundle.Should().NotBeNull();
            bundle.AccountId.Should().Be(AccountId);
        }


    }
}