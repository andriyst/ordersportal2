
namespace OrdersPortal.Domain.Repositories
{
	public interface IUnitOfWork
	{
		void Begin();
		void Commit();
	}
}
