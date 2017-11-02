using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage20.Models;

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

            if(Detailed)
                return View("IndexDetailed",vehicles.ToList());
            else
                return View("Index", vehicles.ToList());

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
        public ActionResult Create()
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
        public ActionResult Create([Bind(Include = "Id,Type,RegNo,Color,NoWheels,Model,Brand,VehicleTypeId,MemberId")] Vehicle vehicle)
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
        public ActionResult Edit([Bind(Include = "Id,Type,RegNo,Color,NoWheels,Model,Brand,Date,VehicleTypeId,MemberId")] Vehicle vehicle)
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
        public ActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
