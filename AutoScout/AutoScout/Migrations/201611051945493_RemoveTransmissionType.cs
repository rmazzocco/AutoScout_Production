namespace AutoScout.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTransmissionType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vehicles", "TransmissionType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "TransmissionType", c => c.String());
        }
    }
}
