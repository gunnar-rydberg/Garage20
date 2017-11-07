using Garage20.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public void Park(Vehicle vehicle, int firstParkingLotId)
        {
            vehicle.ParkingLots = new List<ParkingLot>();

            var size = (int)Decimal.Ceiling(db.VehicleTypes.Find(vehicle.VehicleTypeId).NumberOfParkingLots);
            var parkingLotIds = Enumerable.Range(firstParkingLotId, size);
            var parkingLots = db.ParkingLots.Where(x => parkingLotIds.Contains(x.Id))
                                            .ToList();
            //TODO Validate that the selected parkinglots are free and throw error if they are not

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

        private class SelectListData
        {
            public string Value { get; set; } // Despite what SelectList docs say these must be properties, not fields.
            public string Name { get; set; }
        }

        /// <summary>
        /// Get SelectList (for HTML5 dropdown) of all types in vehicleTypes table
        /// </summary>
        public SelectList GetVehicleTypes(string emptyValueName = null)
        {
            var vehicleTypes = new List<SelectListData>();
            if (emptyValueName != null)
                vehicleTypes.Add(new SelectListData { Value = "", Name = emptyValueName });

            foreach (var t in db.VehicleTypes.OrderBy(x => x.Name))
                vehicleTypes.Add(new SelectListData { Value = t.Id.ToString(), Name = t.Name });

            return new SelectList(vehicleTypes, "Value", "Name");
        }

        /// <summary>
        /// Get SelectList (for HTML5 dropdown) of all available parking lots
        /// </summary>
        public SelectList GetParkingLots(int vehicleTypeId, string emptyValueName = null)
        {
            var parkingLotSelectList = new List<SelectListData>();

            if (emptyValueName != null)
                parkingLotSelectList.Add(new SelectListData { Value = "", Name = emptyValueName });

            var size = db.VehicleTypes.Find(vehicleTypeId).NumberOfParkingLots;
            //TODO Raise error on nonexisting vehicleType ?

            if (size <= 1m)
            {
                var parkingLots = db.ParkingLots.Where(x => !x.Vehicles.Any());
                foreach (var lot in parkingLots)
                    parkingLotSelectList.Add(new SelectListData { Value= lot.Id.ToString(), Name = lot.Name });
            }
            if (size < 1m)
            {
                foreach (var lot in findFreeParkingSubSpace2(size))
                    parkingLotSelectList.Add(new SelectListData { Value= lot.Id.ToString(), Name = lot.Name + " Shared" });
                
                var padding = parkingLotSelectList.Max(x => x.Value.Length);
                parkingLotSelectList = parkingLotSelectList.OrderBy(x => x.Value.PadLeft(padding,'0')).ToList();
            }
            if (size > 1m)
            {
                foreach (var lots in findFreeParkingSpaceMultiple2((int)Decimal.Ceiling(size)))
                {
                    var id = lots.First().Id;
                    string name;
                    if (lots.Count() == 2)
                        name = lots[0].Name + " and " + lots[1].Name;
                    else
                        name = lots[0].Name + " through " + lots.Last().Name;

                    parkingLotSelectList.Add(new SelectListData { Value = id.ToString(), Name = name });
                }
            }

            if ((emptyValueName != null && parkingLotSelectList.Count == 1) ||
               (emptyValueName == null && parkingLotSelectList.Count == 0))
                parkingLotSelectList = new List<SelectListData> { new SelectListData { Value = "", Name = "No vacant space" } };

            return new SelectList(parkingLotSelectList, "Value", "Name");
        }

        /// <summary>
        /// Find already used parking lot space where a smaller vehicle can fit
        /// </summary>
        private IEnumerable<ParkingLot> findFreeParkingSubSpace2(decimal size)
        {
            //TODO write this fantastic method using LINQ count()
            var usedSpaces = db.ParkingLots.Where(x => x.Vehicles.Any())
                                  .Select(x => new { Id = x.Id, Sizes = x.Vehicles.Select(y => y.VehicleType.NumberOfParkingLots) })
                                  .ToList();

            foreach (var parkingSpace in usedSpaces)
            {
                var spaceSum = size;
                foreach (var s in parkingSpace.Sizes)
                    spaceSum += s;
                if (spaceSum <= 1m)
                    yield return db.ParkingLots.Where(x => x.Id == parkingSpace.Id).First();
            }
        }

        private IEnumerable<List<ParkingLot>> findFreeParkingSpaceMultiple2(int size)
        {
            //TODO? query db for a List<{Id, bool}> based on free parking space
            //      then search for adjacent free space in that local list


            for (int i = 0; i < TotalCapacity - size; i++)
            {
                var q = db.ParkingLots.OrderBy(x => x.Id)
                                      .Skip(i)
                                      .Take(size);
                if (q.All(x => !x.Vehicles.Any()))
                    yield return q.ToList();
            }
        }

        /// <summary>
        /// Find suitable parking space(s) for vehicle of given size
        /// </summary>
        private List<ParkingLot> findFreeParkingSpace(decimal size)
        {
            var parkingLots = new List<ParkingLot>();

            if (size == 1m)
            {
                var parkingLot = findFreeParkingSpace();
                if (parkingLot != null)
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