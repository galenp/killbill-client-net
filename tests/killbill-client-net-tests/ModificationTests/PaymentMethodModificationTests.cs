using System;
using System.Collections.Generic;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{
    [TestFixture]
    public class PaymentMethodModificationTests : BaseTestFixture
    {

        [TestCase("tok_154JgSC746zpV9sQFB1h4ACJ")]
        public void Create_PaymentMethod_Stripe(string stripeToken)
        {
            //Given
            var stripePaymentMethod = new PaymentMethod
            {
                AccountId = AccountId,
                PluginName = "killbill-stripe",
                PluginInfo = new PaymentMethodPluginDetail
                {
                    Properties = new List<PluginProperty>
                    {
                        new PluginProperty
                        {
                            Key = "token",
                            Value = stripeToken
                        }
                    }
                }
            };

            //When
            var paymentMethod = Client.CreatePaymentMethod(stripePaymentMethod, Options);

            //Then
            paymentMethod.Should().NotBeNull();
            paymentMethod.AccountId.Should().Be(AccountId);
            paymentMethod.PaymentMethodId.HasValue.Should().BeTrue();
            paymentMethod.PaymentMethodId.Should().NotBe(Guid.Empty);
        }

     
       
    }
}