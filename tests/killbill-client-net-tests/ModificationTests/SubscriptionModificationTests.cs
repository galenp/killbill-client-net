using System;
using System.Collections.Generic;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{
    [TestFixture]
    public class SubscriptionModificationTests : BaseTestFixture
    {
        private readonly string externalKey = "aaaaa";

        [Test]
        public void Create_New_Subscription_WithAddons()
        {
            //Given
            var accountId = new Guid("5e61b191-16cf-4322-bf52-df805fc78ba9");
            var bundleKey = Guid.NewGuid();
            var subscriptions = new List<Subscription>()
            {
                new Subscription
                {
                  AccountId = accountId,
                  PlanName = "system-connect-monthly",
                  ProductName = "system-connect",
                  ProductCategory = "BASE",
                  BillingPeriod = "MONTHLY",
                  ExternalKey = $"system-connect-" + bundleKey,
                },

                new Subscription
                {
                    AccountId = accountId,
                    PlanName = "external-site-monthly",
                    ProductName = "external-site",
                    ProductCategory = "ADD_ON",
                    BillingPeriod = "MONTHLY",
                    ExternalKey = $"system-connect-" + bundleKey,
                }
            };

            //When
            var bundle = Client.CreateSubscriptionWithAddOns(subscriptions, Options);

            //Then
            bundle.Should().NotBeNull();
        }

        [Test]
        public void Create_New_Subscription_Bundle()
        {
            //Given
            var subscription =new Subscription
            {
                AccountId = AccountId,
                ExternalKey = externalKey,
                ProductName = "Standard",
                ProductCategory = "BASE",
                BillingPeriod = "MONTHLY",
                PriceList = "DEFAULT",
                StartDate = DateTime.Now
            };

            //When
            var bundle = Client.CreateSubscription(subscription, Options);

            //Then
            bundle.Should().NotBeNull();
            bundle.AccountId.Should().Be(AccountId);
        }


        [Test]
        [Ignore]  //I THINK i've misunderstood bundles<->subscriptions. 9:30am after an all nighter... time to sleep and resume tomorrow... or the day after.
       
        public void Add_Subscription_To_Bundle()
        {
            //Given
            var existingBundle = Client.GetBundle(externalKey, Options);
            existingBundle.Should().NotBeNull("We can't add a second subscription if the bundle doesnt exist... run the above test first.");

            var subscription = new Subscription
            {
                AccountId = AccountId,
                BundleId = existingBundle.BundleId,
                ExternalKey = externalKey,
                ProductName = "Sports",
                ProductCategory = "BASE",
                BillingPeriod = "MONTHLY",
                PriceList = "DEFAULT",
                StartDate = DateTime.Now
            };

            //When
            var secondSubcription = Client.CreateSubscription(subscription, Options);

            //Then
            secondSubcription.Should().NotBeNull();
            secondSubcription.BundleId.Should().Be(existingBundle.BundleId);
        }
    }
}