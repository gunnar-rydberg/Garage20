using Garage20.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Garage20.Utility
{
    public class GarageHandler
    {
        private Garage20Context db; // = new Garage20Context();

        public GarageHandler(Garage20Context dbContext)
        {
            db = dbContext;
        }

        public int FreeCapacity
        {
            get
            {
                return TotalCapacity - db.ParkingLots.Where(x => x.Vehicles.Any()).Count();
            }
        }
        public int TotalCapacity
        {
            get
            {
                return db.ParkingLots.Count();
            }
        }

        /// <summary>
        /// Park vehicle in garage
        /// </summary>
        /// <param name="vehicle"></param>
        public void Park(Vehicle vehicle)
        {
            var parkingSpace = findFreeParkingSpace();
            vehicle.ParkingLots = new List<ParkingLot>();
            vehicle.ParkingLots.Add(parkingSpace);

            db.Vehicles.Add(vehicle);
            db.SaveChanges();
        }

        /// <summary>
        /// Remove vehicle from garage, clearing parking space(s)
        /// </summary>
        /// <param name="vehicle"></param>
        public void CheckOut(Vehicle vehicle)
        {
            //Remove from parking space
            foreach(var parkingSpace in vehicle.ParkingLots)
            {
                //var vehicleToCheckOut = parkingSpace.Vehicles.First(x => x.Id == vehicle.Id);
                //parkingSpace.Vehicles.Remove(vehicleToCheckOut);
                parkingSpace.Vehicles.Remove(vehicle);
                //TODO trow exception if ParkingLots is empty on checkout
            }

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
        }

        private ParkingLot findFreeParkingSpace()
        {
            return db.ParkingLots.Where(x => !x.Vehicles.Any()).First();
            //TODO throw exception if nothing is found... or other solution
        }

    }
}