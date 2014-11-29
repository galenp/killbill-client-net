using System;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{
    [TestFixture]
    public class AccountModificationTests : BaseTestFixture
    {
     
        private Account account;

        private readonly string userName = "Test User";
        private readonly string reason = "Account testing";
        private readonly string comment = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private Guid accountId = Guid.Empty;


        [SetUp]
        
        public void SetUp()
        {
            account = new Account()
            {                
                ExternalKey = Guid.NewGuid().ToString(),
                Name = "AU Account",
                Email = "testing@propertycompass.com.au",
                Currency = "AUD",
                Address1 = "Test Address 1",
                Address2 = "Test Address 2",

            };

          
        }

       

        [Test]
        
        public void Update_Account()
        {
            //Given
            const string newAddress1 = "NEW ADDRESS 1";
            const string newAddress2 = "NEW ADDRESS 2";

            var updateAccount = new Account
            {
                AccountId = AccountId,
                Address1 = newAddress1,
                Address2 = newAddress2
            };
            

            //When
            var updatedAccount = Client.UpdateAccount(updateAccount, userName, "updated account", "changed address");

            //Then
            updatedAccount.Should().NotBeNull();
            updatedAccount.Address1.Should().Be(newAddress1);
            updatedAccount.Address2.Should().Be(newAddress2);
        }

        [Test]
        public void Add_Email_To_Account()
        {
            //Given
            var email = new AccountEmail {AccountId = AccountId, Email = "tester@test.com"};

            //When
            Client.AddEmailToAccount(email, CreatedBy, Reason, "AccountModificationTests:Add_Email_To_Account");

            //Then -- it should not error.....
        }



        //This test is currently failing because the http server running my KB I think is blocked DELETE methods.
        //HTTP/1.1 405 Method Not Allowed
        //Allow: POST,GET,OPTIONS,HEAD
        //Have verified at http://killbill.io/api/#!/accounts/removeEmail that the process works so this is just a configuration issue.
        [Test]
        [Ignore]
        public void Remove_Email_From_Account()
        {
            //Given
            var email = new AccountEmail { AccountId = AccountId, Email = "tester@test.com" };

            //When
            Client.RemoveEmailFromAccount(email, CreatedBy, Reason, "AccountModificationTests:Remove_Email_From_Account");

            //Then -- it should not error.....
        }


        [TestCase(true)]
        [TestCase(false)]
        public void Update_Email_Notifications_For_Account(bool notificationSetting)
        {
            //Given
            var invoiceEmail = new InvoiceEmail {AccountId = AccountId, IsNotifiedForInvoices = notificationSetting};

            //When
            Client.UpdateEmailNotificationsForAccount(invoiceEmail, CreatedBy, Reason, "AccountModificationTests:Update_Email_Notifications_For_Account");

            //Then
            var setting = Client.GetEmailNotificationsForAccount(AccountId);
            setting.Should().NotBeNull();
            setting.AccountId.Should().Be(AccountId);
            setting.IsNotifiedForInvoices.Should().Be(notificationSetting);
        }
    }
}
