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
        private Garage20Context db;

        public GarageHandler(Garage20Context dbContext)
        {
            db = dbContext;
        }

        public int FreeCapacity => TotalCapacity - db.ParkingLots.Where(x => x.Vehicles.Any()).Count();
        public int TotalCapacity => db.ParkingLots.Count();

        /// <summary>
        /// Park vehicle in garage
        /// </summary>
        /// <param name="vehicle"></param>
        public void Park(Vehicle vehicle)
        {
            vehicle.ParkingLots = new List<ParkingLot>();
            var size = db.VehicleTypes.Find(vehicle.VehicleTypeId).NumberOfParkingLots;
            var parkingLots = findFreeParkingSpace(size);
            parkingLots.ForEach(vehicle.ParkingLots.Add);

            db.Vehicles.Add(vehicle);
            db.SaveChanges();
        }

        /// <summary>
        /// Remove vehicle from garage, clearing parking space(s)
        /// </summary>
        /// <param name="vehicle"></param>
        public void CheckOut(Vehicle vehicle)
        {
            foreach (var parkingSpace in vehicle.ParkingLots)
            {
                parkingSpace.Vehicles.Remove(vehicle);
                //TODO? throw exception if ParkingLots is empty on checkout
            }

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
        }

        /// <summary>
        /// Find suitable parking space(s) for vehicle of given size
        /// </summary>
        private List<ParkingLot> findFreeParkingSpace(decimal size)
        {
            var parkingLots = new List<ParkingLot>();

            if(size == 1m)
            {
                var parkingLot = findFreeParkingSpace();
                if(parkingLot != null)
                    parkingLots.Add(parkingLot);
            }
            else if (size < 1m)
            {
                var parkingLot = findFreeParkingSubSpace(size) ?? findFreeParkingSpace();
                if (parkingLot != null)
                    parkingLots.Add(parkingLot);
            }
            else if (size > 1m)
            {
                parkingLots.AddRange(findFreeParkingSpaceMultiple((int)Decimal.Ceiling(size)));
            }
            return parkingLots;
        }

        /// <summary>
        /// Find exactly on free parking lot 
        /// </summary>
        private ParkingLot findFreeParkingSpace()
        {
            return db.ParkingLots.Where(x => !x.Vehicles.Any()).First();
            //TODO throw exception if nothing is found... or other solution
        }

        /// <summary>
        /// Find already used parking lot space where a smaller vehicle can fit
        /// </summary>
        private ParkingLot findFreeParkingSubSpace(decimal size)
        {
            var usedSpaces = db.ParkingLots.Where(x => x.Vehicles.Any())
                                  .Select(x => new { Id = x.Id, Sizes = x.Vehicles.Select(y => y.VehicleType.NumberOfParkingLots) })
                                  .ToList();

            foreach (var parkingSpace in usedSpaces)
            {
                var spaceSum = size;
                foreach (var s in parkingSpace.Sizes)
                    spaceSum += s;
                if (spaceSum <= 1m)
                    return db.ParkingLots.Where(x => x.Id == parkingSpace.Id).First();
            }
            return null;
        }

        /// <summary>
        /// find free parking spaces adjacent to each other (use for larger vehicles)
        /// </summary>
        private List<ParkingLot> findFreeParkingSpaceMultiple(int size)
        {
            //TODO? query db for a List<{Id, bool}> based on free parking space
            //      then search for adjacent free space in that local list
            for (int i = 0; i < TotalCapacity; i++)
            {
                var q = db.ParkingLots.OrderBy(x => x.Id)
                                      .Skip(i)
                                      .Take(size);
                if (q.All(x => !x.Vehicles.Any()))
                    return q.ToList();
            }
            return new List<ParkingLot>(); 
        }

    }
}