namespace OrdersPortal.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganizationsAndUserOrganizations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderPortalUserOrganizations",
                c => new
                    {
                        OrderPortalUserId = c.String(nullable: false, maxLength: 128),
                        OrganizationId = c.Int(nullable: false),
                        JoinedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderPortalUserId, t.OrganizationId })
                .ForeignKey("dbo.AspNetUsers", t => t.OrderPortalUserId, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId, cascadeDelete: true)
                .Index(t => t.OrderPortalUserId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false, identity: true),
                        OrganizationName = c.String(),
                    })
                .PrimaryKey(t => t.OrganizationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderPortalUserOrganizations", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.OrderPortalUserOrganizations", "OrderPortalUserId", "dbo.AspNetUsers");
            DropIndex("dbo.OrderPortalUserOrganizations", new[] { "OrganizationId" });
            DropIndex("dbo.OrderPortalUserOrganizations", new[] { "OrderPortalUserId" });
            DropTable("dbo.Organizations");
            DropTable("dbo.OrderPortalUserOrganizations");
        }
    }
}
