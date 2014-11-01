using System;
using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests
{
    [TestFixture]
    public class InvoiceTests : BaseTestFixture
    {

        [TestCase("bb279232-c378-41de-be13-653850583485")]
        public void Get_Invoice(string invoiceId)
        {
            //Given
            
            //When
            var invoice = Client.GetInvoice(invoiceId, false, AuditLevel.NONE);

            //Then
            invoice.Should().NotBeNull();
            invoice.InvoiceId.Should().Be(invoiceId);
            invoice.Balance.Should().BeGreaterThan(0);
        }
         
    }
}