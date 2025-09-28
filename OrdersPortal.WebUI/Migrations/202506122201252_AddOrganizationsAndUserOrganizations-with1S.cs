namespace OrdersPortal.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganizationsAndUserOrganizationswith1S : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "Organization1cId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "Organization1cId");
        }
    }
}
