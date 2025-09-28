using System.Data.Entity;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DbContext _dbContext;
		public string ConnectionString { get; private set; }

		public UnitOfWork(IDbContextProvider dbContextProvider)
		{
			_dbContext = dbContextProvider.DbContext;
		}
		public void Begin()
		{

		}

		public void Commit()
		{
			_dbContext.SaveChanges();
		}
		public DbContext DbContext
		{
			get { return _dbContext; }
		}
	}
}
