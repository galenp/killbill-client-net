using System;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{
    [TestFixture]
    public class PaymentModificationTests : BaseTestFixture
    {

        [TestCase("1524e2d3-cd26-4714-a105-e3983dcfded6")]
        public void Create_Payment(string paymentMethodString)
        {
            //Given
            var paymentMethodId = Guid.Parse(paymentMethodString);
            var paymentTransaction = new PaymentTransaction
            {
                Amount = 550,
                Currency = "AUD",
                EffectiveDate = DateTime.UtcNow,
                TransactionType = "AUTHORIZE",
                TransactionExternalKey = Guid.NewGuid().ToString()

            };
            //When
            var payment = Client.CreatePayment(AccountId, paymentMethodId, paymentTransaction, CreatedBy, Reason, "PaymentModificationTests:Create_Payment");


            //Then
            payment.Should().NotBeNull();
        }
         
    }
}