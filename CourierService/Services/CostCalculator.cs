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

        /// <summary>
        /// Calculate the Cost for a package including any applicable discounts
        /// </summary>
        /// <param name="baseCost">Base delivery cost</param>
        /// <param name="package">Details of the <see cref="Package"/></param> 
        public void CalculateCost(Package package, double baseCost)
        {
            var deliveryCost = baseCost + (package.Weight * 10) + (package.Distance * 5);

            var discount = _offerService.GetDiscount(package, deliveryCost);

            package.DeliveryCost = deliveryCost;
            package.Discount = discount;
            package.TotalCost = deliveryCost - discount;
            package.OfferApplied = discount > 0 ? true : false;
        }
    }
}
