using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class ParkingLot
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string Description { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}