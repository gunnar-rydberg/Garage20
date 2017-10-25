namespace Garage20.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_parkinglots : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParkingLots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleParkingLots",
                c => new
                    {
                        Vehicle_Id = c.Int(nullable: false),
                        ParkingLot_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vehicle_Id, t.ParkingLot_Id })
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id, cascadeDelete: true)
                .ForeignKey("dbo.ParkingLots", t => t.ParkingLot_Id, cascadeDelete: true)
                .Index(t => t.Vehicle_Id)
                .Index(t => t.ParkingLot_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleParkingLots", "ParkingLot_Id", "dbo.ParkingLots");
            DropForeignKey("dbo.VehicleParkingLots", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.VehicleParkingLots", new[] { "ParkingLot_Id" });
            DropIndex("dbo.VehicleParkingLots", new[] { "Vehicle_Id" });
            DropTable("dbo.VehicleParkingLots");
            DropTable("dbo.ParkingLots");
        }
    }
}
