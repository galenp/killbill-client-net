using System;
using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.SafeTests
{
      [TestFixture]
    public class SubscriptionTests : BaseTestFixture
    {
          [TestCase("fab1f507-d43c-4f9d-8ec3-070414512c45")]
          public void Get_Subscription(string strId)
          {
              //Given
              var subscriptionId = Guid.Parse(strId);

              //When
              var subscription = Client.GetSubscription(subscriptionId, Options);

              //Then
              if (subscription == null)
                Assert.Inconclusive("Could not find subscription in KBill");

              subscription.SubscriptionId.Should().Be(subscriptionId);
          }
         
    }
}