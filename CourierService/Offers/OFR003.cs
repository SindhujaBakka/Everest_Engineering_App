using CourierService.Domain;

namespace CourierService.Offers
{
    public class OFR003 : IOffer
    {
        public string Code => "OFR003";

        public bool IsApplicable(Package p)
        {
            return p.Distance >= 50 &&
                   p.Distance <= 250 &&
                   p.Weight >= 10 &&
                   p.Weight <= 150;
        }

        public double CalculateDiscount(double deliveryCost)
        {
            return deliveryCost * 0.05;
        }
    }
}