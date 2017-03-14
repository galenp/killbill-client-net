using System;
using System.Collections.Generic;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.ModificationTests
{

    [TestFixture]
    public class InvoiceModificationTests : BaseTestFixture
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
            var invoiceItems = Client.CreateExternalCharges(externalCharges, DateTime.Now, false, false, Options);

            //Then
            invoiceItems.Should().NotBeNull();
            invoiceItems.Should().NotBeEmpty();
            invoiceItems.Count.Should().Be(2);
        }
         
    }
}