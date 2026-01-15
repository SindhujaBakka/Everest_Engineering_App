using CourierService.Domain;
using CourierService.Offers;
using CourierService.Services;

namespace CourierService.Tests
{
    /// <summary>
    /// Performs Package Delivery Cost Tests
    /// </summary>
    public class CostAndOfferTests
    {

        [Fact]
        public void OFR003_Should_Apply_5Percent_Discount()
        {
            var offers = new List<IOffer> { new OFR003() };
            var offerService = new OfferService(offers);
            var costCalculator = new CostCalculator(offerService);

            var pkg = new Package
            {
                Weight = 10,
                Distance = 100,
                OfferCode = "OFR003"
            };

            costCalculator.CalculateCost(pkg, 100);
            
            Assert.True(pkg.OfferApplied);
            Assert.Equal(35, pkg.Discount);
            Assert.Equal(665, pkg.TotalCost);
        }

        [Fact]
        public void Invalid_Offer_Should_Not_Apply_Discount()
        {
            var offers = new List<IOffer> { new OFR001() };
            var offerService = new OfferService(offers);
            var costCalculator = new CostCalculator(offerService);

            var pkg = new Package
            {
                Weight = 5,
                Distance = 5,
                OfferCode = "OFR001"
            };

            costCalculator.CalculateCost(pkg, 100);

            Assert.False(pkg.OfferApplied);
            Assert.Equal(0, pkg.Discount);
            Assert.Equal(175, pkg.TotalCost);
        }

        [Fact]
        public void Invalid_OfferCode_Should_Not_Apply_Discount()
        {
            var offers = new List<IOffer> { new OFR001() };
            var offerService = new OfferService(offers);
            var costCalculator = new CostCalculator(offerService);

            var pkg = new Package
            {
                Weight = 5,
                Distance = 5,
                OfferCode = "OFFER001"
            };

            costCalculator.CalculateCost(pkg, 100);

            Assert.False(pkg.OfferApplied);
            Assert.Equal(0, pkg.Discount);
            Assert.Equal(175, pkg.TotalCost);
        }

        [Fact]
        public void No_Offer_Code_Should_Give_No_Discount()
        {
            var offers = new List<IOffer> { new OFR001() };
            var offerService = new OfferService(offers);
            var costCalculator = new CostCalculator(offerService);

            var pkg = new Package
            {
                Weight = 50,
                Distance = 30,
                OfferCode = "NA"
            };

            costCalculator.CalculateCost(pkg, 100);

            Assert.False(pkg.OfferApplied);
            Assert.Equal(0, pkg.Discount);
        }
    }
}
