namespace Garage20.Migrations
{
    using Garage20.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage20.Models.Garage20Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Garage20.Models.Garage20Context db)
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
            var vehicleTypes = new[]
            {
                new VehicleType {Name="Car", NumberOfParkingLots=1m},
                new VehicleType {Name="Light truck", NumberOfParkingLots=2m},
                new VehicleType {Name="Medium truck", NumberOfParkingLots=3m},
                new VehicleType {Name="Heavy truck", NumberOfParkingLots=4m},
                new VehicleType {Name="Motorcycle", NumberOfParkingLots=0.33m},
            };
            db.VehicleTypes.AddOrUpdate(
                    x => x.Name,
                    vehicleTypes);

            var members = new[]
            {
                new Member { FirstName="Adam", LastName="Adamsson", Telephone="999 12345678" , Mail="foo@bar.com"},
                new Member { FirstName="Berit", LastName="Brül", Telephone="999 7777777" , Mail="foo2@bar.com"},
            };
            db.Members.AddOrUpdate(
                x => new { x.FirstName, x.LastName },
                members);
            db.SaveChanges();

            var GARAGE_SIZE = 20;
            for (int i = 1; i <= GARAGE_SIZE; i++)
            {
                db.ParkingLots.AddOrUpdate(
                    x => x.Id,
                    new ParkingLot { Id = i, Name = $"P {i}" });
            }

            var vehicles = new[]
            {
                new Vehicle {RegNo="ABC-123", Brand="Saab", Model="9000", Date=new DateTime(2017,11,01),
                             Type=VehicleTypeEnum.Car,
                             MemberId=members.First(x => x.FirstName == "Adam").Id,
                             VehicleTypeId=vehicleTypes.First(x => x.Name == "Car").Id
                             }
            };
            db.Vehicles.AddOrUpdate(
                x => x.RegNo,
                vehicles);
            db.SaveChanges();

            // Add vehicle(s) to parking lot(s)
            db.Vehicles.First(x => x.RegNo == "ABC-123")
              .ParkingLots.Add(db.ParkingLots.First(x => x.Name == "P 1"));
            db.SaveChanges();



        }
    }
}
