namespace Garage20.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTypeEnum : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vehicles", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Type", c => c.Int(nullable: false));
        }
    }
}
