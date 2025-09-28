using System.Data.Entity.Migrations;

using OrdersPortal.Infrastructure.EntityFramework;


namespace OrdersPortal.WebUI.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<OrderPortalDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OrderPortalDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
