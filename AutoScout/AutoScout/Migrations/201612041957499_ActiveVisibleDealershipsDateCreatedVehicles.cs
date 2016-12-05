namespace AutoScout.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActiveVisibleDealershipsDateCreatedVehicles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dealerships", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Vehicles", "Visible", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vehicles", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "Active");
            DropColumn("dbo.Vehicles", "Visible");
            DropColumn("dbo.Dealerships", "DateCreated");
        }
    }
}
