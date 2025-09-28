using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Services
{
	public interface IRegionService
	{
		List<Region> GetRegionList();
		void AddRegion(Region region);
		void RemoveRegion(int regionId);
		void EditRegion(Region region);
	}
}
