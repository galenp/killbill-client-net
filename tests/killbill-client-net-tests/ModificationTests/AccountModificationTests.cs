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
       
        public void Create_Account()
        {
            //When
            var createdAccount = Client.CreateAccount(account, userName, reason, comment);

            //Then
            createdAccount.Should().NotBeNull();
            createdAccount.ExternalKey.Should().Be(account.ExternalKey);
            accountId = createdAccount.AccountId;
        }
       

        [Test]
       
        public void Get_Single_Account()
        {
            //Given
            accountId.Should().NotBeEmpty();

            //When
            var getAccount = Client.GetAccount(accountId);

            //Then
            getAccount.Should().NotBeNull();
            getAccount.AccountId.Should().Be(accountId);
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
    }
}
