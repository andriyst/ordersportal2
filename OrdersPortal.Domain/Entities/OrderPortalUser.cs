using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OrdersPortal.Domain.Entities
{
	public class OrderPortalUser : IdentityUser
	{
		public OrderPortalUser()
		{
			this.Order = new HashSet<Order>();
			this.HelpServiceLog = new HashSet<HelpServiceLog>();
			this.OrderPortalUserOrganizations = new HashSet<OrderPortalUserOrganization>();
		}

		public string FullName { get; set; }
		//public string EMail { get; set; }
		public string Tel { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? RegisterDate { get; set; }
		public DateTime? ExpireDate { get; set; }
		public Boolean Enable { get; set; }
		public int? RegionId { get; set; }
		public Region Region { get; set; }


		public ICollection<OrderPortalUserOrganization> OrderPortalUserOrganizations { get; set; }


		public virtual ICollection<Order> Order { get; set; }
		public virtual ICollection<HelpServiceLog> HelpServiceLog { get; set; }

		public virtual Customer Customer { get; set; }
		public virtual Manager Manager { get; set; }
		public virtual RegionManager RegionManager { get; set; }
		public virtual Director Director { get; set; }
		public virtual Operator Operator { get; set; }
		public virtual Admin Admin { get; set; }

	}
}