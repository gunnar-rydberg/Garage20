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
        private const decimal HOURLY_PRICE = 10;

        private Garage20Context db = new Garage20Context();

        // GET: Vehicles
        public ActionResult Index(string search = "", string searchBrand = "", string searchModel = "")
        {
            var query = Enumerable.Empty<Vehicle>().AsQueryable();
            query = db.Vehicles;
            if (search != "")
                query = db.Vehicles.Where(x => x.RegNo.Contains(search));
            if (searchBrand != "")
                query = query.Where(x => x.Brand.Contains(searchBrand));
            if (searchModel != "")
                query = query.Where(x => x.Model.Contains(searchModel));

            return View(query.ToList());

        }

        // GET: Vehicles/Details/5
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

        // GET: Vehicles/Create
        public ActionResult Park()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Park([Bind(Include = "Id,Type,RegNo,Color,NoWheels,Model,Brand")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.Date = DateTime.Now;
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
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
            return View(vehicle);
        }

       

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,RegNo,Color,NoWheels,Model,Brand,Date")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public ActionResult CheckOut(int? id)
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

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("CheckOutConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOutConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);

            var receipt = new Models.Receipt()
            {
                CheckoutTimestamp = DateTime.Now,
                Vehicle = vehicle,
            };
            receipt.TotalParkingTime = receipt.CheckoutTimestamp - receipt.Vehicle.Date;
            receipt.Price =  (int)Math.Ceiling(receipt.TotalParkingTime.TotalHours) * HOURLY_PRICE;



            //db.Vehicles.Remove(vehicle);
            //db.SaveChanges();

            return View("Receipt", receipt);
            //return RedirectToAction("Receipt", receipt);
            //return RedirectToAction("Index");
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
