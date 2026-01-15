using System;
using System.Collections.Generic;
using System.Linq;
using CourierService.Domain;

namespace CourierService
{
    public class Program
    {
        // Predefined offer applicability rule criteria
        public static readonly Dictionary<string, Func<Package, bool>> OfferRules =
            new Dictionary<string, Func<Package, bool>>
        {
            { "OFR001", p => p.Distance < 200 && p.Weight >= 70 && p.Weight <= 200 },
            { "OFR002", p => p.Distance >= 50 && p.Distance <= 150 && p.Weight >= 100 && p.Weight <= 250 },
            { "OFR003", p => p.Distance >= 50 && p.Distance <= 250 && p.Weight >= 10 && p.Weight <= 150 }
        };

        // Predefined offer discount percentages
        public static readonly Dictionary<string, double> OfferDiscounts =
            new Dictionary<string, double>
        {
            { "OFR001", 0.10 },
            { "OFR002", 0.07 },
            { "OFR003", 0.05 }
        };

        public static readonly bool isTestMode = true;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Courier Service Cost & Delivery Time Calculator ===\n");

            string pkgId;
            double baseCost, speed, maxLoad;
            int packageCount, vehicleCount;

            if (!isTestMode)
            {
                Console.Write("Enter base delivery cost and no. of packages: ");
                var input = Console.ReadLine().Split();
                baseCost = double.Parse(input[0]);
                packageCount = int.Parse(input[1]);
            }
            else
            {
                // Collect Base Delivery Cost and Package Details separately for better readability.
                Console.Write("Enter base delivery cost: ");
                baseCost = double.Parse(Console.ReadLine());

                Console.Write("Enter number of packages: ");
                packageCount = int.Parse(Console.ReadLine());
            }            

            var packages = new List<Package>();

            // Collect individual Package Details.
            for (int i = 0; i < packageCount; i++)
            {
                if (!isTestMode)
                {
                    Console.Write($"Enter details for Package {i + 1}:");
                    var parts = Console.ReadLine().Split();
                    packages.Add(new Package
                    {
                        Id = parts[0],
                        Weight = double.Parse(parts[1]),
                        Distance = double.Parse(parts[2]),
                        OfferCode = parts[3]
                    });
                }
                else
                {
                    pkgId = $"PKG{i + 1}";
                    Console.WriteLine($"\nEnter details for Package {pkgId}");

                    Console.Write("Weight (kg): ");
                    double weight = double.Parse(Console.ReadLine());

                    Console.Write("Distance (km): ");
                    double distance = double.Parse(Console.ReadLine());

                    Console.Write("Offer Code (OFR001 / OFR002 / OFR003 / NA): ");
                    string offerCode = Console.ReadLine();

                    packages.Add(new Package
                    {
                        Id = pkgId,
                        Weight = weight,
                        Distance = distance,
                        OfferCode = offerCode
                    });
                }
                
            }

            Console.WriteLine("\nEnter Vehicle Details: ");
            if (!isTestMode)
            {
                var vehicleInput = Console.ReadLine().Split();
                vehicleCount = int.Parse(vehicleInput[0]);
                speed = double.Parse(vehicleInput[1]);
                maxLoad = double.Parse(vehicleInput[2]);
            }
            else
            {
                // Collect Vehicle Details separately for better readability.                
                Console.Write("Number of vehicles: ");
                vehicleCount = int.Parse(Console.ReadLine());

                Console.Write("Vehicle speed (km/hr): ");
                speed = double.Parse(Console.ReadLine());

                Console.Write("Max carriable weight per vehicle (kg): ");
                maxLoad = double.Parse(Console.ReadLine());
            }

                        
            // Calculate costs for each package
            foreach (var pkg in packages)
            {
                DeliveryHelper.CalculateCost(baseCost, pkg);
            }

            // Calculate delivery times for each package
            DeliveryHelper.CalculateDeliveryTimes(packages, vehicleCount, speed, maxLoad);

            if (isTestMode)
            {
                // Build and display the calculated output in a summary format for better readability.
                Console.WriteLine("\n=== Package Delivery Summary ===\n");

                foreach (var pkg in packages)
                {
                    BuildOutput(pkg);
                    Console.WriteLine(pkg.Output);
                    Console.WriteLine("--------------------------------------------");
                }
            }

            Console.WriteLine("\n=== Package Delivery: Final Required Output Format ===\n");

            foreach (var pkg in packages)
            {
                Console.WriteLine($"{pkg.Id} {pkg.Discount:F0} {pkg.TotalCost:F0} {pkg.DeliveryTime:F2}");
            }

        }

        /// <summary>
        /// Builds the output string for a package in detailed summary format
        /// </summary>
        /// <param name="pkg">Details of the <see cref="Package"/></param>
        static void BuildOutput(Package pkg)
        {
            string offerStatus = pkg.OfferApplied ? $"Offer {pkg.OfferCode} applied" : "No offer applied";

            pkg.Output =
                $"Package Id    : {pkg.Id}\n" +
                $"Weight        : {pkg.Weight} kg\n" +
                $"Distance      : {pkg.Distance} km\n" +
                $"Delivery Cost : {pkg.DeliveryCost:F0}\n" +
                $"Discount      : {pkg.Discount:F0}\n" +
                $"Final Cost    : {pkg.TotalCost:F0}\n" +
                $"Delivery Time : {pkg.DeliveryTime:F2} hrs\n" +
                $"Offer Status  : {offerStatus}";
        }

    }

}
