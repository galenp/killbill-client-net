﻿using System;
using NUnit.Framework;
using FluentAssertions;

namespace KillBill.Client.Net.Tests.SafeTests
{
    [TestFixture]
    public class PaymentMethodTests : BaseTestFixture
    {

        [TestCase("035ccf7d-8015-4296-97a5-91571321ba1c")]
        public void Get_PaymentMethod(string paymentMethodString)
        {
            //Given
            var paymentMethodId = Guid.Parse(paymentMethodString);
            
            //When
            var paymentMethod = Client.GetPaymentMethod(paymentMethodId, Options);
            
            //Then
            paymentMethod.Should().NotBeNull();
            paymentMethod.PaymentMethodId.Should().Be(paymentMethodId);

        }


        [Test]
        public void Get_PaymentMethodsForAccount()
        {
            //When
            var paymentMethods = Client.GetPaymentMethodsForAccount(AccountId, Options);

            //Then
            paymentMethods.Should().NotBeNull();
            paymentMethods.Should().NotBeEmpty();
        }
         
    }
}