using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class VideoContentRepository : Repository<VideoContent>, IVideoContentRepository
	{
		public VideoContentRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}
	
	}
}
