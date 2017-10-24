namespace Garage20.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage20.Models.Garage20Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Garage20Context";
        }

        protected override void Seed(Garage20.Models.Garage20Context context)
        {

            context.Vehicles.AddOrUpdate(
                x => x.Id,
                new Models.Vehicle()
                {
                    Id = 1,
                    RegNo = "AAA-111",
                    Type = VehicleType.Car,
                    Brand = "Saab",
                    Model = "9000",
                    Color = "Silver",
                    NoWheels = 4,
                    Date = new DateTime(2000, 12, 16)
                },
                new Models.Vehicle()
                {
                    Id = 2,
                    RegNo = "BBB-111",
                    Type = VehicleType.Car,
                    Brand = "Saab",
                    Model = "900",
                    Color = "Silver",
                    NoWheels = 4,
                    Date = new DateTime(1999, 12, 16)
                },
                new Models.Vehicle()
                {
                    Id = 3,
                    RegNo = "BBB-222",
                    Type = VehicleType.Car,
                    Brand = "Volkswagen",
                    Model = "Golf",
                    Color = "Red",
                    NoWheels = 4,
                    Date = new DateTime(2017, 10, 23)
                },
                new Models.Vehicle()
                {
                    Id = 4,
                    RegNo = "ZZZ-222",
                    Type = VehicleType.Motorcycle,
                    Brand = "Yamaha",
                    Model = "Tracer 700",
                    Color = "Red",
                    NoWheels = 2,
                    Date = new DateTime(2017, 10, 23)
                });

            const int NUMBER_OF_PARKINGSPACES = 100;

            for (int i = 0; i < NUMBER_OF_PARKINGSPACES; i++)
            {
                var vehicleId = context.Vehicles.OrderBy(x => x.Id)
                                                .Select(x => (int?)x.Id)
                                                .Skip(i)
                                                .DefaultIfEmpty(null)
                                                .First();
                context.Parking.AddOrUpdate(
                    x => x.Id,
                    new Models.ParkingLot() { Id = i, VehicleId = vehicleId });

            }
        }
    }
}
