namespace AutoScout.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MileageToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vehicles", "Mileage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vehicles", "Mileage", c => c.String());
        }
    }
}
