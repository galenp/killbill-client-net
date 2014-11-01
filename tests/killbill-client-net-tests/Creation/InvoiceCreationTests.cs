using System;
using System.Collections.Generic;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.Creation
{

    [TestFixture]
    public class InvoiceCreationTests : BaseTestFixture
    {

        [Test]
        public void Create_External_Charge()
        {
          
            //Given
            var externalCharges = new List<InvoiceItem>()
            {
                new InvoiceItem
                {
                    AccountId = AccountId,
                    Amount = 100,
                    Currency = "AUD",
                    Description = "LINE ITEM 1",
                   
                }, 
                new InvoiceItem
                {
                    AccountId = AccountId,
                    Amount = 200,
                    Currency = "AUD",
                    Description = "LINE ITEM 2",
                   
                }, 
            };

            //When
            var invoiceItems = Client.CreateExternalCharge(externalCharges, DateTime.Now, false, "Testing User", "api tests", "InvoiceCreationTests:Create_External_Charge");

            //Then
            invoiceItems.Should().NotBeNull();
            invoiceItems.Should().NotBeEmpty();
            invoiceItems.Count.Should().Be(2);
        }
         
    }
}