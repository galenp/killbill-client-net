using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using KillBill.Client.Net.Model;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.Creation
{
    [TestFixture]
    public class CreditCreationTests : BaseTestFixture
    {

        [Test]
        public void Add_Credit_To_Account()
        {
            //Given
            var credit = new Credit()
            {
                AccountId = AccountId,
                CreditAmount = Random.Next(100, 200)
            };

            //When
            var processed = Client.CreateCredit(credit, CreatedBy, Reason, "CreditCreationTests:Add_Credit_To_Account");

            //Then
            processed.Should().NotBeNull();
            processed.AccountId.Should().Be(AccountId);
            processed.CreditAmount.Should().BeInRange(100, 200);
        }
    }
}
