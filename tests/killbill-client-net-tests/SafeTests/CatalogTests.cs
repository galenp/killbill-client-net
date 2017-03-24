using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace KillBill.Client.Net.Tests.SafeTests
{

    [TestFixture]
    public class CatalogTests : BaseTestFixture
    {

        [Test]
        public void Get_Catalog_Json()
        {
            //When
            var catalogs = Client.GetCatalogJson(Options);

            //Then
            if (catalogs == null)
                Assert.Inconclusive("Catalogs not found.");

            catalogs.Should().NotBeNullOrEmpty();
            var catalog = catalogs.First();
            catalog.Currencies.Should().NotBeNullOrEmpty();
            catalog.Name.Should().NotBeNullOrEmpty();
            catalog.PriceLists.Should().NotBeNullOrEmpty();

            var priceList = catalog.PriceLists.First();
            priceList.Name.Should().NotBeNullOrEmpty();
            priceList.Plans.Should().NotBeNullOrEmpty();

        }
    }
}