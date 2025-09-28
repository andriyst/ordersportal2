using System.Linq;
using System.Data.Entity;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class AccountRepository : Repository<OrderPortalUser>, IAccountRepository
	{
		public AccountRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public Customer GetCustomerByContrCode(int contrCode)
		{
			return DbSet.Select(x => x.Customer).Include(f => f.OrderPortalUser).FirstOrDefault(y => y.CustomerContrCode == contrCode);
		}
		public Manager GetManagerByGuid(string guid)
		{
			return DbSet.Select(x => x.Manager).Include(f => f.OrderPortalUser).FirstOrDefault(y => y.Guid == guid);
		}
		public RegionManager GetRegionManagerByGuid(string guid)
		{
			return DbSet.Select(x => x.RegionManager).Include(f => f.OrderPortalUser).FirstOrDefault(y => y.Guid == guid);
		}
		public OrderPortalUser GetByUserName(string userName)
		{
			return DbSet.Include(x => x.Customer)
						.Include(x => x.Manager)
						.Include(x => x.OrderPortalUserOrganizations.Select(o => o.Organization))
						.Include(x => x.RegionManager)
						.Include(x => x.Director)
						.Include(x => x.Operator)
						.Include(x => x.Admin)
						.Include(x => x.Region)
						.FirstOrDefault(y => y.UserName == userName);
		}
		public OrderPortalUser GetByIdIncludes(string id)
		{
			var result = DbSet.Include(x => x.Customer)
						.Include(x => x.Manager)
						.Include(x => x.OrderPortalUserOrganizations.Select(o => o.Organization))
						.Include(x => x.RegionManager)
						.Include(x => x.Director)
						.Include(x => x.Operator)
						.Include(x => x.Admin)
						.Include(x => x.Region)
						.FirstOrDefault(y => y.Id == id);

			return result;
		}

	}
}
