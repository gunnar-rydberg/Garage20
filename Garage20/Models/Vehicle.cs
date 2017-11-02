using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Vehicle
    {

        public int Id { get; set; }

        public VehicleTypeEnum Type { get; set; } //TODO Remove me

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

//        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
//        [ForeignKey("Member")]
        public int MemberId { get; set; }


        public virtual VehicleType VehicleType { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<ParkingLot> ParkingLots { get; set; }
    }
}