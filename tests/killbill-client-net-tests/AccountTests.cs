using System;
using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests
{
    [TestFixture]
    public class AccountTests : BaseTestFixture
    {
        [Test]
        public void Get_Account()
        {
            //When
            var account = Client.GetAccount(AccountId);

            //Then
            account.Should().NotBeNull();
            account.AccountId.Should().Be(AccountId);
        }

        [Test]
        public void Get_Bundles()
        {
            //When
            var bundles = Client.GetAccountBundles(AccountId);

            //Then
            bundles.Should().NotBeNull();
            bundles.Should().NotBeEmpty();
        }

        [Test]
        public void Get_Invoices()
        {
            //When
            var invoices = Client.GetInvoicesForAccount(AccountId);

            //Then
            invoices.Should().NotBeNull("There should be a test invoice in the system, if not create one");
            invoices.Should().NotBeEmpty("Service returns a list of invoices.");
        }
        
    }
}