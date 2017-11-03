using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Receipt
    {
        [Display(Name = "Checkout Time")]
        public DateTime CheckoutTimestamp { get; set; }
        public Vehicle Vehicle { get; set; }
        [Display(Name = "Total Parking Time")]
        [DisplayFormat(DataFormatString = "{0:%d} day(s) {0:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan TotalParkingTime { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}