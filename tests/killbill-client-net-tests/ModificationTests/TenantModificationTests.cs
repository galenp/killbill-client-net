using System;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{
    [TestFixture]
    public class TenantModificationTests : BaseTestFixture
    {

        [Test]
        public void Create_Tenant()
        {
            //Given
            var apiKey = Guid.NewGuid().ToString();
            var externalKey = Guid.NewGuid().ToString();            
            var tenant = new Tenant()
            {
                ApiKey = apiKey,
                ApiSecret = Guid.NewGuid().ToString(),
                ExternalKey = externalKey
            };

            //When
            var processed = Client.CreateTenant(tenant, CreatedBy, Reason, "TenantModificationTests:Create_Tenant");

            //Then
            processed.Should().NotBeNull();
            processed.TenantId.Should().NotBeEmpty();
            processed.ApiKey.Should().Be(apiKey);
        }

        [Test]
        public void Register_Notification_Callback()
        {
            //Given
            var callback = "http://192.165.56.1:8080/notsurewhatiexpecttosee";

            //When
            var tenantKey = Client.RegisterCallBackNotificationForTenant(callback, CreatedBy, Reason, "TenantModificationTests:Register_Notification_Callback");

            //Then
            tenantKey.Should().NotBeNull();
        }

        [Test]
        public void Retrieve_Notification_Callbacks()
        {
            //When
            var tenantKey = Client.RetrieveRegisteredCallBacks();

            //Then
            tenantKey.Should().NotBeNull("Because in the above test we registered a new one");
            tenantKey.Values.Should().NotBeEmpty("Because it should atleast have the callback in the above test");
        }
       
    }
}