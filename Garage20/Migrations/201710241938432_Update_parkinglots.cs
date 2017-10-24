namespace Garage20.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_parkinglots : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParkingLots", "MotorCycleId1", c => c.Int());
            AddColumn("dbo.ParkingLots", "MotorCycleId2", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParkingLots", "MotorCycleId2");
            DropColumn("dbo.ParkingLots", "MotorCycleId1");
        }
    }
}
