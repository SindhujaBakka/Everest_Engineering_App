using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierService.Domain;

namespace CourierService
{
    /// <summary>
    /// Helper class for all the business logic to perform Delivery Calculations
    /// </summary>
    public class DeliveryHelper
    {

        /// <summary>
        /// Calculate the Cost for a package including any applicable discounts
        /// </summary>
        /// <param name="baseCost">Base delivery cost</param>
        /// <param name="packages">Details of all the <see cref="Package"/></param>        
        public static Package CalculateCost(double baseCost, Package pkg)
        {

            // Base delivery cost calculation:
            // Base Delivery Cost + (Package Total Weight * 10) + (Distance to Destination * 5) = Delivery Cost
            pkg.DeliveryCost = baseCost + (pkg.Weight * 10) + (pkg.Distance * 5);

            pkg.Discount = 0;
            pkg.OfferApplied = false;

            // Apply offer if rule exists with valid code and criteria matches
            if (Program.OfferRules.ContainsKey(pkg.OfferCode) && Program.OfferRules[pkg.OfferCode](pkg))
            {
                pkg.Discount = pkg.DeliveryCost * Program.OfferDiscounts[pkg.OfferCode];
                pkg.OfferApplied = true;
            }

            pkg.TotalCost = pkg.DeliveryCost - pkg.Discount;


            return pkg;
        }


        /// <summary>
        /// Calculate the delivery times for all packages based on vehicle availability and load constraints
        /// </summary>
        /// <param name="packages">List of the all packages to be calculated</param>
        /// <param name="vehicleCount">No. of vehicles available</param>
        /// <param name="speed">Vehicle speed</param>
        /// <param name="maxLoad">Max weight to be carried in a vehicle</param>
        public static void CalculateDeliveryTimes(List<Package> packages, int vehicleCount, double speed, double maxLoad)
        {
            var vehicles = new List<Vehicle>();

            for (int i = 0; i < vehicleCount; i++)
            {
                vehicles.Add(new Vehicle(maxLoad, speed));
            }

            var remaining = new List<Package>(packages);

            while (remaining.Any())
            {
                var vehicle = vehicles.OrderBy(v => v.AvailableAt).First();

                double currentTime = vehicle.AvailableAt;

                var validCombinations = GetAllValidShipmentCombinations(remaining, maxLoad);

                var shipment = validCombinations
                                .OrderByDescending(s => s.Count)                    // Rule 1: fetch max packages
                                .ThenByDescending(s => s.Sum(p => p.Weight))        // Rule 2: filter by max weight
                                .ThenBy(s => s.Max(p => p.Distance))                // Rule 3: filter by earliest delivery
                                .First();

                double maxDistance = shipment.Max(p => p.Distance);
                double travelTime = maxDistance / speed;

                foreach (var pkg in shipment)
                {
                    double travel = Truncate(pkg.Distance / speed, 2);
                    pkg.DeliveryTime = Truncate(currentTime + travel, 2);
                    remaining.Remove(pkg);
                }

                vehicle.AvailableAt = Truncate(currentTime + (2 * travelTime), 2);
            }
        }


        /// <summary>
        /// Gets the subsets of all possible combinations of shipments based on their weights to the max load.
        /// </summary>
        /// <returns></returns>
        private static List<List<Package>> GetAllValidShipmentCombinations(List<Package> packages, double maxLoad)
        {
            var result = new List<List<Package>>();

            int n = packages.Count;

            // bitmasking to generate all subsets using inclusion-exclusion policy
            for (int mask = 1; mask < (1 << n); mask++)
            {
                var shipment = new List<Package>();
                double weight = 0;

                for (int i = 0; i < n; i++)
                {
                    if ((mask & (1 << i)) != 0)
                    {
                        shipment.Add(packages[i]);
                        weight += packages[i].Weight;
                    }
                }

                if (weight <= maxLoad)
                    result.Add(shipment);
            }

            return result;
        }

        /// <summary>
        /// Truncates a double value to specified decimal places without rounding.
        /// </summary>
        /// <returns></returns>
        private static double Truncate(double value, int decimals)
        {
            double factor = Math.Pow(10, decimals);
            return Math.Truncate(value * factor) / factor;
        }

    }
}
