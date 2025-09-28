using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class Status
	{
		public Status()
		{
			this.Order = new HashSet<Order>();
            this.Complaint = new HashSet<Complaint>();
		}
		[Key]
		public int StatusId { get; set; }
		public string StatusName { get; set; }

		public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<Complaint> Complaint { get; set; }
	}
}