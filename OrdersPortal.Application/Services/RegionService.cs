using System.Collections.Generic;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Application.Services
{
	public class RegionService : IRegionService
	{
		private readonly IRegionRepository _regionRepository;

		public RegionService(IRegionRepository regionRepository)
		{
			_regionRepository = regionRepository;
		}

		public List<Region> GetRegionList()
		{
			return _regionRepository.GetList();
		}

		public void AddRegion(Region region)
		{
			_regionRepository.AddPermanent(region);
		}

		public void RemoveRegion(int regionId)
		{
			var region = _regionRepository.GetById(regionId);
			if (region != null)
			{
				_regionRepository.Remove(region);
				_regionRepository.SaveChanges();
			}
		}

		public void EditRegion(Region region)
		{
			var result = _regionRepository.GetById(region.RegionId);
			result.RegionName = region.RegionName;
			_regionRepository.UpdatePermanent(result);
		}
	}
}
