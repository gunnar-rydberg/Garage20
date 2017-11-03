namespace Garage20.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Telephone = c.String(),
                        Mail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        RegNo = c.String(nullable: false),
                        Color = c.String(),
                        NoWheels = c.Int(nullable: false),
                        Model = c.String(),
                        Brand = c.String(),
                        Date = c.DateTime(nullable: false),
                        VehicleTypeId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleTypeId, cascadeDelete: true)
                .Index(t => t.VehicleTypeId)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.ParkingLots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        NumberOfParkingLots = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParkingLotVehicles",
                c => new
                    {
                        ParkingLot_Id = c.Int(nullable: false),
                        Vehicle_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParkingLot_Id, t.Vehicle_Id })
                .ForeignKey("dbo.ParkingLots", t => t.ParkingLot_Id, cascadeDelete: true)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id, cascadeDelete: true)
                .Index(t => t.ParkingLot_Id)
                .Index(t => t.Vehicle_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "VehicleTypeId", "dbo.VehicleTypes");
            DropForeignKey("dbo.ParkingLotVehicles", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.ParkingLotVehicles", "ParkingLot_Id", "dbo.ParkingLots");
            DropForeignKey("dbo.Vehicles", "MemberId", "dbo.Members");
            DropIndex("dbo.ParkingLotVehicles", new[] { "Vehicle_Id" });
            DropIndex("dbo.ParkingLotVehicles", new[] { "ParkingLot_Id" });
            DropIndex("dbo.Vehicles", new[] { "MemberId" });
            DropIndex("dbo.Vehicles", new[] { "VehicleTypeId" });
            DropTable("dbo.ParkingLotVehicles");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.ParkingLots");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Members");
        }
    }
}
