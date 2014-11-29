using System;
using System.Configuration;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests._SetUp
{
    [TestFixture]
    public class NewEnvironmentTests : BaseTestFixture
    {
        /// <summary>
        /// Run this test and then save the TenantId in BaseTestFixture
        /// </summary>
        [Test]
        public void Create_Tenant()
        {
            //Given
            var apiKey = ConfigurationManager.AppSettings["kb.api.key"];
            var externalKey = Guid.NewGuid().ToString();
            var tenant = new Tenant()
            {
                ApiKey = apiKey,
                ApiSecret = ConfigurationManager.AppSettings["kb.api.secret"],
                ExternalKey = externalKey
            };

            //When
            var newTenant = Client.CreateTenant(tenant, CreatedBy, Reason, "NewEnvironmentTests:Create_Tenant");

            //Then
            newTenant.Should().NotBeNull();
            newTenant.TenantId.Should().NotBeEmpty();
            newTenant.ApiKey.Should().Be(apiKey);
            Console.WriteLine("TENANTID: " + newTenant.TenantId);
        }


        /// <summary>
        /// Run this test and then save the AccountId in BaseTestFixture
        /// </summary>
        [Test]

        public void Create_Account()
        {
            //Given
            var account = new Account()
            {
                ExternalKey = Guid.NewGuid().ToString(),
                Name = "AU Account",
                Email = "testing@propertycompass.com.au",
                Currency = "AUD",
                Address1 = "Test Address 1",
                Address2 = "Test Address 2",

            };

            //When
            var createdAccount = Client.CreateAccount(account, CreatedBy, Reason, "NewEnvironmentTests:Create_Account");

            //Then
            createdAccount.Should().NotBeNull();
            createdAccount.ExternalKey.Should().Be(account.ExternalKey);
            Console.WriteLine("ACCOUNNTID: " + createdAccount.AccountId);
        }
    }
}
