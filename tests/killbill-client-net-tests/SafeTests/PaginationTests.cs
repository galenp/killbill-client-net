using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.SafeTests
{
    [TestFixture]
    public class PaginationTests : BaseTestFixture
    {

        [Test]
        public void Next_Uri_Link_Is_Correct()
        {
            //Given
            const int limit = 1;
            const int offset = 0;

            //When
            var accounts = Client.GetAccounts(offset, limit);

            //Then
            accounts.Should().NotBeNull("Because even in empty situations we return a blank Accounts object");
            accounts.Count.Should().BeLessOrEqualTo(limit);
            accounts.PaginationMaxNbRecords.Should().BeGreaterThan(1, "Because we should have more than 1 account as test data");
            accounts.PaginationNextPageUri.Should().NotBeEmpty("Because with a limit of 1 there should be more data to trigger paging");

            var secondPage = accounts.GetNext();
            secondPage.Should().NotBeNull();
        }
    }
}
