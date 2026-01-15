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

        private double Truncate(double value, int decimals)
        {
            double factor = System.Math.Pow(10, decimals);
            return System.Math.Truncate(value * factor) / factor;
        }
    }
}
