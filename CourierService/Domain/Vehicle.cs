using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Domain
{
    public class Vehicle
    {
        public double MaxLoad { get; }
        public double Speed { get; }
        public double AvailableAt { get; set; }

        public Vehicle(double maxLoad, double speed)
        {
            MaxLoad = maxLoad;
            Speed = speed;
            AvailableAt = 0;
        }
    }

}
