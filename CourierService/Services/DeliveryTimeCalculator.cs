using CourierService.Domain;
using System.Collections.Generic;
using System.Linq;

namespace CourierService.Services
{
    public class DeliveryTimeCalculator
    {
        private readonly ShipmentPlanner _shipmentPlanner;

        public DeliveryTimeCalculator(ShipmentPlanner shipmentPlanner)
        {
            _shipmentPlanner = shipmentPlanner;
        }

        /// <summary>
        /// Calculate the delivery times for all packages based on vehicle availability and load constraints
        /// </summary>
        /// <param name="packages">List of the all packages to be calculated</param>
        /// <param name="vehicles">List of Vehicles</param>
        public void CalculateDeliveryTimes(List<Package> packages, List<Vehicle> vehicles)
        {
            var remaining = new List<Package>(packages);

            while (remaining.Any())
            {
                var vehicle = vehicles.OrderBy(v => v.AvailableAt).First();

                var shipment = _shipmentPlanner.GetShipment(remaining, vehicle.MaxLoad);

                double currentTime = vehicle.AvailableAt;
                double maxDistance = shipment.Max(p => p.Distance);
                double travelTime = maxDistance / vehicle.Speed;

                foreach (var pkg in shipment)
                {
                    pkg.DeliveryTime = Truncate(currentTime + (pkg.Distance / vehicle.Speed), 2);
                    remaining.Remove(pkg);
                }

                vehicle.AvailableAt = Truncate(currentTime + (2 * travelTime), 2);
            }
        }

        /// <summary>
        /// Truncates a double value to specified decimal places without rounding.
        /// </summary>
        /// <returns></returns>
        private double Truncate(double value, int decimals)
        {
            double factor = System.Math.Pow(10, decimals);
            return System.Math.Truncate(value * factor) / factor;
        }
    }
}
