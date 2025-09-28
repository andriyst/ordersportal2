using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class Order
	{
		public Order()
		{
			this.Db1SOrderNumbers = new HashSet<Db1SOrderNumbers>();
            this.OrdersMessage = new HashSet<OrdersMessage>();
		}

		
		[Key]
		public int OrderId { get; set; }
		public string OrderNumber { get; set; }		
		public DateTime OrderDateCreate { get; set; }
		public DateTime? OrderDateProgress { get; set; }
		public DateTime? OrderDateComplete { get; set; }
		public string CustomerId { get; set; }
		public OrderPortalUser Customer { get; set; }
		public string ManagerId { get; set; }
		public OrderPortalUser Manager { get; set; }
		public string File { get; set; }
		public int StatusId { get; set; }
		public Status Status { get; set; }
		public double OrderNumberContructions { get; set; }

		public DateTime? LastMessageTime { get; set; }

		public virtual ICollection<Db1SOrderNumbers> Db1SOrderNumbers { get; set; }

		public virtual ICollection<OrdersMessage> OrdersMessage { get; set; }
	}
}