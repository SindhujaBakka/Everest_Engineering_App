using CourierService.Domain;

namespace CourierService.Services
{
    public class CostCalculator
    {
        public void CalculateCost(Package package, double baseCost)
        {
            var deliveryCost =
                baseCost +
                (package.Weight * 10) +
                (package.Distance * 5);

            package.DeliveryCost = deliveryCost;
        }
    }
}
