using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Receipt
    {
        public DateTime CheckoutTimestamp { get; set; }
        public Vehicle Vehicle { get; set; }
        public TimeSpan TotalParkingTime { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}