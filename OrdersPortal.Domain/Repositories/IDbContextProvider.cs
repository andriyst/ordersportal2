using System.Data.Entity;

namespace OrdersPortal.Domain.Repositories
{
	public interface IDbContextProvider
	{
		DbContext DbContext { get; }
	}
}
