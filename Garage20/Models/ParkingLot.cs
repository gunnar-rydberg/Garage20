using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class ParkingLot
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public int? MotorCycleId1 { get; set; }
        public int? MotorCycleId2 { get; set; }
    }
}