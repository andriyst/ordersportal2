using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class Region
	{
		public Region()
		{
			this.OrderPortalUser = new HashSet<OrderPortalUser>();
		}

		[Key]
		public int RegionId { get; set; }
		public string RegionName { get; set; }

		public virtual ICollection<OrderPortalUser> OrderPortalUser { get; set; }
	}
}