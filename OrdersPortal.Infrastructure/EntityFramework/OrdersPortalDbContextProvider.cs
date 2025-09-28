using System.Data.Entity;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.EntityFramework
{
	
	public class OrdersPortalDbContextProvider : IDbContextProvider
	{
		public DbContext DbContext { get; private set; }
		public OrdersPortalDbContextProvider(string nameOrConnectionString)
		{
			DbContext = new OrderPortalDbContext(nameOrConnectionString);
		}

		public OrdersPortalDbContextProvider(DbContext dbContext)
		{
			DbContext = dbContext;
		}
	}
}
