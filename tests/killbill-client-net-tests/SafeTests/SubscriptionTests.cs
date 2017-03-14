using System;
using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.SafeTests
{
      [TestFixture]
    public class SubscriptionTests : BaseTestFixture
    {
          [TestCase("43ef23dd-caa1-4e16-8f62-0ae799b6dc68")]
          public void Get_Subscription(string strId)
          {
              //Given
              var subscriptionId = Guid.Parse(strId);

              //When
              var subscription = Client.GetSubscription(subscriptionId, Options);

              //Then
              subscription.Should().NotBeNull();
              subscription.SubscriptionId.Should().Be(subscriptionId);
          }
         
    }
}