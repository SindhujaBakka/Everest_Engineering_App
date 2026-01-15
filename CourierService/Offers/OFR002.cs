using CourierService.Domain;

namespace CourierService.Offers
{
    public class OFR002 : IOffer
    {
        public string Code => "OFR002";

        public bool IsApplicable(Package p)
        {
            return p.Distance >= 50 &&
                   p.Distance <= 150 &&
                   p.Weight >= 100 &&
                   p.Weight <= 250;
        }

        public double CalculateDiscount(double deliveryCost)
        {
            return deliveryCost * 0.07;
        }
    }
}