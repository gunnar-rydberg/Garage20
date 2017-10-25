using Garage20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garage20.Utility;

namespace Garage20.Controllers
{
    public class StatisticsController : Controller
    {
        private Garage20Context db = new Garage20Context();

        // GET: Statistics
        public ActionResult Index()
        {
            Statistics stats = new Statistics();

            stats.Wheels = db.Vehicles.Sum(v => v.NoWheels);


            var dates = db.Vehicles.Select(v => v.Date);
            DateTime now = DateTime.Now;

            foreach (var d in dates)
            {
                stats.Time += now - d;
            }


            var vehicleTypes = db.Vehicles.Select(v => v.Type).Distinct();

            foreach (var t in vehicleTypes)
            {
                int n = db.Vehicles.Count(v => v.Type == t);
                stats.Types.Add(t.ToString(), n);
            }


            stats.Price = (int)Math.Ceiling(stats.Time.TotalHours) * CalculatPrice.HOURLY_PRICE;


            return View(stats);
        }
    }
}