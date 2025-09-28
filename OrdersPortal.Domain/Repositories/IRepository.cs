using System.Linq;

using OrdersPortal.Domain.Helpers;

namespace OrdersPortal.Domain.Repositories
{
	public interface IRepository<T>
		where T : class /*, IEntity*/
	{
		IQueryable<T> GetAll();
		T GetById(object id);
		void Add(T newEntity);
		void AddPermanent(T newEntity);
		void Remove(T entity);
		void Update(T entity);
		void UpdatePermanent(T entity);
		void SaveChanges();
		IQueryable<T> GetSortAndSearchList(TableDataModel tableDataModel);

	}
}
