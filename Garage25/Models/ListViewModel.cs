using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class ListViewModel
    {
        public Vehicle vehicle { get; set; }

        [Display(Name = "Parking Time Length")]
        [DisplayFormat(DataFormatString = "{0:%d} day(s) {0:hh} hour(s)", ApplyFormatInEditMode = true)]
        public TimeSpan ParkingTime{ get; set; }
    }
}