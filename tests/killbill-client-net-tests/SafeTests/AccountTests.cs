using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.SafeTests
{
    [TestFixture]
    public class AccountTests : BaseTestFixture
    {
        [Test]
        public void Get_Account()
        {
            //When
            var account = Client.GetAccount(AccountId, Options);

            //Then
            if (account == null)
                Assert.Inconclusive("Account not found.");

            account.Should().NotBeNull();
            account.AccountId.Should().Be(AccountId);
        }

      [Test]
        public void Get_Accounts()
        {
            //When
            var accounts = Client.GetAccounts(Options);

            //Then
            if (!accounts.Any())
                Assert.Inconclusive("No accounts found.");

        }

        [Test]
        public void Get_Bundles()
        {
            //When
            var bundles = Client.GetAccountBundles(AccountId, Options);

            //Then
            if (!bundles.Any())
                Assert.Inconclusive("No bundles found for account.");

       
            Console.WriteLine($"Found {bundles.Count} bundles for account");
            var firstBundle = bundles.First();
            Console.WriteLine($"Testing first bundle with key - {firstBundle.ExternalKey}");
            firstBundle.AccountId.Should().Be(AccountId);
            firstBundle.ExternalKey.Should().NotBeNullOrEmpty();

        }

        [Test]
        public void Get_Invoices()
        {
            //When
            var invoices = Client.GetInvoicesForAccount(AccountId, Options);

            //Then
            if (!invoices.Any())
                Assert.Inconclusive("No invoices found for account.");

            Console.WriteLine($"Found {invoices.Count} invoices for account {AccountId}");
            var invoice = invoices.First();
            invoice.AccountId.Should().Be(AccountId);
            invoice.InvoiceId.Should().NotBe(Guid.Empty);

        }


        [Test]
        public void Get_Account_Timeline()
        {
            //When
            var timeline = Client.GetAccountTimeline(AccountId, Options);

            //Then
            timeline.Should().NotBeNull();
            timeline.Account.Should().NotBeNull();
            timeline.Account.AccountId.Should().Be(AccountId);

        }

        [Test]
        public void Get_Emails_For_Account()
        {
            //When
            var emails = Client.GetEmailsForAccount(AccountId, Options);

            //Then
            if (!emails.Any())
                Assert.Inconclusive("No emails found for account.");

            emails.First().AccountId.Should().Be(AccountId);
        }

        [Test]      
        public void Get_Payments_For_Account()
        {
            //When
            var payments = Client.GetPaymentsForAccount(AccountId, Options);

            //Then
            if (!payments.Any())
                Assert.Inconclusive("No payments found for account.");

            payments.First().AccountId.Should().Be(AccountId);
        }

        [Test]
        public void Get_InvoicePayments_For_Account()
        {
            //When
            var invoicePayments = Client.GetInvoicePaymentsForAccount(AccountId, Options);

            //Then
            if (!invoicePayments.Any())
                Assert.Inconclusive("No invoice payments found for account.");

            invoicePayments.First().AccountId.Should().Be(AccountId);

        }
        
    }
}