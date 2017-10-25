using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class ListViewModel
    {

        public int Id { get; set;  }
        public VehicleType Type { get; set; }
        public string RegNo { get; set; }
        public string Color { get; set; }
        public DateTime Date { get; set; }
       
        [Display(Name = "Parking Time Length")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:ss}", ApplyFormatInEditMode = true)]
        public TimeSpan ParkingTime{ get; set; }
    }
}