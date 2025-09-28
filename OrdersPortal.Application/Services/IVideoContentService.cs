using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Services
{
	public interface IVideoContentService
	{
		List<VideoContent> GetList();
		void Add(VideoContent region);
		void Remove(int regionId);
		void Edit(VideoContent region);
	}
}
