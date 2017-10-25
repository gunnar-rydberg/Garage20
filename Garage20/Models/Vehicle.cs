using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Vehicle
    {

        public int Id { get; set; }
        public VehicleType Type { get; set; }
        [Display(Name = "Reg.No")]
        [Required]
        public string RegNo { get; set; }
        public string Color { get; set; }
       
        [Range(0, 500, ErrorMessage = "Insert positive integers")]
        public int NoWheels { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        [Display(Name = "Check in time")]
        public DateTime Date { get; set; }
                                          
        public virtual ICollection<ParkingLot> ParkingLots { get; set; }
    }
}