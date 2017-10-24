using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Receipt
    {
        public DateTime CheckoutTimestamp { get; set; }
        public Vehicle Vehicle { get; set; }
        public TimeSpan TotalParkingTime { get; set; }
        public decimal Price { get; set; }
    }
}