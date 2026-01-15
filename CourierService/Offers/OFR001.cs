using CourierService.Domain;

namespace CourierService.Offers
{
    public class OFR001 : IOffer
    {
        public string Code => "OFR001";

        public bool IsApplicable(Package p)
        {
            return p.Distance < 200 &&
                   p.Weight >= 70 &&
                   p.Weight <= 200;
        }

        public double CalculateDiscount(double deliveryCost)
        {
            return deliveryCost * 0.10;
        }
    }
}