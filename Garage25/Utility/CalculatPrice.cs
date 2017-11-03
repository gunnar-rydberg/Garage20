using Garage20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage20.Utility
{
    public static class ParkingLogic
    {
        public static int HOURLY_PRICE_PER_PARKING_LOT = 2;
       

        public static Receipt CreateReceipt (Vehicle vehicle)
        {
            var receipt = new Models.Receipt()
            {
                CheckoutTimestamp = DateTime.Now,
                Vehicle = vehicle,
            };
            receipt.TotalParkingTime = receipt.CheckoutTimestamp - receipt.Vehicle.Date;
            
            receipt.Price = (int)Math.Ceiling(receipt.TotalParkingTime.TotalHours) * HOURLY_PRICE_PER_PARKING_LOT;

            return receipt;
        }
    }
}
