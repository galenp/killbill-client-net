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

        [Test]
        public void Get_Base_Plans()
        {
            //When
            var plans = Client.GetBasePlans(Options);

            if (plans == null)
                Assert.Inconclusive("No base plans found");

            plans.Should().NotBeNullOrEmpty();

            var plan = plans.First();
            plan.Product.Should().NotBeNullOrEmpty();
            plan.PriceList.Should().NotBeNullOrEmpty();
            plan.Plan.Should().NotBeNullOrEmpty();

            plan.FinalPhaseRecurringPrice.Should().NotBeNullOrEmpty();
            var price = plan.FinalPhaseRecurringPrice.First();
            price.Currency.Should().NotBeNullOrEmpty();
        }
    }
}