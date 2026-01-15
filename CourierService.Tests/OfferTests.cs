using CourierService.Domain;
using CourierService.Offers;
using CourierService.Services;

namespace CourierService.Tests
{
    public class OfferTests
    {
        /// <summary>
        /// Test Offer 1 Bounderies
        /// Weight should fall between 70-200
        /// Distance should fall between 1-200 (lessthan 200)
        /// </summary>
        [Theory]
        [InlineData(70, 199, true)]
        [InlineData(200, 100, true)]
        [InlineData(85, 165, true)]
        [InlineData(69, 100, false)]
        [InlineData(201, 100, false)]
        [InlineData(200, 201, false)]
        public void OFR001_EdgeCaseTest(double weight, double distance, bool expected)
        {
            var offer = new OFR001();
            var package = new Package
            {
                Weight = weight,
                Distance = distance,
            };

            Assert.Equal(expected, offer.IsApplicable(package));
        }

        /// <summary>
        /// Test Offer 2 Bounderies
        /// Weight should fall between 100-250
        /// Distance should fall between 50-150
        /// </summary>
        [Theory]
        [InlineData(100, 143, true)]
        [InlineData(250, 100, true)]
        [InlineData(200, 150, true)]
        [InlineData(99, 49, false)]
        [InlineData(151, 200, false)]
        [InlineData(251, 151, false)]
        public void OFR002_EdgeCaseTest(double weight, double distance, bool expected)
        {
            var offer = new OFR002();
            var package = new Package
            {
                Weight = weight,
                Distance = distance,
            };

            Assert.Equal(expected, offer.IsApplicable(package));
        }

        /// <summary>
        /// Test Offer 3 Bounderies
        /// Weight should fall between 10-150
        /// Distance should fall between 50-250
        /// </summary>
        [Theory]
        [InlineData(150, 250, true)]
        [InlineData(10, 50, true)]
        [InlineData(100, 150, true)]
        [InlineData(151, 251, false)]
        [InlineData(9, 49, false)]
        [InlineData(251, 151, false)]
        public void OFR003_EdgeCaseTest(double weight, double distance, bool expected)
        {
            var offer = new OFR003();
            var package = new Package
            {
                Weight = weight,
                Distance = distance,
            };

            Assert.Equal(expected, offer.IsApplicable(package));
        }
    }
}
