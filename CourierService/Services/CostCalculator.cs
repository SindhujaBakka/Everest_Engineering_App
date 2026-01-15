using CourierService.Domain;
using CourierService.Offers;

namespace CourierService.Services
{
    public class CostCalculator
    {
        private readonly OfferService _offerService;

        public CostCalculator(OfferService offerService)
        {
            _offerService = offerService;
        }

        public void CalculateCost(Package package, double baseCost)
        {
            var deliveryCost = baseCost + (package.Weight * 10) + (package.Distance * 5);

            var discount = _offerService.GetDiscount(package, deliveryCost);

            package.DeliveryCost = deliveryCost;
            package.Discount = discount;
            package.TotalCost = deliveryCost - discount;
        }
    }
}
