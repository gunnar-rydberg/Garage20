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

        public ActionResult GetChartImage()
        {
            var key = new Chart(width: 300, height: 300)
                .AddTitle("Employee Chart")
                .AddSeries(
                chartType: "Bubble",
                name: "Employee",
                xValue: new[] { "Peter", "Andrew", "Julie", "Dave" },
                yValues: new[] { "2", "7", "5", "3" });

            return File(key.ToWebImage().GetBytes(), "image/jpeg");
        }

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

            var vehicleTypes = db.Vehicles.Select(v => v.VehicleType).Distinct();

            foreach (var t in vehicleTypes)
            {
                int n = db.Vehicles.Count(v => v.VehicleType.Name == t.Name);
                stats.Types.Add(t.Name, n);
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

        public ActionResult Chart()
        {
            var vehicleTypes = db.Vehicles.Select(v => v.VehicleType.Name).Distinct();
            List<int> counts = new List<int>();

            foreach (var t in vehicleTypes)
            {
                int n = db.Vehicles.Count(v => v.VehicleType.Name == t);
                counts.Add(n);
            }

            var chart = new Chart(width: 300, height: 200)
                .AddTitle("Vehicle Types")
                .AddSeries(
                    name: "Types",
                    chartType: "Bar",
                    xValue: vehicleTypes.ToArray(),
                    yValues: counts.ToArray());
                //.GetBytes("png");
            //.Write();

            //return File(chart, "image/bytes");
            return File(chart.ToWebImage().GetBytes(), "image/png");
        }


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