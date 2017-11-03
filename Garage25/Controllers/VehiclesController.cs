using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage20.Models;
using Garage20.Utility;

namespace Garage20.Controllers
{
    public class VehiclesController : Controller
    {
        private Garage20Context db = new Garage20Context();

        // GET: NEWVehicles
        public ActionResult Index(string regNo = "", int VehicleTypeId = 0, bool Detailed = false)
        {
            var vehicleTypeList = db.VehicleTypes.OrderBy(x => x.Name).ToList();
            vehicleTypeList.Insert(0, new VehicleType { Id = 0, Name = "Any vehicle type" });
            ViewBag.VehicleTypeId = new SelectList(vehicleTypeList, "Id", "Name");


            var vehicles = db.Vehicles.Include(v => v.Member).Include(v => v.VehicleType);
            if (regNo != "")
                vehicles = vehicles.Where(x => x.RegNo == regNo);
            if (VehicleTypeId != 0)
                vehicles = vehicles.Where(x => x.VehicleTypeId == VehicleTypeId);

            var vehicleList = new List<ListViewModel>();
            foreach (var v in vehicles)
                vehicleList.Add(new ListViewModel {
                    vehicle = v,
                    ParkingTime = DateTime.Now - v.Date, });
            
            if (Detailed)
                return View("IndexDetailed",vehicleList);
            else
                return View("Index", vehicleList);

        }

        //public ActionResult IndexDetailed(string regNo = "", int VehicleTypeId = 0)
        //{
        //    var vehicleTypeList = db.VehicleTypes.OrderBy(x => x.Name).ToList();
        //    vehicleTypeList.Insert(0, new VehicleType { Id = 0, Name = "Any vehicle type" });
        //    ViewBag.VehicleTypeId = new SelectList(vehicleTypeList, "Id", "Name");


        //    var vehicles = db.Vehicles.Include(v => v.Member).Include(v => v.VehicleType);
        //    if (regNo != "")
        //        vehicles = vehicles.Where(x => x.RegNo == regNo);
        //    if (VehicleTypeId != 0)
        //        vehicles = vehicles.Where(x => x.VehicleTypeId == VehicleTypeId);

        //    return View(vehicles.ToList());
        //}

        // GET: NEWVehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: NEWVehicles/Create
        public ActionResult Park()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName");
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name");
            return View();
        }

        // POST: NEWVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Park([Bind(Include = "Id,RegNo,Color,NoWheels,Model,Brand,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.Date = DateTime.Now;

                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: NEWVehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // POST: NEWVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RegNo,Color,NoWheels,Model,Brand,Date,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FirstName", vehicle.MemberId);
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "Id", "Name", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // GET: NEWVehicles/Delete/5
        public ActionResult Checkout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: NEWVehicles/Delete/5
        [HttpPost, ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);

            var receipt = ParkingLogic.CreateReceipt(vehicle);

            db.Vehicles.Remove(vehicle);
            db.SaveChanges();

            return View("Receipt", receipt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //public ActionResult Sort(string sortOrder, string column)
        //{
        //    var vehicles = new List<Vehicle>();
        //    //var vehicles = db.Vehicles.ToList().ToList();

        //    if (sortOrder == null || sortOrder == "desc")
        //    {
        //        switch (column)
        //        {
        //            case "type":
        //                vehicles = db.Vehicles.OrderByDescending(e => e.Type).ToList();
        //                break;
        //            case "regno":
        //                vehicles = db.Vehicles.OrderByDescending(e => e.RegNo).ToList();
        //                break;
        //            case "color":
        //                vehicles = db.Vehicles.OrderByDescending(e => e.Color).ToList();
        //                break;
        //            case "nowheels":
        //                vehicles = db.Vehicles.OrderByDescending(e => e.NoWheels).ToList();
        //                break;
        //            case "model":
        //                vehicles = db.Vehicles.OrderByDescending(e => e.Model).ToList();
        //                break;
        //            case "brand":
        //                vehicles = db.Vehicles.OrderByDescending(e => e.Brand).ToList();
        //                break;
        //            case "date":
        //                vehicles = db.Vehicles.OrderByDescending(e => e.Date).ToList();
        //                break;
        //        }
        //        ViewBag.SortTypeOrder = "asc";
        //     //   ViewBag.FirstNameSortIcon = "glyphicon glyphicon-sort-by-alphabet";
        //    }
        //    else
        //    {
        //        switch (column)
        //        {
        //            case "type":
        //                vehicles = db.Vehicles.OrderBy(e => e.Type).ToList();
        //                break;
        //            case "regno":
        //                vehicles = db.Vehicles.OrderBy(e => e.RegNo).ToList();
        //                break;
        //            case "color":
        //                vehicles = db.Vehicles.OrderBy(e => e.Color).ToList();
        //                break;
        //            case "nowheels":
        //                vehicles = db.Vehicles.OrderBy(e => e.NoWheels).ToList();
        //                break;
        //            case "model":
        //                vehicles = db.Vehicles.OrderBy(e => e.Model).ToList();
        //                break;
        //            case "brand":
        //                vehicles = db.Vehicles.OrderBy(e => e.Brand).ToList();
        //                break;
        //            case "date":
        //                vehicles = db.Vehicles.OrderBy(e => e.Date).ToList();
        //                break;
        //        }
        //        ViewBag.SortTypeOrder = "desc";
        //    //    ViewBag.FirstNameSortIcon = "glyphicon glyphicon-sort-by-alphabet-alt";
        //    }

        //    return View("Index", vehicles);
        //}
    }
}
