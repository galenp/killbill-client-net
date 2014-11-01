using System;
using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests
{
    [TestFixture]
    public class InvoiceTests : BaseTestFixture
    {

        [TestCase("5149bcae-f0de-4bc8-9b32-a6e1bc5f83ed")]
        public void Get_Invoice_By_Id(string invoiceId)
        {
            //When
            var invoice = Client.GetInvoice(invoiceId);

            //Then
            invoice.Should().NotBeNull();
            invoice.InvoiceId.Should().Be(invoiceId);
            invoice.Balance.Should().BeGreaterThan(0);
        }

        [TestCase(1)]
        public void Get_Invoice_By_Number(int invoiceNumber)
        {
            //When
            var invoice = Client.GetInvoice(invoiceNumber);

            //Then
            invoice.Should().NotBeNull();
            invoice.InvoiceNumber.Should().Be(invoiceNumber);
            invoice.Balance.Should().BeGreaterThan(0);
        }
         
    }
}