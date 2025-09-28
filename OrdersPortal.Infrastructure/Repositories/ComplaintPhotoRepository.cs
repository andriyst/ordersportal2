using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class ComplaintPhotoRepository: Repository<ComplaintPhoto>, IComplaintPhotoRepository
	{
		public ComplaintPhotoRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}
	}
}
