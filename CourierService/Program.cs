using System;
using System.Collections.Generic;
using System.Linq;
using CourierService.Domain;
using CourierService.Input;
using CourierService.Offers;
using CourierService.Services;
using CourierService.Validation;

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
            var packages = new List<Package>();
            var vehicles = new List<Vehicle>();

            try
            {
                if (!isTestMode)
                {
                    Console.Write("Enter base delivery cost and no. of packages: ");
                    var input = Console.ReadLine().Split();
                    baseCost = double.Parse(input[0]);
                    packageCount = int.Parse(input[1]);
                }
                else
                {
                    baseCost = ConsoleInputReader.ReadDouble("Enter base delivery cost: ");
                    packageCount = ConsoleInputReader.ReadInt("Enter number of packages: ");
                }

                InputValidator.ValidateBaseCost(baseCost);

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

                        double weight = ConsoleInputReader.ReadDouble("Weight (kg): ");
                        double distance = ConsoleInputReader.ReadDouble("Distance (km): ");
                        string offerCode = ConsoleInputReader.ReadString("Offer Code (OFR001 / OFR002 / OFR003 / NA): ");

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
                    vehicleCount = ConsoleInputReader.ReadInt("Number of vehicles: ");
                    speed = ConsoleInputReader.ReadDouble("Vehicle speed (km/hr): ");
                    maxLoad = ConsoleInputReader.ReadDouble("Max carriable weight per vehicle (kg): ");
                }

                InputValidator.ValidateVehicleInputs(vehicleCount, speed, maxLoad);

                // Create vehicles            
                for (int i = 0; i < vehicleCount; i++)
                {
                    vehicles.Add(new Vehicle(maxLoad, speed));
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input error: {ex.Message}");
                return;
            }            

            // Register offers
            var offers = new List<IOffer>
            {
                new OFR001(),
                new OFR002(),
                new OFR003()
            };

            // Create services
            var shipmentPlanner = new ShipmentPlanner();
            var deliveryTimeCalculator = new DeliveryTimeCalculator(shipmentPlanner);
            var offerService = new OfferService(offers);
            var costCalculator = new CostCalculator(offerService);


            // Calculate costs for each package
            foreach (var pkg in packages)
            {
                InputValidator.ValidatePackage(pkg);
                costCalculator.CalculateCost(pkg, baseCost);
            }

            // Calculate delivery times for each package
            deliveryTimeCalculator.CalculateDeliveryTimes(packages, vehicles);

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
