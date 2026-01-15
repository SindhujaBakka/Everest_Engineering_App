using CourierService.Domain;

namespace CourierService.Offers
{
    public interface IOffer
    {
        string Code { get; }
        bool IsApplicable(Package package);
        double CalculateDiscount(double deliveryCost);
    }
}