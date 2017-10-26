namespace Garage20.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage20.Models.Garage20Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Garage20.Models.Garage20Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            const int GARAGE_SIZE = 20;

            for (int i = 1; i <= GARAGE_SIZE; i++)
            {
                if (!context.ParkingLots.Any(x => x.Id == i))
                {
                    context.ParkingLots.AddOrUpdate(
                        x => x.Id,
                        new Models.ParkingLot()
                        {
                            Id = i,
                            Description = $"Parking {i}",
                            Vehicles = new List<Models.Vehicle>()
                        });
                }
            }
            context.SaveChanges();

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
                    Date = new DateTime(2000, 12, 16),
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
                }
            );
            context.SaveChanges();

            //Assign vehicle to parking space

            for (int i = 1; i <= 4; i++)
            {
                context.Vehicles.Single(x => x.Id == i)
                       .ParkingLots.Add(context.ParkingLots.Single(x => x.Id == i));
            }
        }
    }
}
