using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Entities
{
	public class OrderPortalUserOrganization
	{
		[Key, Column(Order = 0)]
		public string OrderPortalUserId { get; set; }

		[Key, Column(Order = 1)]
		public int OrganizationId { get; set; }

		public DateTime JoinedAt { get; set; }

		[ForeignKey("OrderPortalUserId")]
		public virtual OrderPortalUser OrderPortalUser { get; set; }

		[ForeignKey("OrganizationId")]
		public virtual Organization Organization { get; set; }
	}
}
