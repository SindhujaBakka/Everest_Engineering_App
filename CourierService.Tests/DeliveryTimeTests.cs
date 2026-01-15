using System.Collections.Generic;
using CourierService.Domain;
using Xunit;

namespace CourierService.Tests
{
    /// <summary>
    /// Performs calculation for Package Delivery Time tests.
    /// </summary>
    public class DeliveryTimeTests
    {
        [Fact]
        public void DeliveryTime_Should_Be_Calculated_For_Package()
        {
            var packages = new List<Package>
            {
                new Package { Id = "PKG1", Weight = 50, Distance = 70 }
            };

            DeliveryHelper.CalculateDeliveryTimes(packages, vehicleCount: 1, speed: 70, maxLoad: 200);

            Assert.True(packages[0].DeliveryTime > 0);
            Assert.Equal(1.0, packages[0].DeliveryTime, 2);
        }
    }
}
