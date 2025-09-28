using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class Organization
	{
		public Organization()
		{
			this.OrderPortalUserOrganizations = new HashSet<OrderPortalUserOrganization>();
		}
		[Key]
		public int OrganizationId { get; set; }

		public string OrganizationName { get; set; }
		public string Organization1cId { get; set; }

		public virtual ICollection<OrderPortalUserOrganization> OrderPortalUserOrganizations { get; set; }

	}
}
