using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Domain
{
    /// <summary>
    /// Package details and calculated properties
    /// </summary>
    public class Package
    {
        public string Id { get; set; }
        public double Weight { get; set; }
        public double Distance { get; set; }
        public string OfferCode { get; set; }

        public double DeliveryCost { get; set; }
        public double Discount { get; set; }
        public double TotalCost { get; set; }
        public bool OfferApplied { get; set; }
        public double DeliveryTime { get; set; }

        public string Output { get; set; }
    }
}
