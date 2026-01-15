using CourierService.Domain;
using System;

namespace CourierService.Validation
{
    public static class InputValidator
    {
        public static void ValidatePackage(Package package)
        {
            if (string.IsNullOrWhiteSpace(package.Id))
                throw new ArgumentException("Package Id is required.");

            if (package.Weight <= 0)
                throw new ArgumentException(
                    $"Package {package.Id}: Weight must be greater than zero.");

            if (package.Distance <= 0)
                throw new ArgumentException(
                    $"Package {package.Id}: Distance must be greater than zero.");
        }

        public static void ValidateBaseCost(double baseCost)
        {
            if (baseCost < 0)
                throw new ArgumentException("Base delivery cost cannot be negative.");
        }

        public static void ValidateVehicleInputs(
            int vehicleCount,
            double speed,
            double maxLoad)
        {
            if (vehicleCount <= 0)
                throw new ArgumentException("Number of vehicles must be greater than zero.");

            if (speed <= 0)
                throw new ArgumentException("Vehicle speed must be greater than zero.");

            if (maxLoad <= 0)
                throw new ArgumentException("Vehicle max load must be greater than zero.");
        }
    }
}