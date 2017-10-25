using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Garage20.Models
{
    public class Garage20Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Garage20Context() : base("name=Garage20Context")
        {
        }

        public System.Data.Entity.DbSet<Garage20.Models.Vehicle> Vehicles { get; set; }
        public DbSet<ParkingLot> ParkingLots { get; set; }

        // Example from MS Docs to setup a custom Join Table
        // https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-a-more-complex-data-model-for-an-asp-net-mvc-application
        //
        // Use if you want full controll over the Join Table (column names etc.)
        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                    .HasMany(c => c.ParkingLots)
                    .WithMany(i => i.Vehicles)
                    .Map(t => t.MapLeftKey("VehicleID")
                               .MapRightKey("ParkingLotID")
                               .ToTable("VehicleParkingLot"));
        }
        */

    }
}
