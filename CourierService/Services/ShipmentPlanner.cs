using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierService.Domain;

namespace CourierService.Services
{
    public class ShipmentPlanner
    {
        /// <summary>
        /// Get the best shipment follwing the rules mentioned below
        /// Rule 1: fetch max packages
        /// Rule 2: filter by max weight
        /// Rule 3: filter by earliest delivery
        /// </summary>
        /// <returns></returns>
        public List<Package> GetShipment(List<Package> packages, double maxLoad)
        {
            var validCombinations = GetAllValidShipmentCombinations(packages, maxLoad);

            var shipment = validCombinations
                            .OrderByDescending(s => s.Count)                    // Rule 1: fetch max packages
                            .ThenByDescending(s => s.Sum(p => p.Weight))        // Rule 2: filter by max weight
                            .ThenBy(s => s.Max(p => p.Distance))                // Rule 3: filter by earliest delivery
                            .First();

            return shipment;
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

    }
}
