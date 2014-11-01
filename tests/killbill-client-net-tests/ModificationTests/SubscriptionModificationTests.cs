using System;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{
    [TestFixture]
    public class SubscriptionModificationTests : BaseTestFixture
    {
        private Subscription subscription;
      
      
        [SetUp]
        public void Setup()
        {
           
            subscription = new Subscription
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
        }

        [Test]
        public void Create_Subscription()
        {
            //When
            var createdBunde = Client.CreateSubscription(subscription, "test user", "testing bundle creation", "Create_Bundle()");

            //Then
            createdBunde.Should().NotBeNull();
            createdBunde.AccountId.Should().Be(AccountId);
        }


    }
}