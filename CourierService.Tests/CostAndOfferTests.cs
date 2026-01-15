namespace CourierService.Tests
{
    public class CostAndOfferTests
    {
        [Fact]
        public void OFR003_Should_Apply_5Percent_Discount()
        {
            var pkg = new Package
            {
                Weight = 10,
                Distance = 100,
                OfferCode = "OFR003"
            };

            DeliveryHelper.CalculateCost(100, pkg);

            Assert.True(pkg.OfferApplied);
            Assert.Equal(35, pkg.Discount);
            Assert.Equal(665, pkg.TotalCost);
        }

        [Fact]
        public void Invalid_Offer_Should_Not_Apply_Discount()
        {
            var pkg = new Package
            {
                Weight = 5,
                Distance = 5,
                OfferCode = "OFR001"
            };

            DeliveryHelper.CalculateCost(100, pkg);

            Assert.False(pkg.OfferApplied);
            Assert.Equal(0, pkg.Discount);
            Assert.Equal(175, pkg.TotalCost);
        }

        [Fact]
        public void Invalid_OfferCode_Should_Not_Apply_Discount()
        {
            var pkg = new Package
            {
                Weight = 5,
                Distance = 5,
                OfferCode = "OFFER001"
            };

            DeliveryHelper.CalculateCost(100, pkg);

            Assert.False(pkg.OfferApplied);
            Assert.Equal(0, pkg.Discount);
            Assert.Equal(175, pkg.TotalCost);
        }

        [Fact]
        public void No_Offer_Code_Should_Give_No_Discount()
        {
            var pkg = new Package
            {
                Weight = 50,
                Distance = 30,
                OfferCode = "NA"
            };

            DeliveryHelper.CalculateCost(100, pkg);

            Assert.False(pkg.OfferApplied);
            Assert.Equal(0, pkg.Discount);
        }
    }
}
