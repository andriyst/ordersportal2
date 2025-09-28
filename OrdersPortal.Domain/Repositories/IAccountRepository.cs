using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IAccountRepository : IRepository<OrderPortalUser>
	{
		Customer GetCustomerByContrCode(int contrCode);
		Manager GetManagerByGuid(string guid);
		RegionManager GetRegionManagerByGuid(string guid);
		OrderPortalUser GetByUserName(string userName);
		OrderPortalUser GetByIdIncludes(string id);
	}
}
