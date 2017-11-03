using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Type")]
        public string Name { get; set; }
        [Required]
        public decimal NumberOfParkingLots { get; set; }
    }
}