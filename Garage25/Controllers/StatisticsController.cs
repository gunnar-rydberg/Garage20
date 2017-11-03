using Garage20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Garage20.Utility;
using System.Web.Helpers;

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

            stats.Price = (int)Math.Ceiling(stats.Time.TotalHours) * ParkingLogic.HOURLY_PRICE_PER_PARKING_LOT;

            return View(stats);
        }

    //    public ActionResult ChartView()
    //    {
    //        Statistics stats = new Statistics();

    //        var vehicleTypes = db.Vehicles.Select(v => v.Type).Distinct();

    //        foreach (var t in vehicleTypes)
    //        {
    //            int n = db.Vehicles.Count(v => v.Type == t);
    //            stats.Types.Add(t.ToString(), n);
    //        }

    //        return View(stats);
    //    }

    //    public ActionResult Chart()
    //    {
    //        var vehicleTypes = db.Vehicles.Select(v => v.Type).Distinct();
    //        List<int> counts = new List<int>();

    //        foreach (var t in vehicleTypes)
    //        {
    //            int n = db.Vehicles.Count(v => v.Type == t);
    //            counts.Add(n);
    //        }

    //        var chart = new Chart(width: 600, height: 400)
    //            .AddTitle("Chart Title")
    //            .AddSeries(
    //                name: "Types",
    //                xValue: vehicleTypes.ToArray(),
    //                yValues: counts.ToArray())
    //            .GetBytes("png");
    //            //.Write();

    //        return File(chart, "image/bytes");
    //    }


    //    public ActionResult CreateCountryGDPPie()
    //    {
    //        var chart = new Chart(width: 300, height: 200, theme: ChartTheme.Vanilla)
    //        .AddTitle("GDP Current Prices in Billion($)")
    //        .AddLegend()
    //        .AddSeries(chartType: "pie",
    //                    xValue: new[] { "USA", "China", "Japan", "Germany", "UK", "France", "India"},
    //                    yValues: new[] { "17968", "11385", "4116", "3371", "2865", "2423", "2183" })
    //        .GetBytes("png");
    //        return File(chart, "image/bytes");
    //    }
    }
}