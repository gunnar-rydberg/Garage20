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
            vehicle.ParkingLots = new List<ParkingLot>();

            switch (vehicle.Type)
            {
                case VehicleType.Motorcycle:
                    {
                        vehicle.ParkingLots.Add(findFreeSubParkingSpace() ?? findFreeParkingSpace());
                        break;
                    }
                case VehicleType.Trucks:
                    {
                        var parkingSpaces = findFreeParkingSpace(3);
                        //TODO validate parking space 
                        parkingSpaces.ForEach(vehicle.ParkingLots.Add);
                        break;
                    }
                default:
                    {
                        vehicle.ParkingLots.Add(findFreeParkingSpace());
                        break;
                    }
            }

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
            foreach (var parkingSpace in vehicle.ParkingLots)
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
        private ParkingLot findFreeSubParkingSpace()
        {
            return db.ParkingLots.Where(x => x.Vehicles.Count < 3 &&
                                              x.Vehicles.All(y => y.Type == VehicleType.Motorcycle))
                                  .FirstOrDefault();
        }

        private List<ParkingLot> findFreeParkingSpace(int size)
        {
            // finx {size} number of free spaces adjacent to each other
            for (int i = 0; i < TotalCapacity; i++)
            {
                var q = db.ParkingLots.OrderBy(x => x.Id)
                                      .Skip(i)
                                      .Take(size);
                if (q.All(x => !x.Vehicles.Any()))
                    return q.ToList();
            }
            return null; //TODO Some error handling
        }


    }
}