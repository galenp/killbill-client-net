using System;
using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{
    [TestFixture]
    public class TenantModificationTests : BaseTestFixture
    {

        [Test]
        public void Register_Notification_Callback()
        {
            //Given
            var callback = "http://192.165.56.1:8080/notsurewhatiexpecttosee";

            //When
            var tenantKey = Client.RegisterCallBackNotificationForTenant(callback, Options);

            //Then
            tenantKey.Should().NotBeNull();
        }


        [TestCase("d632f46a-15cf-409a-83c1-34390b983a12")]
        public void Unregister_Notification_Callback(string tenantIdString)
        {
            //Given
            var tenantId = Guid.Parse(tenantIdString);

            //When
            Client.UnregisterCallbackNotificationForTenant(tenantId, Options);
        }

        [Test]
        public void Retrieve_Notification_Callbacks()
        {
            //When
            var tenantKey = Client.GetCallbackNotificationForTenant(Options);

            //Then
            tenantKey.Should().NotBeNull("Because in the above test we registered a new one");
            tenantKey.Values.Should().NotBeEmpty("Because it should atleast have the callback in the above test");
        }
       
    }
}