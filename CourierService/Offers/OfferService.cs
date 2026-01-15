using CourierService.Domain;
using System.Collections.Generic;
using System.Linq;

namespace CourierService.Offers
{
    public class OfferService
    {
        private readonly IEnumerable<IOffer> _offers;

        public OfferService(IEnumerable<IOffer> offers)
        {
            _offers = offers;
        }

        public double GetDiscount(Package package, double deliveryCost)
        {
            if (string.IsNullOrWhiteSpace(package.OfferCode))
                return 0;

            var offer = _offers.FirstOrDefault(o => o.Code == package.OfferCode);

            if (offer == null)
                return 0;

            return offer.IsApplicable(package) ? offer.CalculateDiscount(deliveryCost) : 0;
        }
    }
}
